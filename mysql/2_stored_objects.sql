USE travel_buddy;
# INDEX
-- Date + FK'er
CREATE INDEX idx_td_dates on trip_destination (start_date, end_date);
CREATE INDEX idx_td_trip on trip_destination (trip_id);
# Lavet allerede CREATE Index idx_td_destination on trip_destination(destination_id);

-- Location
CREATE INDEX idx_dest_contry_state ON destination (country,state);

-- Buddies (count)
CREATE INDEX idx_buddy_seg_status on buddy (trip_destination_id, request_status);

# GLOBAL VIEWS

# GLOBAL FUNCTIONS
DROP FUNCTION IF EXISTS get_user_id_from_buddy;
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
END;

DROP FUNCTION IF EXISTS get_group_conversation_for_buddy;
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
END;

# 1. User can search for a trip destination using start/end dates, buddy count and location.
# Maybe also description – for specific activities.
WITH accepted_by_trip AS (
    SELECT td.trip_id,
           SUM(b.person_count) as accepted_persons
    FROM buddy b
    JOIN trip_destination td
    ON td.trip_destination_id = b.trip_destination_id
    WHERE b.request_status = 'accepted'
    GROUP BY td.trip_id
)

SELECT
    td.trip_destination_id,
    t.trip_id,
    d.destination_id,
    d.name as destination_name,
    d.country,
    d.state,
    td.start_date as destination_start,
    td.end_date as destination_end,
    t.max_buddies,
    COALESCE(abt.accepted_persons, 0) AS accepted_persons,
    (t.max_buddies - COALESCE(abt.accepted_persons, 0)) AS remaining_capacity
FROM trip_destination td
JOIN trip t ON t.trip_id = td.trip_id
JOIN destination d ON d.destination_id = td.destination_id
LEFT JOIN accepted_by_trip abt ON abt.trip_id = t.trip_id
WHERE
    (
        (:req_start IS NULL AND :req_end IS NULL)
        OR (:req_start IS NOT NULL AND :req_end IS NOT NULL
            AND NOT (td.end_date < :req_start OR td.start_date > :req_end))
        OR (:req_start IS NOT NULL AND :req_end IS NOT NULL
            AND td.end_date >= :req_start)
        OR (:req_start IS NOT NULL AND :req_end IS NOT NULL
            AND td.start_date <= :req_end)
    )
    AND (:country IS NULL OR :country = '' OR d.country = :country)
    AND (:state   IS NULL OR :state   = '' OR d.state   = :state)
    AND (
        :party_size IS NULL
        OR (COALESCE(t.max_buddies, 0) - COALESCE(abt.accepted_persons, 0)) >= :party_size
        )
    AND (
        :q IS NULL OR :q = ''
        OR td.description LIKE CONCAT('%', :q, '%')
        OR d.name LIKE CONCAT('%', :q, '%')
    )
ORDER BY td.start_date, d.name;

-- Start of notes
SELECT COUNT(*) FROM trip_destination;
SELECT * FROM destination WHERE country = 'Spain';
SELECT MIN(start_date), MAX(end_date) FROM trip_destination;

SELECT d.country, COUNT(*) as segments
From trip_destination td
Join destination d ON d.destination_id = td.destination_id
GROUP BY d.country
order by segments DESC, d.country;

SELECT td.trip_destination_id, td.trip_id,
       d.destination_id, d.name, d.state, d.country,
       td.start_date, td.end_date
FROM trip_destination td
JOIN destination d ON d.destination_id = td.destination_id
ORDER BY td.start_date
LIMIT 20;


SELECT * from trip_destination;
SELECT * from destination;
SELECT * FROM buddy;
-- End of Notes

# 3. User should see all conversations they are part of (also archived ones)
DROP PROCEDURE IF EXISTS get_user_conversations;
DELIMITER $$
CREATE PROCEDURE get_user_conversations(IN p_user_id INT)
BEGIN
    SELECT
        c.conversation_id,
        m.content,
        CASE
            WHEN DATE(sent_at) = CURDATE() THEN DATE_FORMAT(sent_at, '%H:%i')
            WHEN DATE(sent_at) = CURDATE() - INTERVAL 1 DAY THEN 'Yesterday'
            WHEN YEAR(sent_at) = YEAR(CURDATE()) THEN DATE_FORMAT(sent_at, '%d/%m')
            ELSE DATE_FORMAT(sent_at, '%Y')
        END AS formatted_sent_at
    FROM conversation c
    JOIN conversation_participant p ON c.conversation_id = p.conversation_id
    LEFT JOIN message m ON m.message_id = (
        SELECT m2.message_id
        FROM message m2
        WHERE m2.conversation_id = c.conversation_id
        ORDER BY m2.sent_at DESC
        LIMIT 1
    )
    WHERE p.user_id = p_user_id; # Example user 49 has 3 convos
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
        c.conversation_id,
        c.trip_destination_id,
        JSON_OBJECT(
            'destination', d.name,
            'date', td.start_date
        ) AS trip_destination,
        COUNT(DISTINCT u.user_id) AS participant_count,
        JSON_ARRAYAGG(
            JSON_OBJECT(
                'user_id', u.user_id,
                'name', u.name
            )
        ) AS participants
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
      m.message_id,
      m.sender_id,
      u.name,
      m.sent_at,
      m.content
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
    IN p_content TEXT
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

