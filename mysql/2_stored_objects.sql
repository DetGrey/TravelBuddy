USE travel_buddy;

# INDEX
-- Date + FK'er
CREATE INDEX idx_td_dates on trip_destination (start_date, end_date);
CREATE INDEX idx_td_trip on trip_destination (trip_id);
CREATE Index idx_td_destination on trip_destination(destination_id);

-- Location
CREATE INDEX idx_dest_contry_state ON destination (country,state);

-- Buddies (count)
CREATE INDEX idx_buddy_seg_status on buddy (trip_destination_id, request_status);

# GLOBAL FUNCTIONS
DROP FUNCTION IF EXISTS get_user_id_from_buddy;
DELIMITER $$
CREATE FUNCTION get_user_id_from_buddy(f_buddy_id INT)
RETURNS INT
DETERMINISTIC
BEGIN
    RETURN (
        SELECT user_id
        FROM buddy
        WHERE buddy_id = f_buddy_id
        LIMIT 1
    );
END $$
DELIMITER ;

DROP FUNCTION IF EXISTS get_group_conversation_for_buddy;
DELIMITER $$
CREATE FUNCTION get_group_conversation_for_buddy(f_buddy_id INT)
RETURNS INT
DETERMINISTIC
BEGIN
    DECLARE v_trip_id INT;
    DECLARE v_conversation_id INT;

    -- Get trip destination for buddy
    SELECT trip_destination_id
    INTO v_trip_id
    FROM buddy
    WHERE buddy_id = f_buddy_id;

    -- Get group conversation for that trip
    SELECT conversation_id
    INTO v_conversation_id
    FROM conversation
    WHERE is_group = TRUE
    AND trip_destination_id = v_trip_id
    LIMIT 1;

    RETURN v_conversation_id;
END $$
DELIMITER ;

DROP FUNCTION IF EXISTS get_owner_id_from_trip;
DELIMITER $$
CREATE FUNCTION get_owner_id_from_trip (f_trip_id INT)
RETURNS INT
DETERMINISTIC
READS SQL DATA
BEGIN
    DECLARE v_owner_id INT;
    SELECT t.owner_id INTO v_owner_id
    FROM trip t
    WHERE trip_id = f_trip_id
    LIMIT 1;

    RETURN v_owner_id;
END $$
DELIMITER ;

DROP FUNCTION IF EXISTS get_trip_destination_remaining_capacity;
DELIMITER $$
CREATE FUNCTION get_trip_destination_remaining_capacity (f_trip_destination_id INT)
RETURNS INT
DETERMINISTIC
BEGIN
    DECLARE v_trip_id INT;
    DECLARE v_max_buddies INT;
    DECLARE v_accepted_buddies INT;
    DECLARE v_remaining_capacity INT;

    SELECT trip_id INTO v_trip_id
    FROM trip_destination
    WHERE trip_destination_id = f_trip_destination_id
    LIMIT 1;

    IF v_trip_id IS NULL THEN
        RETURN NULL;
    END IF;

    SELECT max_buddies INTO v_max_buddies
    FROM trip
    WHERE trip_id = v_trip_id
    LIMIT 1;

    SELECT SUM(b.person_count) INTO v_accepted_buddies
    FROM buddy b
    WHERE b.trip_destination_id = f_trip_destination_id
        AND b.request_status = 'accepted';

    SET v_remaining_capacity = COALESCE(v_max_buddies, 0) - COALESCE(v_accepted_buddies, 0);

    IF v_remaining_capacity < 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Remaining capacity is negative — data inconsistency detected';
    END IF;

    RETURN v_remaining_capacity;
END $$
DELIMITER ;

