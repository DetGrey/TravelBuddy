-- =====================================
-- Create Database (emoji-safe defaults)
-- =====================================
DROP DATABASE IF EXISTS travel_buddy;
CREATE DATABASE IF NOT EXISTS travel_buddy
  CHARACTER SET utf8mb4          -- Use utf8mb4 to support emojis and multilingual text
  COLLATE utf8mb4_unicode_ci;    -- Unicode collation for consistent sorting/comparison
-- =====================================
USE travel_buddy;
-- =====================================
DROP TABLE IF EXISTS messages;
DROP TABLE IF EXISTS buddy_audits;
DROP TABLE IF EXISTS buddies;
DROP TABLE IF EXISTS conversation_audits;
DROP TABLE IF EXISTS conversation_participants;
DROP TABLE IF EXISTS conversations;
DROP TABLE IF EXISTS messages;
DROP TABLE IF EXISTS trip_audits;
DROP TABLE IF EXISTS trip_destinations;
DROP TABLE IF EXISTS destinations;
DROP TABLE IF EXISTS trips;
DROP TABLE IF EXISTS user_audits;
DROP TABLE IF EXISTS users;
DROP TABLE IF EXISTS system_event_logs;
-- =====================================
-- Users Table
-- =====================================
CREATE TABLE IF NOT EXISTS users (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL CHECK (CHAR_LENGTH(TRIM(name)) > 0),
    email VARCHAR(150) NOT NULL UNIQUE CHECK (CHAR_LENGTH(email) >= 6),
    password_hash VARCHAR(255) NOT NULL CHECK (CHAR_LENGTH(TRIM(password_hash)) > 0),
    birthdate DATE NOT NULL,
    is_deleted BOOLEAN DEFAULT FALSE NOT NULL,
    role ENUM('user', 'admin') DEFAULT 'user' NOT NULL
);
-- =====================================
-- Destinations Table
-- =====================================
CREATE TABLE destinations (
    destination_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL CHECK (CHAR_LENGTH(TRIM(name)) > 0),
    state VARCHAR(100) CHECK (CHAR_LENGTH(TRIM(state)) > 0),
    country VARCHAR(100) NOT NULL CHECK (CHAR_LENGTH(TRIM(country)) > 0),
    longitude DECIMAL(10, 7) CHECK (longitude BETWEEN -180 AND 180),
    latitude DECIMAL(10, 7) CHECK (latitude BETWEEN -90 AND 90)
);
-- =====================================
-- Trips Table
-- =====================================
CREATE TABLE IF NOT EXISTS trips (
    trip_id INT AUTO_INCREMENT PRIMARY KEY,
    owner_id INT,
    trip_name VARCHAR(100) NOT NULL CHECK (CHAR_LENGTH(TRIM(trip_name)) > 0),
    max_buddies INT CHECK (max_buddies >= 1),
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    description VARCHAR(255) CHECK (CHAR_LENGTH(TRIM(description)) > 0),
    is_archived BOOLEAN DEFAULT FALSE NOT NULL,
    CONSTRAINT fk_trip_owner FOREIGN KEY (owner_id) REFERENCES users(user_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT chk_trip_dates CHECK (end_date >= start_date)
);
-- =====================================
-- Trip destinations Table
-- =====================================
CREATE TABLE IF NOT EXISTS trip_destinations (
    trip_destination_id INT AUTO_INCREMENT PRIMARY KEY,
    destination_id INT NOT NULL,
    trip_id INT NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    sequence_number INT NOT NULL,
    description VARCHAR(255) CHECK (CHAR_LENGTH(TRIM(description)) > 0),
    is_archived BOOLEAN DEFAULT FALSE,
    CONSTRAINT fk_itinerary_destination FOREIGN KEY (destination_id) REFERENCES destinations(destination_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_itinerary_trip FOREIGN KEY (trip_id) REFERENCES trips(trip_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT chk_itinerary_dates CHECK (end_date >= start_date)
);
-- =====================================
-- Buddies Table
-- =====================================
CREATE TABLE IF NOT EXISTS buddies (
    buddy_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    trip_destination_id INT NOT NULL,
    person_count INT DEFAULT 1 CHECK (person_count >= 1),
    note VARCHAR(255) CHECK (CHAR_LENGTH(TRIM(note)) > 0),
    is_active BOOLEAN DEFAULT TRUE,
    departure_reason VARCHAR(255) DEFAULT NULL CHECK (CHAR_LENGTH(TRIM(departure_reason)) > 0),
    request_status ENUM('pending', 'accepted', 'rejected') NOT NULL DEFAULT 'pending',
    CONSTRAINT fk_buddy_user FOREIGN KEY (user_id) REFERENCES users(user_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_buddy_tripDestination FOREIGN KEY (trip_destination_id) REFERENCES trip_destinations(trip_destination_id)
        ON DELETE CASCADE ON UPDATE CASCADE
);
-- =====================================
-- Conversations Table
-- =====================================
CREATE TABLE IF NOT EXISTS conversations (
    conversation_id INT AUTO_INCREMENT PRIMARY KEY,
    trip_destination_id INT,
    is_group BOOLEAN DEFAULT FALSE NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    is_archived BOOLEAN DEFAULT FALSE NOT NULL,
    CONSTRAINT fk_conversation_trip FOREIGN KEY (trip_destination_id) REFERENCES trip_destinations(trip_destination_id)
        ON DELETE SET NULL ON UPDATE CASCADE
);
-- =====================================
-- Conversation Participants Table
-- =====================================
CREATE TABLE IF NOT EXISTS conversation_participants (
    conversation_id INT NOT NULL,
    user_id INT NOT NULL,
    joined_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (conversation_id, user_id),
    CONSTRAINT fk_cp_conversation FOREIGN KEY (conversation_id) REFERENCES conversations(conversation_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_cp_user FOREIGN KEY (user_id) REFERENCES users(user_id)
        ON DELETE CASCADE ON UPDATE CASCADE
);
-- =====================================
-- Messages Table
-- =====================================
CREATE TABLE IF NOT EXISTS messages (
    message_id INT AUTO_INCREMENT PRIMARY KEY,
    sender_id INT,
    content VARCHAR(2000) NOT NULL CHECK (CHAR_LENGTH(TRIM(content)) > 0),
    sent_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    conversation_id INT NOT NULL,
    CONSTRAINT fk_message_sender FOREIGN KEY (sender_id) REFERENCES users(user_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_message_conversation FOREIGN KEY (conversation_id) REFERENCES conversations(conversation_id)
        ON DELETE CASCADE ON UPDATE CASCADE
);
-- =====================================
-- User Audits Table
-- Tracks changes to user details like name, email, password, birthdate, deletion status, and role
-- =====================================
CREATE TABLE IF NOT EXISTS user_audits (
    audit_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    action ENUM('created', 'updated', 'deleted') NOT NULL,
    field_changed VARCHAR(100) CHECK (CHAR_LENGTH(TRIM(field_changed)) > 0),
    old_value VARCHAR(255) CHECK (CHAR_LENGTH(TRIM(old_value)) > 0),
    new_value VARCHAR(255) CHECK (CHAR_LENGTH(TRIM(new_value)) > 0),
    changed_by INT, 
    timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_user_audit_user FOREIGN KEY (user_id) REFERENCES users(user_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_user_audit_changed_by FOREIGN KEY (changed_by) REFERENCES users(user_id)
        ON DELETE SET NULL ON UPDATE CASCADE
);
-- =====================================
-- Buddy Audits Table
-- Tracks buddy requests, acceptances, removals, and departures
-- =====================================
CREATE TABLE IF NOT EXISTS buddy_audits (
    audit_id INT AUTO_INCREMENT PRIMARY KEY,
    buddy_id INT NOT NULL,
    action ENUM('requested', 'accepted', 'rejected', 'removed', 'left') NOT NULL,
    reason VARCHAR(255) CHECK (CHAR_LENGTH(TRIM(reason)) > 0),
    changed_by INT,
    timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_buddy_audit_buddy FOREIGN KEY (buddy_id) REFERENCES buddies(buddy_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_buddy_audit_user FOREIGN KEY (changed_by) REFERENCES users(user_id)
        ON DELETE SET NULL ON UPDATE CASCADE
);
-- =====================================
-- Trip Audits Table
-- Tracks changes in trip details like dates, max buddies, and archival status
-- =====================================
CREATE TABLE IF NOT EXISTS trip_audits (
    audit_id INT AUTO_INCREMENT PRIMARY KEY,
    trip_id INT NOT NULL,
    action ENUM('created', 'updated', 'deleted') NOT NULL,
    field_changed VARCHAR(100) CHECK (CHAR_LENGTH(TRIM(field_changed)) > 0),
    old_value VARCHAR(255) CHECK (CHAR_LENGTH(TRIM(old_value)) > 0),
    new_value VARCHAR(255) CHECK (CHAR_LENGTH(TRIM(new_value)) > 0),
    changed_by INT,
    timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_trip_audit_trip FOREIGN KEY (trip_id) REFERENCES trips(trip_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_trip_audit_user FOREIGN KEY (changed_by) REFERENCES users(user_id)
        ON DELETE SET NULL ON UPDATE CASCADE
);
-- =====================================
-- Conversation Audits Table
-- Tracks creation and participant changes in conversations
-- =====================================
CREATE TABLE IF NOT EXISTS conversation_audits (
    audit_id INT AUTO_INCREMENT PRIMARY KEY,
    conversation_id INT NOT NULL,
    affected_user_id INT,
    action ENUM('created', 'user_added', 'user_removed') NOT NULL,
    changed_by INT,
    timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_convo_audit_convo FOREIGN KEY (conversation_id) REFERENCES conversations(conversation_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_convo_audit_affected FOREIGN KEY (affected_user_id) REFERENCES users(user_id)
        ON DELETE SET NULL ON UPDATE CASCADE,
    CONSTRAINT fk_convo_audit_changed FOREIGN KEY (changed_by) REFERENCES users(user_id)
        ON DELETE SET NULL ON UPDATE CASCADE
);
-- =====================================
-- System Event Logs Table
-- Tracks automated processes like monthly archiving
-- =====================================
CREATE TABLE IF NOT EXISTS system_event_logs (
    event_id INT AUTO_INCREMENT PRIMARY KEY,
    event_type VARCHAR(100) NOT NULL CHECK (CHAR_LENGTH(TRIM(event_type)) > 0),
    affected_id INT,
    timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    details VARCHAR(255)
);