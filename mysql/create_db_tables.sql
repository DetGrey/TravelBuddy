-- =====================================
-- Create Database
-- =====================================
CREATE DATABASE IF NOT EXISTS travel_buddy;
USE travel_buddy;

DROP TABLE IF EXISTS itinerary;
DROP TABLE IF EXISTS buddy;
DROP TABLE IF EXISTS destination;
DROP TABLE IF EXISTS trip;
DROP TABLE IF EXISTS user;

-- =====================================
-- User Table
-- =====================================
CREATE TABLE IF NOT EXISTS user (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    birthdate DATE NOT NULL
);

-- =====================================
-- Trip Table
-- =====================================
CREATE TABLE IF NOT EXISTS trip (
    trip_id INT AUTO_INCREMENT PRIMARY KEY,
    owner_id INT NOT NULL,
    max_buddies INT CHECK (max_buddies >= 1),
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    description TEXT,
    CONSTRAINT fk_trip_owner FOREIGN KEY (owner_id) REFERENCES user(user_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT chk_trip_dates CHECK (end_date >= start_date)
);

-- =====================================
-- Destination Table
-- =====================================
CREATE TABLE IF NOT EXISTS destination (
    destination_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(150) NOT NULL,
    address VARCHAR(255),
    zip_code VARCHAR(20),
    city VARCHAR(100),
    state VARCHAR(100),
    country VARCHAR(100) NOT NULL,
    longitude DECIMAL(10, 7),
    latitude DECIMAL(10, 7)
);

-- =====================================
-- Itinerary Table
-- =====================================
CREATE TABLE IF NOT EXISTS itinerary (
    trip_destination_id INT AUTO_INCREMENT PRIMARY KEY,
    destination_id INT NOT NULL,
    trip_id INT NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    sequence_number INT NOT NULL,
    description TEXT,
    CONSTRAINT fk_itinerary_destination FOREIGN KEY (destination_id) REFERENCES destination(destination_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_itinerary_trip FOREIGN KEY (trip_id) REFERENCES trip(trip_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT chk_itinerary_dates CHECK (end_date >= start_date)
);

-- =====================================
-- Buddy Table
-- =====================================
CREATE TABLE IF NOT EXISTS buddy (
    buddy_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    trip_destination_id INT NOT NULL,
    person_count INT DEFAULT 1 CHECK (person_count >= 1),
    note VARCHAR(255),
    invite_status ENUM('pending', 'accepted', 'declined') NOT NULL DEFAULT 'pending',
    CONSTRAINT fk_buddy_user FOREIGN KEY (user_id) REFERENCES user(user_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_buddy_tripDestination FOREIGN KEY (trip_destination_id) REFERENCES itinerary(trip_destination_id)
        ON DELETE CASCADE ON UPDATE CASCADE
);