# 1. User can search for a trip destination using start/end dates, buddy count and location.
# Maybe also description – for specific activities.
DROP PROCEDURE IF EXISTS search_trips;
DELIMITER $$
CREATE PROCEDURE search_trips (
    IN in_req_start DATE,
    IN in_req_end DATE,
    IN in_country VARCHAR(100),
    IN in_state VARCHAR(100),
    IN in_name VARCHAR(200),
    IN in_party_size INT,
    IN in_q VARCHAR(255)
)
BEGIN
    DECLARE v_party_size INT;
    SET v_party_size = IF(in_party_size IS NULL OR in_party_size < 1, 1, in_party_size);

    SELECT
        td.trip_destination_id AS TripDestinationId,
        t.trip_id AS TripId,
        d.destination_id AS DestinationId,
        d.name AS DestinationName,
        d.country AS Country,
        d.state AS State,
        td.start_date AS DestinationStart,
        td.end_date AS DestinationEnd,
        COALESCE(t.max_buddies, 0) AS MaxBuddies,
        COALESCE(abt.accepted_persons, 0) AS AcceptedPersons,
        GREATEST(COALESCE(t.max_buddies, 0) - COALESCE(abt.accepted_persons, 0), 0) AS RemainingCapacity
    FROM trip_destination td
    JOIN trip t ON t.trip_id = td.trip_id
    JOIN destination d ON d.destination_id = td.destination_id
    LEFT JOIN (
        SELECT td2.trip_destination_id, SUM(b.person_count) AS accepted_persons
        FROM buddy b
        JOIN trip_destination td2 ON td2.trip_destination_id = b.trip_destination_id
        WHERE b.request_status = 'accepted'
        GROUP BY td2.trip_destination_id
    ) AS abt
        ON abt.trip_destination_id = td.trip_destination_id
    WHERE
        (
            (in_req_start IS NULL AND in_req_end IS NULL)
            OR (in_req_start IS NOT NULL AND in_req_end IS NOT NULL
                AND td.start_date >= in_req_start
                AND td.end_date <= in_req_end)
            OR (in_req_start IS NOT NULL AND in_req_end IS NULL
                AND td.start_date >= in_req_start)
            OR (in_req_start IS NOT NULL AND in_req_end IS NOT NULL
                AND td.end_date <= in_req_end)
        )
        AND (in_name IS NULL OR in_name = '' OR d.name = in_name)
        AND (in_country IS NULL OR in_country = '' OR d.country = in_country)
        AND (in_state IS NULL OR in_state = '' OR d.state = in_state)
        AND ((COALESCE(t.max_buddies, 0) - COALESCE(abt.accepted_persons, 0)) >= v_party_size)
        AND (
            in_q IS NULL OR in_q = ''
            OR td.description LIKE CONCAT('%', in_q, '%')
            OR d.name LIKE CONCAT('%', in_q, '%')
        )
    ORDER BY td.start_date, d.name
    LIMIT 50;
END $$
DELIMITER ;

# 2. User should be able to see the trips they are connected to (both as owner and as buddy) – also archived ones
DROP PROCEDURE IF EXISTS get_user_trips;
DELIMITER $$
CREATE PROCEDURE get_user_trips (
    IN in_user_id INT
)
BEGIN
    -- Trips where user is owner
    SELECT t.trip_id AS TripId,
           td.trip_destination_id AS TripDestinationId,
           d.name AS DestinationName,
           t.description AS TripDescription,
           td.start_date AS StartDate,
           td.end_date AS EndDate,
           td.is_archived AS IsArchived,
           'owner' AS Role
    FROM trip t
    JOIN trip_destination td ON td.trip_id = t.trip_id
    JOIN destination d ON d.destination_id = td.destination_id
    WHERE get_owner_id_from_trip(t.trip_id) = in_user_id

    UNION

    -- Trips where user is buddy
    SELECT t.trip_id AS TripId,
           td.trip_destination_id  AS TripDestinationId,
           d.name AS DestinationName,
           t.description AS TripDescription,
           td.start_date AS StartDate,
           td.end_date AS EndDate,
           td.is_archived AS IsArchived,
           'buddy' AS Role
    FROM trip t
    JOIN trip_destination td ON td.trip_id = t.trip_id
    JOIN destination d ON d.destination_id = td.destination_id
    JOIN buddy b ON b.trip_destination_id = td.trip_destination_id
    WHERE get_user_id_from_buddy(b.buddy_id) = in_user_id
    AND b.is_active = TRUE

    ORDER BY StartDate;
END $$
DELIMITER ;

DROP PROCEDURE IF EXISTS get_buddy_trips;
DELIMITER $$
CREATE PROCEDURE get_buddy_trips (
    IN in_user_id INT
)
BEGIN
    SELECT t.trip_id AS TripId,
           td.trip_destination_id  AS TripDestinationId,
           d.name AS DestinationName,
           t.description AS TripDescription,
           td.start_date AS StartDate,
           td.end_date AS EndDate,
           td.is_archived AS IsArchived
    FROM trip t
    JOIN trip_destination td ON td.trip_id = t.trip_id
    JOIN destination d ON d.destination_id = td.destination_id
    JOIN buddy b ON b.trip_destination_id = td.trip_destination_id
    WHERE get_user_id_from_buddy(b.buddy_id) = in_user_id
    AND b.is_active = TRUE

    ORDER BY StartDate;
