USE travel_buddy;
show tables;

SELECT *
FROM trip_destination;

SELECT *
FROM trip_destination
INNER JOIN destination USING (destination_id)
WHERE trip_id = 11
ORDER BY sequence_number;

SELECT *
FROM trip_destination
INNER JOIN destination USING (destination_id)
ORDER BY trip_id;