# 10. If a buddy is not active anymore (left or removed) they should be removed from group conversation
DROP PROCEDURE IF EXISTS remove_buddy_from_conversation;
DELIMITER $$
CREATE PROCEDURE remove_buddy_from_conversation(IN p_buddy_id INT)
BEGIN
    DECLARE v_user_id INT;
    DECLARE v_conversation_id INT;

    IF p_buddy_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'buddy_id cannot be NULL';
    END IF;

    SET v_conversation_id = get_group_conversation_for_buddy(p_buddy_id);
    -- If no conversation exists, skip deletion
    IF v_conversation_id IS NOT NULL THEN
        SET v_user_id = get_user_id_from_buddy(p_buddy_id);

        DELETE FROM conversation_participant
        WHERE user_id = v_user_id
        AND conversation_id = v_conversation_id;
    END IF;
END $$
DELIMITER ;

# 13. Trip owners should be able to accept or reject buddy requests
DROP PROCEDURE IF EXISTS update_buddy_request;
DELIMITER $$
CREATE PROCEDURE update_buddy_request(
    IN p_buddy_id INT,
    IN p_new_status VARCHAR(10)
)
BEGIN
    IF p_buddy_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'buddy_id cannot be NULL';
    END IF;

    IF p_new_status NOT IN ('accepted', 'rejected') THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Invalid status. Must be accepted or rejected';
    END IF;

    -- Update buddy request status
    UPDATE buddy
    SET request_status = p_new_status
    WHERE buddy_id = p_buddy_id;
END $$
DELIMITER ;

# 15. Owner should be able to create a group chat for a trip destination
DROP PROCEDURE IF EXISTS create_group_conversation_for_trip_destination;
DELIMITER $$
CREATE PROCEDURE create_group_conversation_for_trip_destination(
    IN p_trip_destination_id INT
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

        -- Step 3: Add all buddies for this trip to the conversation
        INSERT INTO conversation_participant (conversation_id, user_id)
        SELECT v_conversation_id, user_id
        FROM buddy
        WHERE trip_destination_id = p_trip_destination_id
        AND request_status = 'accepted';
    END IF;
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
        END IF;
    END IF;
END $$
DELIMITER ;

# 18. User should be able to make a new convo with a user (optional if it is connected to a trip)
DROP PROCEDURE IF EXISTS insert_new_conversation;
DELIMITER $$
CREATE PROCEDURE insert_new_conversation(
    IN p_trip_destination_id INT,
    IN p_is_group BOOL)
BEGIN
    IF p_is_group IS NULL THEN
        INSERT INTO conversation (trip_destination_id, is_group)
        VALUES (p_trip_destination_id, FALSE);
    ELSE
        INSERT INTO conversation (trip_destination_id, is_group)
        VALUES (p_trip_destination_id, is_group);
    END IF;
END $$
DELIMITER ;

DROP PROCEDURE IF EXISTS insert_new_conversation_participants;
DELIMITER $$
CREATE PROCEDURE insert_new_conversation_participants(
    IN p_conversation_id INT,
    IN p_user_ids_json JSON
)
BEGIN
    DECLARE i INT DEFAULT 0;
    DECLARE user_id_count INT;

    IF p_conversation_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'conversation_id cannot be NULL';
    END IF;

    SET user_id_count = JSON_LENGTH(p_user_ids_json);

    WHILE i < user_id_count DO
        -- Extract the user ID at position i from the JSON array
        -- JSON_EXTRACT gets the value at index i (e.g., $[0], $[1], ...)
        -- JSON_UNQUOTE removes surrounding quotes only if the value is a string (in case the json contains string instead of int)
        INSERT INTO conversation_participant (conversation_id, user_id)
        VALUES (
            p_conversation_id,
            JSON_UNQUOTE(JSON_EXTRACT(p_user_ids_json, CONCAT('$[', i, ']')))
        );

        -- Move to the next index
        SET i = i + 1;
    END WHILE;
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

DROP EVENT IF EXISTS monthly_archive_old_trips $$

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

# 20. Every two weeks, check if a conversation has not been active for at least a week
# and make it archived
DROP EVENT IF EXISTS monthly_archive_old_trips;
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

# 21. Trip owners should be able to see all pending buddy requests
DROP PROCEDURE IF EXISTS get_buddy_requests;
DELIMITER $$
CREATE PROCEDURE get_buddy_requests(IN p_user_id INT)
BEGIN
    IF p_user_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'user_id cannot be NULL';
    END IF;

    SELECT
      td.trip_id,
      d.name AS destination,
      u.user_id,
      u.name
    FROM trip t
    JOIN trip_destination td ON t.trip_id = td.trip_id
    JOIN destination d ON td.destination_id = d.destination_id
    JOIN buddy b ON td.trip_destination_id = b.trip_destination_id
    JOIN user u ON b.user_id = u.user_id
    WHERE b.request_status = 'pending'
      AND t.owner_id = p_user_id; # Example user 4
END $$
DELIMITER ;