END $$
DELIMITER ;

# 3. User should see all conversations they are part of (also archived ones)
DROP PROCEDURE IF EXISTS get_user_conversations;
DELIMITER $$
CREATE PROCEDURE get_user_conversations(IN p_user_id INT)
BEGIN
    SELECT
        c.conversation_id AS ConversationId,
        c.trip_destination_id AS TripDestinationId,
        c.is_group AS IsGroup,
        c.created_at AS CreatedAt,
        c.is_archived AS IsArchived,

        -- Count all participants in this conversation
        (SELECT COUNT(*)
         FROM conversation_participant cp_all
         WHERE cp_all.conversation_id = c.conversation_id) AS ParticipantCount,

        m.content AS LastMessagePreview,
        m.sent_at AS LastMessageAt,

        CASE
            WHEN c.is_group = TRUE THEN CONCAT('Group for ', d.name)
            ELSE (
                SELECT u.name
                FROM conversation_participant cp
                JOIN user u ON u.user_id = cp.user_id
                WHERE cp.conversation_id = c.conversation_id
                  AND cp.user_id <> p_user_id
                LIMIT 1
            )
        END AS ConversationName
    FROM conversation c
    JOIN conversation_participant p
        ON c.conversation_id = p.conversation_id
    LEFT JOIN trip_destination td
        ON c.trip_destination_id = td.trip_destination_id
    INNER JOIN destination d
        ON td.destination_id = d.destination_id
    LEFT JOIN message m
        ON m.message_id = (
            SELECT m2.message_id
            FROM message m2
            WHERE m2.conversation_id = c.conversation_id
            ORDER BY m2.sent_at DESC
            LIMIT 1
        )
    WHERE p.user_id = p_user_id;
END $$
DELIMITER ;

# 4. User should be able to see who are part of the conversation etc. (conversation info)
DROP PROCEDURE IF EXISTS get_conversation_info;
DELIMITER $$
CREATE PROCEDURE get_conversation_info(IN p_conversation_id INT)
BEGIN
    IF p_conversation_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'conversation_id cannot be NULL';
    END IF;

    SELECT
        c.conversation_id AS ConversationId,
        c.trip_destination_id AS TripDestinationId,
        JSON_OBJECT(
            'destination', d.name,
            'date', td.start_date
        ) AS TripDestination,
        COUNT(DISTINCT u.user_id) AS ParticipantCount,
        JSON_ARRAYAGG(
            JSON_OBJECT(
                'user_id', u.user_id,
                'name', u.name
            )
        ) AS Participants
    FROM conversation c
    JOIN trip_destination td ON c.trip_destination_id = td.trip_destination_id
    JOIN destination d ON td.destination_id = d.destination_id
    JOIN conversation_participant p ON c.conversation_id = p.conversation_id
    JOIN user u ON p.user_id = u.user_id
    WHERE c.conversation_id = 9
    GROUP BY c.conversation_id, c.trip_destination_id;
END $$
DELIMITER ;

# 5. User should see all messages in each conversation even
# those before being added to the conversation.
DROP PROCEDURE IF EXISTS get_messages;
DELIMITER $$
CREATE PROCEDURE get_messages(IN p_conversation_id INT)
BEGIN
    IF p_conversation_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'conversation_id cannot be NULL';
    END IF;

    SELECT
      m.message_id AS MessageId,
      m.sender_id AS SenderId,
      u.name AS Name,
      m.sent_at AS SentAt,
      m.content AS Content
    FROM message m
    JOIN conversation c ON m.conversation_id = c.conversation_id
    JOIN user u ON m.sender_id = u.user_id
    WHERE c.conversation_id = p_conversation_id;
END $$
DELIMITER ;

