-- =====================================
-- Create Database
-- =====================================
CREATE DATABASE IF NOT EXISTS travel_buddy;
USE travel_buddy;

DROP TABLE IF EXISTS message;
DROP TABLE IF EXISTS buddy;
DROP TABLE IF EXISTS conversation_participant;
DROP TABLE IF EXISTS conversation;
DROP TABLE IF EXISTS message;
DROP TABLE IF EXISTS trip_destination;
DROP TABLE IF EXISTS destination;
DROP TABLE IF EXISTS trip;
DROP TABLE IF EXISTS user;
DROP TABLE IF EXISTS archived_conversation;
DROP TABLE IF EXISTS archived_conversation_participant;
DROP TABLE IF EXISTS archived_message;
DROP TABLE IF EXISTS archived_trip_destination;

-- =====================================
-- User Table (with soft delete flag)
-- =====================================
CREATE TABLE IF NOT EXISTS user (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    birthdate DATE NOT NULL,
    is_deleted BOOLEAN DEFAULT FALSE
);

-- =====================================
-- Trip Table (preserve trip if owner is deleted)
-- =====================================
CREATE TABLE IF NOT EXISTS trip (
    trip_id INT AUTO_INCREMENT PRIMARY KEY,
    owner_id INT,
    max_buddies INT CHECK (max_buddies >= 1),
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    description TEXT,
    CONSTRAINT fk_trip_owner FOREIGN KEY (owner_id) REFERENCES user(user_id)
        ON DELETE SET NULL ON UPDATE CASCADE,
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
CREATE TABLE IF NOT EXISTS trip_destination (
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
    request_status ENUM('pending', 'accepted', 'declined') NOT NULL DEFAULT 'pending',
    CONSTRAINT fk_buddy_user FOREIGN KEY (user_id) REFERENCES user(user_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_buddy_tripDestination FOREIGN KEY (trip_destination_id) REFERENCES trip_destination(trip_destination_id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- =====================================
-- Conversation Table
-- =====================================
CREATE TABLE IF NOT EXISTS conversation (
    conversation_id INT AUTO_INCREMENT PRIMARY KEY,
    trip_destination_id INT,
    is_group BOOLEAN DEFAULT FALSE,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_conversation_trip FOREIGN KEY (trip_destination_id) REFERENCES trip_destination(trip_destination_id)
        ON DELETE SET NULL ON UPDATE CASCADE
);

-- =====================================
-- Conversation Participant Table
-- =====================================
CREATE TABLE IF NOT EXISTS conversation_participant (
    conversation_id INT NOT NULL,
    user_id INT NOT NULL,
    joined_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (conversation_id, user_id),
    CONSTRAINT fk_cp_conversation FOREIGN KEY (conversation_id) REFERENCES conversation(conversation_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_cp_user FOREIGN KEY (user_id) REFERENCES user(user_id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- =====================================
-- Message Table (preserve messages if sender is deleted)
-- =====================================
CREATE TABLE IF NOT EXISTS message (
    message_id INT AUTO_INCREMENT PRIMARY KEY,
    sender_id INT,
    content TEXT NOT NULL,
    sent_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    conversation_id INT NOT NULL,
    CONSTRAINT fk_message_sender FOREIGN KEY (sender_id) REFERENCES user(user_id)
        ON DELETE SET NULL ON UPDATE CASCADE,
    CONSTRAINT fk_message_conversation FOREIGN KEY (conversation_id) REFERENCES conversation(conversation_id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- =====================================
-- Archive Tables
-- This script creates archive tables for trips and their related data
-- =====================================

-- Create the archived_trip_destination table
-- This table will store completed trips and their destinations.
CREATE TABLE IF NOT EXISTS archived_trip_destination (
    trip_destination_id INT PRIMARY KEY,
    trip_id INT NOT NULL,
    destination_id INT NOT NULL,
    start_date DATE,
    end_date DATE,
    description TEXT,
    -- Add an archived_at timestamp to track when the data was moved
    archived_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_arch_td_trip FOREIGN KEY (trip_id) REFERENCES trip(trip_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_arch_td_destination FOREIGN KEY (destination_id) REFERENCES destination(destination_id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- Create the archived_conversation table
-- This table will store conversations related to completed trips.
CREATE TABLE IF NOT EXISTS archived_conversation (
    conversation_id INT PRIMARY KEY,
    trip_destination_id INT,
    is_group BOOLEAN DEFAULT FALSE,
    created_at DATETIME,
    archived_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_arch_convo_td FOREIGN KEY (trip_destination_id) REFERENCES archived_trip_destination(trip_destination_id)
        ON DELETE SET NULL ON UPDATE CASCADE
);

-- Create the archived_conversation_participant table
-- This table will store participants for archived conversations.
CREATE TABLE IF NOT EXISTS archived_conversation_participant (
    conversation_id INT NOT NULL,
    user_id INT NOT NULL,
    joined_at DATETIME,
    archived_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (conversation_id, user_id),
    CONSTRAINT fk_arch_cp_convo FOREIGN KEY (conversation_id) REFERENCES archived_conversation(conversation_id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- Create the archived_message table
-- This table will store messages from archived conversations.
CREATE TABLE IF NOT EXISTS archived_message (
    message_id INT PRIMARY KEY,
    sender_id INT,
    content TEXT NOT NULL,
    sent_at DATETIME,
    conversation_id INT NOT NULL,
    archived_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_arch_msg_convo FOREIGN KEY (conversation_id) REFERENCES archived_conversation(conversation_id)
        ON DELETE CASCADE ON UPDATE CASCADE
);