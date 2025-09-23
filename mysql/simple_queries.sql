USE travel_buddy;

SELECT *
FROM trip_destination
INNER JOIN destination USING (destination_id)
WHERE trip_id = 50
ORDER BY sequence_number;

SELECT *
FROM destination;