# 6. User should be able to send a message in a conversation
# 7. When sending a message, also check if convo is archived and if yes, change it back to false
DROP PROCEDURE IF EXISTS insert_new_message;
DELIMITER $$
CREATE PROCEDURE insert_new_message(
    IN p_sender_id INT,
    IN p_conversation_id INT,
    IN p_content VARCHAR(2000)
)
BEGIN
    -- Validate that none of the inputs are NULL
    IF p_sender_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'sender_id cannot be NULL';
    END IF;

    IF p_conversation_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'conversation_id cannot be NULL';
    END IF;

    IF p_content IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'content cannot be NULL';
    END IF;

    INSERT INTO message (sender_id, content, conversation_id)
    VALUES (p_sender_id, p_content, p_conversation_id);

    -- After inserting the message
    UPDATE conversation
    SET is_archived = FALSE
    WHERE conversation_id = p_conversation_id
      AND is_archived = TRUE;
END $$
DELIMITER ;

# 8. Buddy can leave a trip destination
# 9. Owner can remove a buddy from a trip destination.
DROP PROCEDURE IF EXISTS remove_buddy_from_trip_destination;
DELIMITER $$
CREATE PROCEDURE remove_buddy_from_trip_destination(
    IN p_user_id INT,
    IN p_trip_destination_id INT,
    IN p_triggered_by INT,
    IN p_departure_reason VARCHAR(255)
)
BEGIN
    DECLARE v_buddy_id INT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    DECLARE EXIT HANDLER FOR NOT FOUND
    BEGIN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Buddy record not found for given user and trip destination';
    END;


    -- Validate inputs
    IF p_user_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'user_id cannot be NULL';
    END IF;

    IF p_trip_destination_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'trip_destination_id cannot be NULL';
    END IF;

    IF p_triggered_by IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'triggered_by cannot be NULL';
    END IF;

    -- Resolve buddy record
    SELECT buddy_id
    INTO v_buddy_id
    FROM buddy
    WHERE user_id = p_user_id
      AND trip_destination_id = p_trip_destination_id
    LIMIT 1;

    START TRANSACTION;

    -- Mark buddy inactive
    UPDATE buddy
    SET is_active = FALSE,
        departure_reason = p_departure_reason
    WHERE buddy_id = v_buddy_id;

    -- Cleanup conversation
    CALL remove_buddy_from_conversation(v_buddy_id, p_triggered_by);

    COMMIT;
    SELECT 1 AS Success;
END $$
DELIMITER ;

# 10. If a buddy is not active anymore (left or removed) they should be removed from group conversation
DROP PROCEDURE IF EXISTS remove_buddy_from_conversation;
DELIMITER $$
CREATE PROCEDURE remove_buddy_from_conversation(
    IN p_buddy_id INT,
    IN p_triggered_by INT -- the user who initiated the action
)
BEGIN
    DECLARE v_user_id INT;
    DECLARE v_conversation_id INT;

    IF p_buddy_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'buddy_id cannot be NULL';
    END IF;

    IF p_triggered_by IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'triggered_by cannot be NULL';
    END IF;

    SET v_conversation_id = get_group_conversation_for_buddy(p_buddy_id);
    -- If no conversation exists, skip deletion
    IF v_conversation_id IS NOT NULL THEN
        SET v_user_id = get_user_id_from_buddy(p_buddy_id);

        DELETE FROM conversation_participant
        WHERE user_id = v_user_id
        AND conversation_id = v_conversation_id;

        -- Audit log
        INSERT INTO conversation_audit (
            conversation_id,
            affected_user_id,
            action,
            triggered_by
        )
        VALUES (
            v_conversation_id,
            v_user_id,
            'user_removed',
            p_triggered_by
        );
    END IF;
END $$
DELIMITER ;

# 11. If someone is removed from group conversation, there should be sent a message in the group saying someone left
# 12. User should be able to create a new trip with trip destinations

