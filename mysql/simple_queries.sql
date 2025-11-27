USE travel_buddy;
show tables;

SELECT *
FROM buddy
JOIN travel_buddy.user u on u.user_id = buddy.user_id
WHERE u.user_id = 1;

SELECT *
FROM conversation_participant
WHERE conversation_id = 9;

# td id: 28, 23, 21, 19
SELECT trip_id
FROM trip_destination
WHERE trip_destination_id = 21;
# trip id: 13
SELECT owner_id
FROM trip
WHERE trip_id = 10;
# owner 73

CALL remove_buddy_from_trip_destination(1, 129, 1, 'trying');
CALL get_user_trips(1);


DELETE from buddy where buddy_id = 431;

UPDATE user
SET role = 'admin'
where email = 'admin@admin.com';

CALL get_user_trips(1);

-- See itinerary (all destinations) for a trip
SELECT *
FROM trip_destination
INNER JOIN destination USING (destination_id)
WHERE trip_id = 11
ORDER BY sequence_number;

-- Find all trips that has passed
SELECT *
FROM trip
WHERE end_date < NOW();

-- Find all trips in the future
SELECT *
FROM trip
WHERE start_date > NOW();

-- See all users and buddies for a trip
SELECT
    'Owner' AS user_role,
    user.name AS user_name,
    'All' AS trip_destination_id
FROM trip
INNER JOIN user ON trip.owner_id = user.user_id
WHERE trip.trip_id = 11

UNION ALL

SELECT
    'Buddy' AS user_role,
    user.name AS user_name,
  destination.name
FROM trip
INNER JOIN trip_destination ON trip.trip_id = trip_destination.trip_id
INNER JOIN buddy ON trip_destination.trip_destination_id = buddy.trip_destination_id
INNER JOIN user ON buddy.user_id = user.user_id
INNER JOIN destination USING (destination_id)
WHERE trip.trip_id = 11
ORDER BY trip_destination_id;

