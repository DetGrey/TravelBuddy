USE travel_buddy;

SELECT *
FROM itinerary
INNER JOIN destination USING (destination_id)
WHERE trip_id = 11
ORDER BY sequence_number;

SELECT *
FROM destination;