# 13. Trip owners should be able to accept or reject buddy requests
DROP PROCEDURE IF EXISTS update_buddy_request;
DELIMITER $$
CREATE PROCEDURE update_buddy_request(
    IN p_buddy_id INT,
    IN p_new_status ENUM('accepted', 'rejected'),
    IN p_owner_id INT
)
BEGIN
    DECLARE v_trip_destination_id INT;
    DECLARE v_trip_owner_id INT;

    -- Validate inputs
    IF p_buddy_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'buddy_id cannot be NULL';
    END IF;

    IF p_new_status IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'new_status cannot be NULL';
    END IF;

    IF p_owner_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'owner_id cannot be NULL';
    END IF;

    -- Get trip destination for buddy
    SELECT trip_destination_id
    INTO v_trip_destination_id
    FROM buddy
    WHERE buddy_id = p_buddy_id
    LIMIT 1;

    IF v_trip_destination_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Buddy not found';
    END IF;

    -- Get actual owner of the trip
    SELECT t.owner_id
    INTO v_trip_owner_id
    FROM trip t
    JOIN trip_destination td ON td.trip_id = t.trip_id
    WHERE td.trip_destination_id = v_trip_destination_id
    LIMIT 1;

    IF v_trip_owner_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Trip owner not found';
    END IF;

    -- Check if it is the actual owner
    IF p_owner_id <> v_trip_owner_id THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Not authorized: only trip owner can update buddy request';
    END IF;

    -- Update buddy request status
    UPDATE buddy
    SET request_status = p_new_status
    WHERE buddy_id = p_buddy_id;

    -- Audit log
    INSERT INTO buddy_audit (buddy_id, action, reason, changed_by)
    VALUES (
        p_buddy_id,
        p_new_status,
        'Owner answered the request',
        p_owner_id
    );
END $$
DELIMITER ;

# 15. Owner should be able to create a group chat for a trip destination
DROP PROCEDURE IF EXISTS create_group_conversation_for_trip_destination;
DELIMITER $$
CREATE PROCEDURE create_group_conversation_for_trip_destination(
    IN p_trip_destination_id INT,
    IN p_owner_id INT
)
BEGIN
    DECLARE v_conversation_id INT;

    IF p_trip_destination_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'trip_destination_id cannot be NULL';
    END IF;

    SELECT conversation_id
    INTO v_conversation_id
    FROM conversation
    WHERE is_group = TRUE
    AND trip_destination_id = p_trip_destination_id
    LIMIT 1;

    -- Only if there is no conversation already
    IF v_conversation_id IS NULL THEN
        -- Step 1: Create the group conversation
        INSERT INTO conversation (trip_destination_id, is_group)
        VALUES (p_trip_destination_id, TRUE);

        -- Step 2: Get the new conversation ID
        SET v_conversation_id = LAST_INSERT_ID();

        -- Step 3: Add owner to the conversation
        INSERT INTO conversation_participant (conversation_id, user_id) VALUES (v_conversation_id, p_owner_id);

        -- Step 4: Add all buddies for this trip to the conversation
        INSERT INTO conversation_participant (conversation_id, user_id)
        SELECT v_conversation_id, user_id
        FROM buddy
        WHERE trip_destination_id = p_trip_destination_id
        AND request_status = 'accepted';

        -- Audit log
        INSERT INTO conversation_audit (
            conversation_id,
            affected_user_id,
            action,
            triggered_by
        )
        VALUES (
            v_conversation_id,
            NULL,
            'created',
            p_owner_id
        );
    END IF;
END $$
DELIMITER ;

# 16. User should be able to request to join a trip destination
DROP PROCEDURE IF EXISTS request_to_join_trip_destination;
DELIMITER $$
CREATE PROCEDURE request_to_join_trip_destination(
    IN p_user_id INT,
    IN p_trip_destination_id INT,
    IN p_person_count INT,
    IN p_note VARCHAR(255)
)
BEGIN
    DECLARE v_buddy_id INT;
    DECLARE v_remaining_capacity INT;
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
        BEGIN
            ROLLBACK;
            RESIGNAL;
        END;

    IF p_user_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'user_id cannot be NULL';
    END IF;
    IF p_trip_destination_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'trip_destination_id cannot be NULL';
    END IF;
    IF p_person_count IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'person_count cannot be NULL';
    END IF;
    IF p_person_count <= 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'person_count must be greater than zero';
    END IF;

    START TRANSACTION;

    SET v_remaining_capacity = get_trip_destination_remaining_capacity(p_trip_destination_id);

    IF v_remaining_capacity < p_person_count THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'there is not enough buddy capacity for the person_count';
    END IF;

    IF EXISTS (
        SELECT 1 FROM buddy
        WHERE user_id = p_user_id AND trip_destination_id = p_trip_destination_id
    ) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'User has already requested to join this trip destination';
    END IF;

    INSERT INTO buddy (user_id, trip_destination_id, person_count, note)
    VALUES (p_user_id, p_trip_destination_id, p_person_count, p_note);

    SET v_buddy_id = LAST_INSERT_ID();

    INSERT INTO buddy_audit (buddy_id, action, reason, changed_by)
    VALUES (v_buddy_id, 'requested', 'Buddy request added', p_user_id);

    COMMIT;
