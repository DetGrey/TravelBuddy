USE travel_buddy;
-- ------------------------------------------------------------------ INDEXES
-- Date + FK'er
CREATE INDEX idx_td_dates on trip_destinations(start_date, end_date);
CREATE INDEX idx_td_trip on trip_destinations(trip_id);
CREATE Index idx_td_destination on trip_destinations(destination_id);
-- Location
CREATE INDEX idx_dest_contry_state ON destinations(country,state);
-- Buddies (count)
CREATE INDEX idx_buddy_seg_status on buddies(trip_destination_id, request_status);
-- ------------------------------------------------------------------ FUNCTIONS
DROP FUNCTION IF EXISTS get_user_id_from_buddy;
DELIMITER $$
CREATE FUNCTION get_user_id_from_buddy(f_buddy_id INT)
RETURNS INT
DETERMINISTIC
BEGIN
    RETURN (
        SELECT user_id
        FROM buddies
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
    FROM buddies
    WHERE buddy_id = f_buddy_id;

    -- Get group conversation for that trip destination
    SELECT conversation_id
    INTO v_conversation_id
    FROM conversations
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
    FROM trips t
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
    FROM trip_destinations
    WHERE trip_destination_id = f_trip_destination_id
    LIMIT 1;

    IF v_trip_id IS NULL THEN
        RETURN NULL;
    END IF;

    SELECT max_buddies INTO v_max_buddies
    FROM trips
    WHERE trip_id = v_trip_id
    LIMIT 1;

    SELECT SUM(b.person_count) INTO v_accepted_buddies
    FROM buddies b
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
-- ------------------------------------------------------------------ VIEWS
-- =====================================
-- 22. See all users (admin). No need to see e.g. password_hash though.
-- =====================================
CREATE or REPLACE VIEW all_users AS
SELECT user_id, name, email, birthdate, is_deleted
FROM users;
-- =====================================
-- See trip destination info
-- =====================================
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
        FROM conversations c
        WHERE c.trip_destination_id = td.trip_destination_id
          AND c.is_group = TRUE
    ) AS GroupConversationId

FROM
    trip_destinations td
JOIN
    trips t ON td.trip_id = t.trip_id
JOIN
    destinations d ON td.destination_id = d.destination_id
JOIN
    users u_owner ON t.owner_id = u_owner.user_id;
-- =====================================
-- See accepted buddies
-- =====================================
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
    buddies b
JOIN
    users u_buddy ON b.user_id = u_buddy.user_id
WHERE
    b.is_active = TRUE
    AND b.request_status = 'accepted';
-- =====================================
-- See pending requests
-- =====================================
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
    buddies b
JOIN
    users u_requester ON b.user_id = u_requester.user_id
WHERE
    b.request_status = 'pending';
-- =====================================
-- See trip info
-- =====================================
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
    trip_destinations td
JOIN
    destinations d ON td.destination_id = d.destination_id
JOIN
    trips t ON td.trip_id = t.trip_id;
-- =====================================
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
    trips t
JOIN
    users u_owner ON t.owner_id = u_owner.user_id;
