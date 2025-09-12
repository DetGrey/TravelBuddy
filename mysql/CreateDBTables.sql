-- =====================================
-- Create Database
-- =====================================
CREATE DATABASE IF NOT EXISTS TravelBuddy;
USE TravelBuddy;

-- =====================================
-- User Table
-- =====================================

DROP TABLE IF EXISTS User;
CREATE TABLE IF NOT EXISTS User (
    userId INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    passwordHash VARCHAR(255) NOT NULL,
    birthdate DATE NOT NULL
);

-- =====================================
-- Trip Table
-- =====================================
DROP TABLE IF EXISTS Trip;
CREATE TABLE IF NOT EXISTS Trip (
    tripId INT AUTO_INCREMENT PRIMARY KEY,
    ownerId INT NOT NULL,
    maxBuddies INT CHECK (maxBuddies >= 1),
    startDate DATE NOT NULL,
    endDate DATE NOT NULL,
    description TEXT,
    CONSTRAINT fk_trip_owner FOREIGN KEY (ownerId) REFERENCES User(userId)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT chk_trip_dates CHECK (endDate >= startDate)
);

-- =====================================
-- Destination Table
-- =====================================
DROP TABLE IF EXISTS Destination;
CREATE TABLE IF NOT EXISTS Destination (
    destinationId INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(150) NOT NULL,
    address VARCHAR(255),
    zipCode VARCHAR(20),
    city VARCHAR(100),
    state VARCHAR(100),
    country VARCHAR(100) NOT NULL,
    longitude DECIMAL(10, 7),
    latitude DECIMAL(10, 7)
);

-- =====================================
-- Itinerary Table
-- =====================================
DROP TABLE IF EXISTS Itinerary;
CREATE TABLE IF NOT EXISTS Itinerary (
    tripDestinationId INT AUTO_INCREMENT PRIMARY KEY,
    destinationId INT NOT NULL,
    tripId INT NOT NULL,
    startDate DATE NOT NULL,
    endDate DATE NOT NULL,
    sequenceNumber INT NOT NULL,
    description TEXT,
    CONSTRAINT fk_itinerary_destination FOREIGN KEY (destinationId) REFERENCES Destination(destinationId)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_itinerary_trip FOREIGN KEY (tripId) REFERENCES Trip(tripId)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT chk_itinerary_dates CHECK (endDate >= startDate)
);

-- =====================================
-- Buddy Table
-- =====================================
DROP TABLE IF EXISTS Buddy;
CREATE TABLE IF NOT EXISTS Buddy (
    buddyId INT AUTO_INCREMENT PRIMARY KEY,
    userId INT NOT NULL,
    tripDestinationId INT NOT NULL,
    personCount INT DEFAULT 1 CHECK (personCount >= 1),
    note VARCHAR(255),
    inviteStatus ENUM('pending', 'accepted', 'declined') NOT NULL DEFAULT 'pending',
    CONSTRAINT fk_buddy_user FOREIGN KEY (userId) REFERENCES User(userId)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_buddy_tripDestination FOREIGN KEY (tripDestinationId) REFERENCES Itinerary(tripDestinationId)
        ON DELETE CASCADE ON UPDATE CASCADE
);
