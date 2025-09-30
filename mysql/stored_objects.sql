USE travel_buddy;
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