-- ------------------------------------------------------------------ USERS
-- =====================================
-- 27. User should be able to delete their own account + admin can delete all
-- =====================================
DROP PROCEDURE IF EXISTS delete_user;
DELIMITER $$
CREATE PROCEDURE delete_user(
    IN p_user_id INT,
    IN p_password_hash VARCHAR(255)
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    IF p_user_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'user_id cannot be NULL';
    END IF;

    START TRANSACTION;

    UPDATE users
    SET
        is_deleted = TRUE,
        name = 'Deleted User',
        email = CONCAT('deleted_', user_id, '@example.com'),
        password_hash = p_password_hash,
        birthdate = NOW()
    WHERE user_id = p_user_id;

    IF ROW_COUNT() = 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'No user found with the given user_id';
    END IF;

    INSERT INTO user_audits (user_id, action, field_changed, old_value, new_value, changed_by)
    VALUES (p_user_id, 'deleted', 'user', NULL, NULL, NULL);

    COMMIT;
END $$
DELIMITER ;
-- =====================================
-- Update email
-- =====================================
DROP PROCEDURE IF EXISTS update_user_role;
DELIMITER $$
DELIMITER $$
CREATE PROCEDURE update_user_role (
    IN target_user_id INT,
    IN new_role ENUM('user', 'admin'),
    IN changed_by_user_id INT
)
BEGIN
    DECLARE v_changer_role ENUM('user', 'admin');
    DECLARE v_old_role ENUM('user', 'admin');

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;
    
    START TRANSACTION;

    SELECT role INTO v_changer_role 
    FROM users
    WHERE user_id = changed_by_user_id;

    IF v_changer_role IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Changer user ID not found.';
    END IF;

    IF v_changer_role <> 'admin' THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Permission denied: Only admin users can update user roles.';
    END IF;

    SET @SESSION_TRIGGER_AUDIT_SKIP = TRUE;

    UPDATE users
    SET role = new_role
    WHERE user_id = target_user_id;

    SET @SESSION_TRIGGER_AUDIT_SKIP = NULL;

    INSERT INTO user_audits (user_id, action, field_changed, old_value, new_value, changed_by)
    VALUES (target_user_id, 'updated', 'role', v_old_role, new_role, changed_by_user_id);

    COMMIT;
END $$
DELIMITER ;
-- ------------------------------------------------------------------ TRIPS
-- =====================================
-- 1. User can search for a trip destination using start/end dates, buddy count and location.
-- Maybe also description – for specific activities.
-- =====================================
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
    FROM trip_destinations td
    JOIN trips t ON t.trip_id = td.trip_id
    JOIN destinations d ON d.destination_id = td.destination_id
    LEFT JOIN (
        SELECT td2.trip_destination_id, SUM(b.person_count) AS accepted_persons
        FROM buddies b
        JOIN trip_destinations td2 ON td2.trip_destination_id = b.trip_destination_id
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
-- =====================================
-- 2. User should be able to see the trips they are connected to 
-- (both as owner and as buddy) – also archived ones
-- =====================================
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
    FROM trips t
    JOIN trip_destinations td ON td.trip_id = t.trip_id
    JOIN destinations d ON d.destination_id = td.destination_id
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
    FROM trips t
    JOIN trip_destinations td ON td.trip_id = t.trip_id
    JOIN destinations d ON d.destination_id = td.destination_id
    JOIN buddies b ON b.trip_destination_id = td.trip_destination_id
    WHERE get_user_id_from_buddy(b.buddy_id) = in_user_id
    AND b.is_active = TRUE

    ORDER BY StartDate;
END $$
DELIMITER ;
-- ===================================== get_buddy_trips
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
    FROM trips t
    JOIN trip_destinations td ON td.trip_id = t.trip_id
    JOIN destinations d ON d.destination_id = td.destination_id
    JOIN buddies b ON b.trip_destination_id = td.trip_destination_id
    WHERE get_user_id_from_buddy(b.buddy_id) = in_user_id
    AND b.is_active = TRUE

    ORDER BY StartDate;
END $$
DELIMITER ;
-- =====================================
-- 12. User should be able to create a new trip with trip destinations
-- =====================================
DROP PROCEDURE IF EXISTS create_trip;
DELIMITER $$
CREATE PROCEDURE create_trip (
    IN p_owner_id INT,
    IN p_trip_name VARCHAR(100),
    IN p_max_buddies INT,
    IN p_start_date DATE,
    IN p_end_date DATE,
    IN p_description VARCHAR(255),
    IN p_changed_by INT
)
BEGIN
    DECLARE new_trip_id INT;

    -- Validate inputs
    IF p_owner_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Owner ID cannot be NULL';
    END IF;

    IF p_trip_name IS NULL OR p_trip_name = '' THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Trip name is required';
    END IF;

    IF p_max_buddies IS NULL OR p_max_buddies < 1 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Max buddies must be >= 1';
    END IF;

    IF p_start_date IS NULL OR p_end_date IS NULL THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Start and end dates are required';
    END IF;

    IF p_end_date < p_start_date THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'End date must be after start date';
    END IF;
    
    SET @SESSION_TRIGGER_AUDIT_SKIP = TRUE;

    -- Insert trip
    INSERT INTO trips (owner_id, trip_name, max_buddies, start_date, end_date, description)
    VALUES (p_owner_id, p_trip_name, p_max_buddies, p_start_date, p_end_date, p_description);
    
    SET @SESSION_TRIGGER_AUDIT_SKIP = NULL;
    
    SET new_trip_id = LAST_INSERT_ID();

    -- Audit log
    INSERT INTO trip_audits (trip_id, action, field_changed, old_value, new_value, changed_by)
    VALUES (new_trip_id, 'created', NULL, NULL, CONCAT('Trip created: ', p_trip_name), p_changed_by);

    -- Return the new tripId
    SELECT new_trip_id AS TripId;
END $$
DELIMITER ;
-- =====================================
-- Add trip destination to trip
-- =====================================
DROP PROCEDURE IF EXISTS create_trip_destination;
DELIMITER $$
CREATE PROCEDURE create_trip_destination (
    IN p_trip_id INT,
    IN p_destination_id INT,
    IN p_name VARCHAR(255),
    IN p_state VARCHAR(100),
    IN p_country VARCHAR(100),
    IN p_longitude DECIMAL(10,7),
    IN p_latitude DECIMAL(10,7),
    IN p_start_date DATE,
    IN p_end_date DATE,
    IN p_sequence_number INT,
    IN p_description VARCHAR(255)
)
BEGIN
    DECLARE new_destination_id INT;

    -- Validate trip
    IF p_trip_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Trip ID cannot be NULL';
    END IF;

    IF p_start_date IS NULL OR p_end_date IS NULL THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Start and end dates are required';
    END IF;

    IF p_end_date < p_start_date THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'End date must be after start date';
    END IF;

    IF p_sequence_number IS NULL OR p_sequence_number < 1 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Sequence number must be >= 1';
    END IF;

    -- If no destination_id, create one
    IF p_destination_id IS NULL THEN
        IF p_name IS NULL OR p_name = '' THEN
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'Destination name required when creating new destination';
        END IF;

        IF p_country IS NULL OR p_country = '' THEN
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'Country required when creating new destination';
        END IF;

        -- Validate coordinates
        IF p_longitude IS NOT NULL AND (p_longitude < -180 OR p_longitude > 180) THEN
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'Longitude must be between -180 and 180';
        END IF;

        IF p_latitude IS NOT NULL AND (p_latitude < -90 OR p_latitude > 90) THEN
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'Latitude must be between -90 and 90';
        END IF;

        INSERT INTO destinations (name, state, country, longitude, latitude)
        VALUES (p_name, p_state, p_country, p_longitude, p_latitude);

        SET new_destination_id = LAST_INSERT_ID();
    ELSE
        SET new_destination_id = p_destination_id;
    END IF;

    -- Insert trip destination
    INSERT INTO trip_destinations (trip_id, destination_id, start_date, end_date, sequence_number, description)
    VALUES (p_trip_id, new_destination_id, p_start_date, p_end_date, p_sequence_number, p_description);
END $$
DELIMITER ;
-- ------------------------------------------------------------------ MESSAGING
-- =====================================
-- 3. User should see all conversations they are part of (also archived ones)
-- =====================================
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
         FROM conversation_participants cp_all
         WHERE cp_all.conversation_id = c.conversation_id) AS ParticipantCount,

        m.content AS LastMessagePreview,
        m.sent_at AS LastMessageAt,

        CASE
            WHEN c.is_group = TRUE THEN CONCAT('Group for ', d.name)
            ELSE (
                SELECT IFNULL(u.name, 'Unknown User')
                FROM conversation_participants cp
                LEFT JOIN users u ON u.user_id = cp.user_id
                WHERE cp.conversation_id = c.conversation_id
                  AND cp.user_id <> p_user_id
                LIMIT 1
            )
        END AS ConversationName
    FROM conversations c
    JOIN conversation_participants p
        ON c.conversation_id = p.conversation_id
    LEFT JOIN trip_destinations td
        ON c.trip_destination_id = td.trip_destination_id
    LEFT JOIN destinations d
        ON td.destination_id = d.destination_id
    LEFT JOIN messages m
        ON m.message_id = (
            SELECT m2.message_id
            FROM messages m2
            WHERE m2.conversation_id = c.conversation_id
            ORDER BY m2.sent_at DESC
            LIMIT 1
        )
    WHERE p.user_id = p_user_id;
END $$
DELIMITER ;
-- =====================================
-- 4. User should be able to see who are part of the conversation etc. (conversation info)
-- =====================================
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
    FROM conversations c
    JOIN trip_destinations td ON c.trip_destination_id = td.trip_destination_id
    JOIN destinations d ON td.destination_id = d.destination_id
    JOIN conversation_participants p ON c.conversation_id = p.conversation_id
    JOIN users u ON p.user_id = u.user_id
    WHERE c.conversation_id = 9
    GROUP BY c.conversation_id, c.trip_destination_id;
END $$
DELIMITER ;
-- =====================================
-- 5. User should see all messages in each conversation even
-- those before being added to the conversation.
-- =====================================
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
    FROM messages m
    JOIN conversations c ON m.conversation_id = c.conversation_id
    JOIN users u ON m.sender_id = u.user_id
    WHERE c.conversation_id = p_conversation_id;
END $$
DELIMITER ;
-- =====================================
-- 6. User should be able to send a message in a conversation
-- 7. When sending a message, also check if convo is archived and if yes, change it back to false
-- =====================================
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
    
    SET @SESSION_TRIGGER_AUDIT_SKIP = TRUE;
    
    INSERT INTO messages (sender_id, content, conversation_id)
    VALUES (p_sender_id, p_content, p_conversation_id);

    SET @SESSION_TRIGGER_AUDIT_SKIP = NULL;
    
    -- After inserting the message
    UPDATE conversations
    SET is_archived = FALSE
    WHERE conversation_id = p_conversation_id
      AND is_archived = TRUE;
END $$
DELIMITER ;
-- =====================================
-- 10. If a buddy is not active anymore (left or removed) they should be removed from group conversation
-- =====================================
DROP PROCEDURE IF EXISTS remove_buddy_from_conversation;
DELIMITER $$
CREATE PROCEDURE remove_buddy_from_conversation(
    IN p_buddy_id INT,
    IN p_changed_by INT -- the user who initiated the action
)
BEGIN
    DECLARE v_user_id INT;
    DECLARE v_conversation_id INT;

    IF p_buddy_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'buddy_id cannot be NULL';
    END IF;

    IF p_changed_by IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'changed_by cannot be NULL';
    END IF;

    SET v_conversation_id = get_group_conversation_for_buddy(p_buddy_id);
    -- If no conversation exists, skip deletion
    IF v_conversation_id IS NOT NULL THEN
        SET v_user_id = get_user_id_from_buddy(p_buddy_id);

        DELETE FROM conversation_participants
        WHERE user_id = v_user_id
        AND conversation_id = v_conversation_id;

        -- Audit log
        INSERT INTO conversation_audits (
            conversation_id,
            affected_user_id,
            action,
            changed_by
        )
        VALUES (
            v_conversation_id,
            v_user_id,
            'user_removed',
            p_changed_by
        );
    END IF;
END $$
DELIMITER ;
-- =====================================
-- 18. User should be able to make a new convo with a user (optional if it is connected to a trip)
-- =====================================
DROP PROCEDURE IF EXISTS insert_new_private_conversation;
DELIMITER $$
CREATE PROCEDURE insert_new_private_conversation(
    IN p_trip_destination_id INT,
    IN p_owner_id INT,
    IN p_user_id INT
)
BEGIN
    DECLARE v_conversation_id INT;
    DECLARE v_existing_conversation_id INT DEFAULT NULL;

    IF p_owner_id IS NULL OR p_user_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'owner_id and user_id must be non-NULL';
    END IF;

    IF p_owner_id = p_user_id THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'User cannot create conversation with themselves';
    END IF;

    START TRANSACTION;
  
    SELECT c.conversation_id INTO v_existing_conversation_id
    FROM conversations c
    INNER JOIN conversation_participants cp1 ON c.conversation_id = cp1.conversation_id
    INNER JOIN conversation_participants cp2 ON c.conversation_id = cp2.conversation_id
    WHERE 
        c.trip_destination_id = p_trip_destination_id 
        AND c.is_group = FALSE
        AND cp1.user_id = p_owner_id
        AND cp2.user_id = p_user_id
        AND cp1.user_id != cp2.user_id 
        AND (SELECT COUNT(*) FROM conversation_participants cp_count WHERE cp_count.conversation_id = c.conversation_id) = 2
    LIMIT 1;

    IF v_existing_conversation_id IS NOT NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Private conversation already exists';
    END IF;
    
    SET @SESSION_TRIGGER_AUDIT_SKIP = TRUE;
    
    INSERT INTO conversations (trip_destination_id, is_group)
    VALUES (p_trip_destination_id, FALSE);

    SET v_conversation_id = LAST_INSERT_ID();

    INSERT INTO conversation_audits (
        conversation_id,
        affected_user_id,
        action,
        changed_by
    )
    VALUES (
        v_conversation_id,
        NULL,
        'created',
        p_owner_id
    );

    INSERT INTO conversation_participants (conversation_id, user_id) VALUES
    (v_conversation_id, p_owner_id),
    (v_conversation_id, p_user_id);
    
    SET @SESSION_TRIGGER_AUDIT_SKIP = NULL;

    INSERT INTO conversation_audits (
        conversation_id,
        affected_user_id,
        action,
        changed_by
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

    COMMIT;
    
    SELECT v_conversation_id AS conversation_id;
END $$
DELIMITER ;
-- =====================================
-- 15. Owner should be able to create a group chat for a trip destination
-- =====================================
DROP PROCEDURE IF EXISTS create_group_conversation_for_trip_destination;
DELIMITER $$
CREATE PROCEDURE create_group_conversation_for_trip_destination(
    IN p_trip_destination_id INT,
    IN p_owner_id INT
)
BEGIN
    DECLARE v_conversation_id INT;
    DECLARE v_trip_id INT;
    DECLARE v_actual_owner_id INT;

    IF p_trip_destination_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'trip_destination_id cannot be NULL';
    END IF;

    SELECT conversation_id
    INTO v_conversation_id
    FROM conversations
    WHERE is_group = TRUE
    AND trip_destination_id = p_trip_destination_id
    LIMIT 1;

    -- 2. Retrieve the parent trip_id from trip_destination
    SELECT trip_id
    INTO v_trip_id
    FROM trip_destinations
    WHERE trip_destination_id = p_trip_destination_id;
    
    -- Check if the trip destination exists
    IF v_trip_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Trip destination ID not found.';
    END IF;

    -- Get the trip owner using the provided function
    SET v_actual_owner_id = get_owner_id_from_trip(v_trip_id);
    
    -- Validate p_owner_id against the actual trip owner
    IF v_actual_owner_id <> p_owner_id THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Permission denied: Only the main trip owner can create the group conversation.';
    END IF;

    -- Only if there is no conversation already
    IF v_conversation_id IS NULL THEN
        SET @SESSION_TRIGGER_AUDIT_SKIP = TRUE;
        
        -- Step 1: Create the group conversation
        INSERT INTO conversations (trip_destination_id, is_group)
        VALUES (p_trip_destination_id, TRUE);

        -- Step 2: Get the new conversation ID
        SET v_conversation_id = LAST_INSERT_ID();

        -- Step 3: Add owner to the conversation
        INSERT INTO conversation_participants (conversation_id, user_id) VALUES (v_conversation_id, p_owner_id);

        -- Step 4: Add all buddies for this trip to the conversation
        INSERT INTO conversation_participants (conversation_id, user_id)
        SELECT v_conversation_id, user_id
        FROM buddies
        WHERE trip_destination_id = p_trip_destination_id
        AND request_status = 'accepted'
        AND user_id <> p_owner_id;
        
        SET @SESSION_TRIGGER_AUDIT_SKIP = NULL;

        -- Audit log
        INSERT INTO conversation_audits (
            conversation_id,
            affected_user_id,
            action,
            changed_by
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
-- =====================================
-- 17. When a buddy is accepted to join, they should be added to group chat if it exists
-- =====================================
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
            SELECT 1 FROM conversation_participants -- SELECT 1 = Check if any row exists
            WHERE conversation_id = v_conversation_id
            AND user_id = v_user_id
        ) THEN
            SET @SESSION_TRIGGER_AUDIT_SKIP = TRUE;
        
            INSERT INTO conversation_participants (conversation_id, user_id)
            VALUES (v_conversation_id, v_user_id);
            
            SET @SESSION_TRIGGER_AUDIT_SKIP = NULL;

            -- Audit log
            INSERT INTO conversation_audits (
                conversation_id,
                affected_user_id,
                action,
                changed_by
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
-- ------------------------------------------------------------------ BUDDIES
-- =====================================
-- 8. Buddy can leave a trip destination
-- 9. Owner can remove a buddy from a trip destination.
-- =====================================
DROP PROCEDURE IF EXISTS remove_buddy_from_trip_destination;
DELIMITER $$
CREATE PROCEDURE remove_buddy_from_trip_destination(
    IN p_user_id INT,
    IN p_trip_destination_id INT,
    IN p_changed_by INT,
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

    IF p_changed_by IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'changed_by cannot be NULL';
    END IF;

    -- Resolve buddy record
    SELECT buddy_id
    INTO v_buddy_id
    FROM buddies
    WHERE user_id = p_user_id
      AND trip_destination_id = p_trip_destination_id
    LIMIT 1;

    START TRANSACTION;

    -- Mark buddy inactive
    UPDATE buddies
    SET is_active = FALSE,
        departure_reason = p_departure_reason
    WHERE buddy_id = v_buddy_id;

    -- Cleanup conversation
    CALL remove_buddy_from_conversation(v_buddy_id, p_changed_by);

    COMMIT;
    SELECT 1 AS Success;
END $$
DELIMITER ;
-- =====================================
-- 21. Trip owners should be able to see all pending buddy requests
-- =====================================
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
    FROM trips t
    JOIN trip_destinations td ON t.trip_id = td.trip_id
    JOIN destinations d ON td.destination_id = d.destination_id
    JOIN buddies b ON td.trip_destination_id = b.trip_destination_id
    JOIN users u ON b.user_id = u.user_id
    WHERE b.request_status = 'pending'
      AND t.owner_id = p_user_id
    ORDER BY td.start_date DESC ; # Example user 4
END $$
DELIMITER ;
-- =====================================
-- 13. Trip owners should be able to accept or reject buddy requests
-- =====================================
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
    FROM buddies
    WHERE buddy_id = p_buddy_id
    LIMIT 1;

    IF v_trip_destination_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Buddy not found';
    END IF;

    -- Get actual owner of the trip
    SELECT t.owner_id
    INTO v_trip_owner_id
    FROM trips t
    JOIN trip_destinations td ON td.trip_id = t.trip_id
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
    UPDATE buddies
    SET request_status = p_new_status
    WHERE buddy_id = p_buddy_id;

    -- Audit log
    INSERT INTO buddy_audits (buddy_id, action, reason, changed_by)
    VALUES (
        p_buddy_id,
        p_new_status,
        'Owner answered the request',
        p_owner_id
    );
END $$
DELIMITER ;
-- =====================================
-- 16. User should be able to request to join a trip destination
-- =====================================
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
        SELECT 1 FROM buddies
        WHERE user_id = p_user_id AND trip_destination_id = p_trip_destination_id
    ) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'User has already requested to join this trip destination';
    END IF;
    
    SET @SESSION_TRIGGER_AUDIT_SKIP = TRUE;

    INSERT INTO buddies (user_id, trip_destination_id, person_count, note)
    VALUES (p_user_id, p_trip_destination_id, p_person_count, p_note);
    
    SET @SESSION_TRIGGER_AUDIT_SKIP = NULL;

    SET v_buddy_id = LAST_INSERT_ID();

    INSERT INTO buddy_audits (buddy_id, action, reason, changed_by)
    VALUES (v_buddy_id, 'requested', 'Buddy request added', p_user_id);

    COMMIT;
END $$
DELIMITER ;
-- =====================================
-- Transaction for Accept + add to group chat
-- =====================================
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
-- ------------------------------------------------------------------ EVENTS
-- =====================================
-- 19. Every month, trip_destination and trip should be archived (is_archived = true)
-- if trip_destination end_date has passed
-- ===================================== monthly_archive_old_trip_destinations
DROP EVENT IF EXISTS monthly_archive_old_trip_destinations;
DELIMITER $$
CREATE EVENT monthly_archive_old_trip_destinations
ON SCHEDULE EVERY 1 MONTH
STARTS '2025-09-30 00:00:00'
ON COMPLETION PRESERVE -- keeps the event definition even after it runs (especially useful if you ever switch to a one-time event)
DO BEGIN
    UPDATE trip_destinations
    SET is_archived = true
    WHERE end_date < NOW();
END $$
DELIMITER ;
-- ===================================== monthly_archive_old_trips
DROP EVENT IF EXISTS monthly_archive_old_trips;
DELIMITER $$
CREATE EVENT monthly_archive_old_trips
ON SCHEDULE EVERY 1 MONTH
STARTS '2025-09-30 00:00:00'
ON COMPLETION PRESERVE
DO BEGIN
    UPDATE trips
    SET is_archived = true
    WHERE end_date < NOW();
END $$
DELIMITER ;
-- =====================================
-- 20. Every two weeks, check if a conversation has not been active for at least a week and make it archived
-- =====================================
DROP EVENT IF EXISTS archive_due_to_inactivity;
DELIMITER $$
CREATE EVENT archive_due_to_inactivity
ON SCHEDULE EVERY 2 WEEK
STARTS '2025-09-30 00:00:00'
ON COMPLETION PRESERVE
DO BEGIN
    UPDATE conversations
    SET is_archived = TRUE
    WHERE conversation_id IN (
        SELECT m.conversation_id
        FROM message m
        GROUP BY m.conversation_id
        HAVING MAX(m.sent_at) <= NOW() - INTERVAL 1 WEEK
    );
END $$
DELIMITER ;
-- ------------------------------------------------------------------ TRIGGERS
DROP TRIGGER IF EXISTS audit_archiving_of_trip_destination;
DELIMITER $$
CREATE TRIGGER audit_archiving_of_trip_destination
AFTER UPDATE ON trip_destinations
FOR EACH ROW
BEGIN
    IF OLD.is_archived = false AND NEW.is_archived = true THEN
        INSERT INTO system_event_logs (event_type, affected_id, details)
        VALUES (
            'ARCHIVE',
            NEW.trip_destination_id,
            'Archiving old trip destination' );
    END IF;
END $$
DELIMITER ;
-- ===================================== audit_archiving_of_trip
DROP TRIGGER IF EXISTS audit_archiving_of_trip;
DELIMITER $$
CREATE TRIGGER audit_archiving_of_trip
AFTER UPDATE ON trips
FOR EACH ROW
BEGIN
    IF OLD.is_archived = false AND NEW.is_archived = true THEN
        INSERT INTO system_event_logs (event_type, affected_id, details)
        VALUES (
            'ARCHIVE',
            NEW.trip_id,
            'Archiving old trip' );
    END IF;
END $$
DELIMITER ;
-- ===================================== audit_archiving_of_conversation
DROP TRIGGER IF EXISTS audit_archiving_of_conversation;
DELIMITER $$
CREATE TRIGGER audit_archiving_of_conversation
AFTER UPDATE ON conversations
FOR EACH ROW
BEGIN
    IF OLD.is_archived = false AND NEW.is_archived = true THEN
        INSERT INTO system_event_logs (event_type, affected_id, details)
        VALUES (
            'ARCHIVE',
            NEW.conversation_id,
            'Archiving inactive conversation' );
    END IF;

    IF OLD.is_archived = true AND NEW.is_archived = false THEN
        INSERT INTO system_event_logs (event_type, affected_id, details)
        VALUES (
            'UNARCHIVE',
            NEW.conversation_id,
            'Unarchiving previously inactive conversation' );
    END IF;
END $$
DELIMITER ;
-- ADDITIONAL TRIGGERS FOR AUDIT TABLES WHEN ADDING SEED DATA
DROP TRIGGER IF EXISTS trg_trip_after_insert;
DROP TRIGGER IF EXISTS trg_conversation_after_insert;
DROP TRIGGER IF EXISTS trg_conversation_participant_after_insert;
DROP TRIGGER IF EXISTS trg_buddy_after_insert;
DROP TRIGGER IF EXISTS trg_user_after_insert;
DROP TRIGGER IF EXISTS trg_user_after_update;
-- =====================================
DELIMITER $$
-- Logs new user creation
CREATE TRIGGER trg_user_after_insert
AFTER INSERT ON users
FOR EACH ROW
BEGIN
    -- Skip if the session variable is set (e.g., if insert is inside a controlled procedure like 'create_trip')
    IF @SESSION_TRIGGER_AUDIT_SKIP IS NULL OR @SESSION_TRIGGER_AUDIT_SKIP != TRUE THEN
        INSERT INTO user_audits (
            user_id,
            action,
            field_changed,
            old_value,
            new_value,
            changed_by
        )
        VALUES (
            NEW.user_id,
            'created',
            'user',
            NULL,
            NULL,
            NEW.user_id
        );
    END IF;
END $$
-- =====================================
CREATE TRIGGER trg_user_after_update
AFTER UPDATE ON users
FOR EACH ROW
BEGIN
    IF @SESSION_TRIGGER_AUDIT_SKIP IS NULL OR @SESSION_TRIGGER_AUDIT_SKIP != TRUE THEN
        IF NEW.email <> OLD.email THEN
            INSERT INTO user_audits (
                user_id,
                action,
                field_changed,
                old_value,
                new_value,
                changed_by
            )
            VALUES (
                NEW.user_id,
                'updated',
                'email',
                OLD.email,
                NEW.email,
                NULL
            );
        END IF;
        IF NEW.password_hash <> OLD.password_hash THEN
            INSERT INTO user_audits (
                user_id,
                action,
                field_changed,
                old_value,
                new_value,
                changed_by
            )
            VALUES (
                NEW.user_id,
                'updated',
                'password_hash',
                'Hidden',
                'Hidden',
                NULL
            );
        END IF;
        IF NEW.role <> OLD.role THEN
            INSERT INTO user_audits (
                user_id,
                action,
                field_changed,
                old_value,
                new_value,
                changed_by
            )
            VALUES (
                NEW.user_id,
                'updated',
                'role',
                OLD.role,
                NEW.role,
                NULL
            );
        END IF;
        IF NEW.name <> OLD.name THEN
            INSERT INTO user_audits (
                user_id,
                action,
                field_changed,
                old_value,
                new_value,
                changed_by
            )
            VALUES (
                NEW.user_id,
                'updated',
                'name',
                OLD.name,
                NEW.name,
                NULL
            );
        END IF;
        IF NEW.birthdate <> OLD.birthdate THEN
            INSERT INTO user_audits (
                user_id,
                action,
                field_changed,
                old_value,
                new_value,
                changed_by
            )
            VALUES (
                NEW.user_id,
                'updated',
                'birthdate',
                OLD.birthdate,
                NEW.birthdate,
                NULL
            );
        END IF;
        IF NEW.is_deleted <> OLD.is_deleted THEN
            INSERT INTO user_audits (
                user_id,
                action,
                field_changed,
                old_value,
                new_value,
                changed_by
            )
            VALUES (
                NEW.user_id,
                'updated',
                'is_deleted',
                OLD.is_deleted,
                NEW.is_deleted,
                NULL
            );
        END IF;
    END IF;
END$$
-- =====================================
-- Logs new trip creation
CREATE TRIGGER trg_trip_after_insert
AFTER INSERT ON trips
FOR EACH ROW
BEGIN
    -- Skip if the session variable is set (e.g., if insert is inside a controlled procedure like 'create_trip')
    IF @SESSION_TRIGGER_AUDIT_SKIP IS NULL OR @SESSION_TRIGGER_AUDIT_SKIP != TRUE THEN
        INSERT INTO trip_audits (
            trip_id,
            action,
            field_changed,
            old_value,
            new_value,
            changed_by
        )
        VALUES (
            NEW.trip_id,
            'created',
            'trip_name',
            NULL,
            NEW.trip_name,
            NEW.owner_id
        );
    END IF;
END $$
-- =====================================
-- Logs new conversation creation
CREATE TRIGGER trg_conversation_after_insert
AFTER INSERT ON conversations
FOR EACH ROW
BEGIN
    -- Skip if the session variable is set, as procedures like 'create_group_conversation_for_trip_destination' already insert into conversation_audit
    IF @SESSION_TRIGGER_AUDIT_SKIP IS NULL OR @SESSION_TRIGGER_AUDIT_SKIP != TRUE THEN
        -- If the insert is NOT from a controlled procedure, we try to create a basic audit log entry.
        -- NOTE: We cannot determine the `changed_by` user here easily, so we use NULL.
        INSERT INTO conversation_audits (
            conversation_id,
            affected_user_id,
            action,
            changed_by
        )
        VALUES (
            NEW.conversation_id,
            NULL,
            'created',
            NULL
        );
    END IF;
END $$
-- =====================================
-- Logs when a participant is added to a conversation
CREATE TRIGGER trg_conversation_participant_after_insert
AFTER INSERT ON conversation_participants
FOR EACH ROW
BEGIN
    -- Skip if the session variable is set, as procedures like 'insert_new_private_conversation' already insert into conversation_audit
    IF @SESSION_TRIGGER_AUDIT_SKIP IS NULL OR @SESSION_TRIGGER_AUDIT_SKIP != TRUE THEN
        INSERT INTO conversation_audits (
            conversation_id,
            affected_user_id,
            action,
            changed_by
        )
        VALUES (
            NEW.conversation_id,
            NEW.user_id,
            'user_added',
            NULL
        );
    END IF;
END $$
-- =====================================
-- Logs when a new buddy entry is created (request to join)
CREATE TRIGGER trg_buddy_after_insert
AFTER INSERT ON buddies
FOR EACH ROW
BEGIN
    -- Skip if the session variable is set (meaning a stored procedure is handling the audit)
    IF @SESSION_TRIGGER_AUDIT_SKIP IS NULL OR @SESSION_TRIGGER_AUDIT_SKIP != TRUE THEN
        INSERT INTO buddy_audits (
            buddy_id,
            action,
            reason,
            changed_by
        )
        VALUES (
            NEW.buddy_id,
            'requested',
            'Buddy request created outside of procedure',
            NEW.user_id
        );
    END IF;
END $$
DELIMITER ;