END $$
DELIMITER ;

# 17. When a buddy is accepted to join, they should be added to group chat if it exists
DROP PROCEDURE IF EXISTS add_buddy_to_conversation;
DELIMITER $$
CREATE PROCEDURE add_buddy_to_conversation(
    IN p_buddy_id INT
)
BEGIN
    DECLARE v_user_id INT; # v added for local variables
    DECLARE v_conversation_id INT;

    IF p_buddy_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'buddy_id cannot be NULL';
    END IF;

    SET v_conversation_id = get_group_conversation_for_buddy(p_buddy_id);
    -- If group conversation exists
    IF v_conversation_id IS NOT NULL THEN
        SET v_user_id = get_user_id_from_buddy(p_buddy_id);

        IF NOT EXISTS (
            SELECT 1 FROM conversation_participant # SELECT 1 = Check if any row exists
            WHERE conversation_id = v_conversation_id
            AND user_id = v_user_id
        ) THEN
            INSERT INTO conversation_participant (conversation_id, user_id)
            VALUES (v_conversation_id, v_user_id);

            -- Audit log
            INSERT INTO conversation_audit (
                conversation_id,
                affected_user_id,
                action,
                triggered_by
            )
            VALUES (
                v_conversation_id,
                v_user_id,
                'user_added',
                NULL
            );
        END IF;
    END IF;
END $$
DELIMITER ;

