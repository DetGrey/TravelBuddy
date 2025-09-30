USE travel_buddy;
# 1. User can search for a trip destination using start/end dates, buddy count and location.
# Maybe also description – for specific activities.



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
