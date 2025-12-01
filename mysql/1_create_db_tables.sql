-- =====================================
-- Create Database (emoji-safe defaults)
-- =====================================
CREATE DATABASE IF NOT EXISTS travel_buddy
  CHARACTER SET utf8mb4          -- Use utf8mb4 to support emojis and multilingual text
  COLLATE utf8mb4_unicode_ci;    -- Unicode collation for consistent sorting/comparison
USE travel_buddy;

DROP TABLE IF EXISTS message;
DROP TABLE IF EXISTS buddy_audit;
DROP TABLE IF EXISTS buddy;
DROP TABLE IF EXISTS conversation_audit;
DROP TABLE IF EXISTS conversation_participant;
DROP TABLE IF EXISTS conversation;
DROP TABLE IF EXISTS message;
DROP TABLE IF EXISTS trip_audit;
DROP TABLE IF EXISTS trip_destination;
DROP TABLE IF EXISTS destination;
DROP TABLE IF EXISTS trip;
DROP TABLE IF EXISTS user;
DROP TABLE IF EXISTS system_event_log;

-- =====================================
-- User Table
-- =====================================
CREATE TABLE IF NOT EXISTS user (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    birthdate DATE NOT NULL,
    is_deleted BOOLEAN DEFAULT FALSE NOT NULL,
    role ENUM('user', 'admin') DEFAULT 'user' NOT NULL
);

-- =====================================
-- Destination Table
-- =====================================
CREATE TABLE destination (
    destination_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    state VARCHAR(100),
    country VARCHAR(100) NOT NULL,
    longitude DECIMAL(10, 7),
    latitude DECIMAL(10, 7)
);

-- =====================================
-- Trip Table
-- =====================================
CREATE TABLE IF NOT EXISTS trip (
    trip_id INT AUTO_INCREMENT PRIMARY KEY,
    owner_id INT,
    trip_name VARCHAR(100),
    max_buddies INT CHECK (max_buddies >= 1),
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    description VARCHAR(255),
    is_archived BOOLEAN DEFAULT FALSE NOT NULL,
    CONSTRAINT fk_trip_owner FOREIGN KEY (owner_id) REFERENCES user(user_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT chk_trip_dates CHECK (end_date >= start_date)
);

-- =====================================
-- Trip destination Table
-- =====================================
CREATE TABLE IF NOT EXISTS trip_destination (
    trip_destination_id INT AUTO_INCREMENT PRIMARY KEY,
    destination_id INT NOT NULL,
    trip_id INT NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    sequence_number INT NOT NULL,
    description VARCHAR(255),
    is_archived BOOLEAN DEFAULT FALSE,
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
    is_active BOOLEAN DEFAULT TRUE,
    departure_reason VARCHAR(255) DEFAULT NULL,
    request_status ENUM('pending', 'accepted', 'rejected') NOT NULL DEFAULT 'pending',
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
    is_group BOOLEAN DEFAULT FALSE NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    is_archived BOOLEAN DEFAULT FALSE NOT NULL,
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
-- Message Table
-- =====================================
CREATE TABLE IF NOT EXISTS message (
    message_id INT AUTO_INCREMENT PRIMARY KEY,
    sender_id INT,
    content VARCHAR(2000) NOT NULL,
    sent_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    conversation_id INT NOT NULL,
    CONSTRAINT fk_message_sender FOREIGN KEY (sender_id) REFERENCES user(user_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_message_conversation FOREIGN KEY (conversation_id) REFERENCES conversation(conversation_id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- Buddy Audit Table
-- Tracks buddy requests, acceptances, removals, and departures
CREATE TABLE IF NOT EXISTS buddy_audit (
    audit_id INT AUTO_INCREMENT PRIMARY KEY,
    buddy_id INT NOT NULL,
    action ENUM('requested', 'accepted', 'rejected', 'removed', 'left') NOT NULL,
    reason VARCHAR(255),
    changed_by INT,
    timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_buddy_audit_buddy FOREIGN KEY (buddy_id) REFERENCES buddy(buddy_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_buddy_audit_user FOREIGN KEY (changed_by) REFERENCES user(user_id)
        ON DELETE SET NULL ON UPDATE CASCADE
);

-- Trip Audit Table
-- Tracks changes in trip details like dates, max buddies, and archival status
CREATE TABLE IF NOT EXISTS trip_audit (
    audit_id INT AUTO_INCREMENT PRIMARY KEY,
    trip_id INT NOT NULL,
    action ENUM('created', 'updated', 'deleted') NOT NULL,
    field_changed VARCHAR(100), -- e.g., 'start_date', 'end_date', 'max_buddies'
    old_value VARCHAR(255),
    new_value VARCHAR(255),
    changed_by INT, -- user who made the change
    timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_trip_audit_trip FOREIGN KEY (trip_id) REFERENCES trip(trip_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_trip_audit_user FOREIGN KEY (changed_by) REFERENCES user(user_id)
        ON DELETE SET NULL ON UPDATE CASCADE
);

-- Conversation Audit Table
-- Tracks creation and participant changes in conversations
CREATE TABLE IF NOT EXISTS conversation_audit (
    audit_id INT AUTO_INCREMENT PRIMARY KEY,
    conversation_id INT NOT NULL,
    affected_user_id INT, -- the user added or removed (if applicable)
    action ENUM('created', 'user_added', 'user_removed') NOT NULL,
    triggered_by INT, -- who initiated the action
    timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_convo_audit_convo FOREIGN KEY (conversation_id) REFERENCES conversation(conversation_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_convo_audit_affected FOREIGN KEY (affected_user_id) REFERENCES user(user_id)
        ON DELETE SET NULL ON UPDATE CASCADE,
    CONSTRAINT fk_convo_audit_triggered FOREIGN KEY (triggered_by) REFERENCES user(user_id)
        ON DELETE SET NULL ON UPDATE CASCADE
);

-- System Event Log Table
-- Tracks automated processes like monthly archiving
CREATE TABLE IF NOT EXISTS system_event_log (
    event_id INT AUTO_INCREMENT PRIMARY KEY,
    event_type VARCHAR(100) NOT NULL,
    affected_id INT,
    triggered_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    details VARCHAR(255)
);