# Transaction for Accept + add to group chat
DROP PROCEDURE IF EXISTS update_buddy_request_tx;
DELIMITER $$
CREATE PROCEDURE update_buddy_request_tx (
    IN p_buddy_id INT,
    IN p_owner_id INT,
    IN p_new_status ENUM('accepted', 'rejected')
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
        BEGIN
            ROLLBACK;
            RESIGNAL;
        END;
    START TRANSACTION;
    CALL update_buddy_request(p_buddy_id, p_new_status, p_owner_id);
    CALL add_buddy_to_conversation(p_buddy_id);
    COMMIT;
END $$
DELIMITER ;


# 18. User should be able to make a new convo with a user (optional if it is connected to a trip)
DROP PROCEDURE IF EXISTS insert_new_private_conversation;
DELIMITER $$
CREATE PROCEDURE insert_new_private_conversation(
    IN p_trip_destination_id INT,
    IN p_owner_id INT,
    IN p_user_id INT
)
BEGIN
    DECLARE v_conversation_id INT;

    IF p_trip_destination_id IS NULL OR p_owner_id IS NULL OR p_user_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'All input parameters must be non-NULL';
    END IF;

    IF p_owner_id = p_user_id THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'User cannot create conversation with themselves';
    END IF;

    -- 1. Create conversation
    INSERT INTO conversation (trip_destination_id, is_group)
    VALUES (p_trip_destination_id, FALSE);

    SET v_conversation_id = LAST_INSERT_ID();

    -- Audit log
    INSERT INTO conversation_audit (
        conversation_id,
        affected_user_id,
        action,
        triggered_by
    )
    VALUES (
        v_conversation_id,
        NULL,
        'created',
        p_owner_id
    );

    -- 2. Add participants
    INSERT INTO conversation_participant (conversation_id, user_id) VALUES
    (v_conversation_id, p_owner_id),
    (v_conversation_id, p_user_id);

    -- Audit log
    INSERT INTO conversation_audit (
        conversation_id,
        affected_user_id,
        action,
        triggered_by
    ) VALUES
    (
        v_conversation_id,
        p_owner_id,
        'user_added',
        p_owner_id
    ),
    (
        v_conversation_id,
        p_user_id,
        'user_added',
        p_owner_id
    );
END $$
DELIMITER ;

# 19. Every month, trip_destination and trip should be archived (is_archived = true)
# if trip_destination end_date has passed
DROP EVENT IF EXISTS monthly_archive_old_trip_destinations;
DELIMITER $$
CREATE EVENT monthly_archive_old_trip_destinations
ON SCHEDULE EVERY 1 MONTH
STARTS '2025-09-30 00:00:00'
ON COMPLETION PRESERVE -- keeps the event definition even after it runs (especially useful if you ever switch to a one-time event)
DO BEGIN
    UPDATE trip_destination
    SET is_archived = true
    WHERE end_date < NOW();
END $$

DROP TRIGGER IF EXISTS audit_archiving_of_trip_destination;
DELIMITER $$
CREATE TRIGGER audit_archiving_of_trip_destination
AFTER UPDATE ON trip_destination
FOR EACH ROW
BEGIN
    IF OLD.is_archived = false AND NEW.is_archived = true THEN
        INSERT INTO system_event_log (event_type, affected_id, details)
        VALUES (
            'ARCHIVE',
            NEW.trip_destination_id,
            'Archiving old trip destination' );
    END IF;
END $$
DELIMITER ;

DROP EVENT IF EXISTS monthly_archive_old_trips;
DELIMITER $$
CREATE EVENT monthly_archive_old_trips
ON SCHEDULE EVERY 1 MONTH
STARTS '2025-09-30 00:00:00'
ON COMPLETION PRESERVE
DO BEGIN
    UPDATE trip
    SET is_archived = true
    WHERE end_date < NOW();
END $$
DELIMITER ;

DROP TRIGGER IF EXISTS audit_archiving_of_trip;
DELIMITER $$
CREATE TRIGGER audit_archiving_of_trip
AFTER UPDATE ON trip
FOR EACH ROW
BEGIN
    IF OLD.is_archived = false AND NEW.is_archived = true THEN
        INSERT INTO system_event_log (event_type, affected_id, details)
        VALUES (
            'ARCHIVE',
            NEW.trip_id,
            'Archiving old trip' );
    END IF;
END $$
DELIMITER ;

# 20. Every two weeks, check if a conversation has not been active for at least a week
# and make it archived
DROP EVENT IF EXISTS archive_due_to_inactivity;
DELIMITER $$
CREATE EVENT archive_due_to_inactivity
ON SCHEDULE EVERY 2 WEEK
STARTS '2025-09-30 00:00:00'
ON COMPLETION PRESERVE
DO BEGIN
    UPDATE conversation
    SET is_archived = TRUE
    WHERE conversation_id IN (
        SELECT m.conversation_id
        FROM message m
        GROUP BY m.conversation_id
        HAVING MAX(m.sent_at) <= NOW() - INTERVAL 1 WEEK
    );
END $$
DELIMITER ;

DROP TRIGGER IF EXISTS audit_archiving_of_conversation;
DELIMITER $$
CREATE TRIGGER audit_archiving_of_conversation
AFTER UPDATE ON conversation
FOR EACH ROW
BEGIN
    IF OLD.is_archived = false AND NEW.is_archived = true THEN
        INSERT INTO system_event_log (event_type, affected_id, details)
        VALUES (
            'ARCHIVE',
            NEW.conversation_id,
            'Archiving inactive conversation' );
    END IF;

    IF OLD.is_archived = true AND NEW.is_archived = false THEN
        INSERT INTO system_event_log (event_type, affected_id, details)
        VALUES (
            'UNARCHIVE',
            NEW.conversation_id,
            'Unarchiving previously inactive conversation' );
    END IF;
END $$
DELIMITER ;

# 21. Trip owners should be able to see all pending buddy requests
DROP PROCEDURE IF EXISTS get_pending_buddy_requests;
DELIMITER $$
CREATE PROCEDURE get_pending_buddy_requests(IN p_user_id INT)
BEGIN
    IF p_user_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'user_id cannot be NULL';
    END IF;

    SELECT
      td.trip_destination_id AS TripDestinationId,
      d.name AS DestinationName,
      td.start_date AS DestinationStartDate,
      td.end_date AS DestinationEndDate,
      t.trip_id AS TripId,
      b.buddy_id AS BuddyId,
      u.user_id AS RequesterUserId,
      u.name AS RequesterName,
      b.note AS BuddyNote,
      b.person_count AS PersonCount
    FROM trip t
    JOIN trip_destination td ON t.trip_id = td.trip_id
    JOIN destination d ON td.destination_id = d.destination_id
    JOIN buddy b ON td.trip_destination_id = b.trip_destination_id
    JOIN user u ON b.user_id = u.user_id
    WHERE b.request_status = 'pending'
      AND t.owner_id = p_user_id
    ORDER BY td.start_date DESC ; # Example user 4
END $$
DELIMITER ;

# 22. See all users (admin). No need to see e.g. password_hash though.
CREATE or REPLACE VIEW all_users AS
SELECT user_id, name, email, birthdate, is_deleted
FROM user;

# 27. User should be able to delete their own account + admin can delete all
DROP PROCEDURE IF EXISTS delete_user;
DELIMITER $$
CREATE PROCEDURE delete_user(
    IN p_user_id INT,
    IN p_password_hash VARCHAR(255)
)
BEGIN
    IF p_user_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'user_id cannot be NULL';
    END IF;

    UPDATE user
    SET
        is_deleted = TRUE,
        name = 'Deleted User',
        email = CONCAT('deleted_', user_id, '@example.com'),
        password_hash = p_password_hash,
        birthdate = NOW()
    WHERE user_id = p_user_id;
END $$
DELIMITER ;

# See trip destination info
CREATE OR REPLACE VIEW V_TripDestinationInfo AS
SELECT
    td.trip_destination_id AS TripDestinationId,
    td.start_date AS DestinationStartDate,
    td.end_date AS DestinationEndDate,
    td.description AS DestinationDescription,
    td.is_archived AS DestinationIsArchived,
    td.trip_id AS TripId,

    t.max_buddies AS MaxBuddies,

    d.destination_id AS DestinationId,
    d.name AS DestinationName,
    d.state AS DestinationState,
    d.country AS DestinationCountry,
    d.longitude AS Longitude,
    d.latitude AS Latitude,

    u_owner.user_id AS OwnerUserId,
    u_owner.name AS OwnerName,

    (
        SELECT c.conversation_id
        FROM conversation c
        WHERE c.trip_destination_id = td.trip_destination_id
          AND c.is_group = TRUE
    ) AS GroupConversationId

FROM
    trip_destination td
JOIN
    trip t ON td.trip_id = t.trip_id
JOIN
    destination d ON td.destination_id = d.destination_id
JOIN
    user u_owner ON t.owner_id = u_owner.user_id;

# See accepted buddies
DROP VIEW IF EXISTS V_AcceptedBuddies;
CREATE VIEW V_AcceptedBuddies AS
SELECT
    b.buddy_id AS BuddyId,
    b.trip_destination_id AS TripDestinationId,
    b.person_count AS PersonCount,
    b.note AS BuddyNote,
    u_buddy.user_id AS BuddyUserId,
    u_buddy.name AS BuddyName
FROM
    buddy b
JOIN
    user u_buddy ON b.user_id = u_buddy.user_id
WHERE
    b.is_active = TRUE
    AND b.request_status = 'accepted';

# See pending requests
DROP VIEW IF EXISTS V_PendingRequests;
CREATE VIEW V_PendingRequests AS
SELECT
    b.buddy_id AS BuddyId,
    b.trip_destination_id AS TripDestinationId,
    b.person_count AS PersonCount,
    b.note AS BuddyNote,
    u_requester.user_id AS RequesterUserId,
    u_requester.name AS RequesterName
FROM
    buddy b
JOIN
    user u_requester ON b.user_id = u_requester.user_id
WHERE
    b.request_status = 'pending';

# --------------- SEE TRIP INFO
CREATE OR REPLACE VIEW V_SimplifiedTripDest AS
SELECT
    td.trip_destination_id AS TripDestinationId,
    td.trip_id AS TripId,
    td.start_date AS DestinationStartDate,
    td.end_date AS DestinationEndDate,
    d.name AS DestinationName,
    d.state AS DestinationState,
    d.country AS DestinationCountry,
    t.max_buddies AS MaxBuddies,
    0 AS AcceptedBuddiesCount
FROM
    trip_destination td
JOIN
    destination d ON td.destination_id = d.destination_id
JOIN
    trip t ON td.trip_id = t.trip_id;

CREATE OR REPLACE VIEW V_TripHeaderInfo AS
SELECT
    t.trip_id AS TripId,
    t.trip_name AS TripName,
    t.max_buddies AS MaxBuddies,
    t.start_date AS TripStartDate,
    t.end_date AS TripEndDate,
    t.description AS TripDescription,
    t.is_archived AS TripIsArchived,

    u_owner.user_id AS OwnerUserId,
    u_owner.name AS OwnerName
FROM
    trip t
JOIN
    user u_owner ON t.owner_id = u_owner.user_id;

# --------------- CREATING A TRIP
