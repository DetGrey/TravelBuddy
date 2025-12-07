-- #################################################################### USER FOR BACKEND API
-- 1. Create the dedicated user for the web application
-- Note: Replace 'YourStrongPassword123' and ensure the host ('%') is correct for your AWS setup
DROP USER IF EXISTS 'backend_api'@'%';
CREATE USER 'backend_api'@'%'
IDENTIFIED BY 'backend_api_password';

-- 2. Grant all necessary DML privileges on all tables within the 'travel_buddy' database
GRANT SELECT, INSERT, UPDATE, DELETE
ON travel_buddy.* TO 'backend_api'@'%';

-- 3. Grant the EXECUTE privilege needed to run Stored Procedures
GRANT EXECUTE
ON travel_buddy.* TO 'backend_api'@'%';

-- 4. Apply the changes
FLUSH PRIVILEGES;

-- #################################################################### USER FOR MIGRATOR
DROP USER IF EXISTS 'migrator'@'%';
CREATE USER 'migrator'@'%'
IDENTIFIED BY 'migrator_password';

GRANT SELECT
ON travel_buddy.* TO 'migrator'@'%';

FLUSH PRIVILEGES;

-- #################################################################### USER FOR RESTRICTED READING
-- 1. Create the dedicated user
DROP USER IF EXISTS 'guest_reader'@'%';
CREATE USER 'guest_reader'@'%'
IDENTIFIED BY 'password123';

-- 2. Grant SELECT privileges only to see available trips (search for trips
GRANT SELECT
ON travel_buddy.trips TO 'guest_reader'@'%';
GRANT SELECT
ON travel_buddy.trip_destinations TO 'guest_reader'@'%';
GRANT SELECT
ON travel_buddy.destinations TO 'guest_reader'@'%';
-- 3. Grant EXECUTE privilege ONLY on the specific 'search_trips' stored procedure
-- This allows them to run this procedure but no others.
GRANT EXECUTE
ON PROCEDURE travel_buddy.search_trips TO 'guest_reader'@'%';

-- 4. Apply the changes
FLUSH PRIVILEGES;