-- =====================================
-- Seed data for travel_buddy conversations
-- This script assumes the schema from create_db_tables.sql
-- and data from travelbuddy_table_data_UPDATED.sql have been loaded.
-- =====================================
USE travel_buddy;

-- Disable foreign key checks temporarily to avoid dependency issues during inserts
-- SET FOREIGN_KEY_CHECKS = 0;

-- =====================================
-- 1. Private Conversations (No Trip Destination)
-- =====================================

-- Convo 1: Allison Hill (ID 1) and Cristian Santos (ID 2)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (1, NULL, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (1, 1), (1, 2);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(1, 'Hey Cristian, how are you?', '2024-05-10 10:00:00', 1),
(2, 'I''m doing well, thanks for asking! What''s up?', '2024-05-10 10:01:00', 1),
(1, 'Just wanted to say hi. Are you planning any new trips soon?', '2024-05-10 10:05:00', 1);

-- Convo 2: Gina Moore (ID 3) and Kim Dudley (ID 4)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (2, NULL, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (2, 3), (2, 4);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(4, 'Hi Gina, I saw your profile and we have similar interests!', '2024-05-11 11:30:00', 2),
(3, 'Oh, that''s great! What kind of places do you like to visit?', '2024-05-11 11:32:00', 2),
(4, 'Mostly historical sites and natural parks. Anywhere with a good hike!', '2024-05-11 11:35:00', 2);

-- Convo 3: Jennifer Wilson (ID 5) and Brian Miller (ID 6)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (3, NULL, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (3, 5), (3, 6);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(5, 'Did you have any luck with your last travel plans?', '2024-05-12 14:15:00', 3),
(6, 'Not yet, still finalizing things. But I found a good flight deal!', '2024-05-12 14:16:00', 3),
(5, 'Nice! Let me know if you need any recommendations.', '2024-05-12 14:20:00', 3);

-- Convo 4: Jessica Brown (ID 7) and Joe Davis (ID 8)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (4, NULL, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (4, 7), (4, 8);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(7, 'Just checking in. Hope you''re well.', '2024-05-13 09:00:00', 4),
(8, 'All good here, thanks. You too!', '2024-05-13 09:01:00', 4),
(7, 'Thinking of planning a solo trip. Have you ever done that?', '2024-05-13 09:05:00', 4);

-- Convo 5: Kevin Martinez (ID 9) and Olivia Garcia (ID 10)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (5, NULL, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (5, 9), (5, 10);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(9, 'Hey, long time no talk! What''s new?', '2024-05-14 16:40:00', 5),
(10, 'Not much, just back from a trip. It was fantastic!', '2024-05-14 16:41:00', 5),
(9, 'Where did you go? I''d love to hear about it.', '2024-05-14 16:45:00', 5);

-- =====================================
-- 2. Private Conversations (Trip-Specific)
-- =====================================

-- Convo 6: Trip to Bali (TD 125, Owner: ID 1, Buddy: ID 16)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (6, 125, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (6, 1), (6, 16);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(1, 'Hey, thanks for accepting the Bali trip request! Happy to have you on board.', '2024-05-15 11:00:00', 6),
(16, 'Awesome, thanks! I''m so excited. Have you looked at any specific activities yet?', '2024-05-15 11:02:00', 6),
(1, 'Not yet, but I''m thinking we should definitely do some hiking. Maybe visit a temple or two?', '2024-05-15 11:05:00', 6);

-- Convo 7: Trip to Paris (TD 126, Owner: ID 17, Buddy: ID 18)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (7, 126, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (7, 17), (7, 18);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(18, 'Hey! I''m so looking forward to Paris. I have a few recommendations for restaurants if you''re interested.', '2024-05-16 12:30:00', 7),
(17, 'That''s perfect, I''d love that! What''s your favorite?', '2024-05-16 12:32:00', 7),
(18, 'There''s this amazing little creperie near the Louvre. We should check it out.', '2024-05-16 12:35:00', 7);

-- Convo 8: Trip to Rio de Janeiro (TD 128, Owner: ID 10, Buddy: ID 11)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (8, 128, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (8, 10), (8, 11);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(10, 'Hey there! Have you started looking for flights to Rio yet?', '2024-05-17 10:00:00', 8),
(11, 'Not yet, I''ve been so busy. I''ll check tonight. What are your dates again?', '2024-05-17 10:01:00', 8),
(10, 'May 20-27. Let me know what you find!', '2024-05-17 10:05:00', 8);

-- Convo 9: Trip to Santorini (TD 129, Owner: ID 11, Buddy: ID 12)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (9, 129, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (9, 11), (9, 12);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(12, 'Hi! I''m so happy to be joining the Santorini trip. The pictures are amazing!', '2024-05-18 15:00:00', 9),
(11, 'I know, right? It''s going to be incredible. We have to make sure we catch a sunset from Oia.', '2024-05-18 15:02:00', 9),
(12, 'Totally agree! I''ve heard the views are breathtaking.', '2024-05-18 15:05:00', 9);

-- Convo 10: Trip to Tokyo (TD 130, Owner: ID 12, Buddy: ID 13)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (10, 130, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (10, 12), (10, 13);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(13, 'Hey, looking forward to Tokyo! Have you decided on accommodation yet?', '2024-05-19 09:30:00', 10),
(12, 'Not yet. I was thinking we could look for an Airbnb. What do you think?', '2024-05-19 09:32:00', 10),
(13, 'Sounds good to me. I''m flexible.', '2024-05-19 09:35:00', 10);

-- =====================================
-- 3. Group Conversations (Trip-Specific)
-- =====================================

-- Convo 11: Trip to Rome (TD 127, Owner: ID 18, Buddies: ID 19, 20)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (11, 127, TRUE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (11, 18), (11, 19), (11, 20);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(18, 'Welcome everyone to the Rome trip chat! So excited to explore with you all.', '2024-05-20 10:00:00', 11),
(19, 'Hey guys! Looking forward to it.', '2024-05-20 10:01:00', 11),
(20, 'Hi all! I''ve already started a list of all the ancient sites we have to see.', '2024-05-20 10:02:00', 11),
(18, 'Perfect, let''s start a document with that list!', '2024-05-20 10:05:00', 11);

-- Convo 12: Trip to Vancouver (TD 133, Owner: ID 15, Buddies: ID 21, 22)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (12, 133, TRUE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (12, 15), (12, 21), (12, 22);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(15, 'Hey team, welcome to the Vancouver trip chat! We can coordinate plans here.', '2024-05-21 11:00:00', 12),
(21, 'Sounds good. I''m definitely up for some hiking and maybe some whale watching.', '2024-05-21 11:02:00', 12),
(22, 'Count me in for the whale watching! I also found a few good breweries we could visit.', '2024-05-21 11:04:00', 12);

-- Convo 13: Trip to New York City (TD 134, Owner: ID 16, Buddies: ID 23, 24)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (13, 134, TRUE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (13, 16), (13, 23), (13, 24);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(16, 'Alright everyone, the NYC trip is a go! Let''s start planning some spots to visit.', '2024-05-22 13:00:00', 13),
(23, 'Yes! First stop: Central Park!', '2024-05-22 13:01:00', 13),
(24, 'I second Central Park! And maybe the MET afterwards?', '2024-05-22 13:03:00', 13),
(16, 'Sounds like a great first day plan to me!', '2024-05-22 13:05:00', 13);

-- Convo 14: Trip to Bali (TD 125, Owner: ID 1, Buddy: ID 16) - Group chat of two
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (14, 125, TRUE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (14, 1), (14, 16);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(1, 'Hey, just wanted to make this the official group chat for our Bali trip!', '2024-05-23 09:00:00', 14),
(16, 'Sounds good! Much easier than private messages.', '2024-05-23 09:01:00', 14),
(1, 'Agreed. Let''s get planning!', '2024-05-23 09:02:00', 14);

-- Convo 15: Trip to Paris (TD 126, Owner: ID 17, Buddy: ID 18) - Group chat of two
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (15, 126, TRUE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (15, 17), (15, 18);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(17, 'Welcome to the Paris group chat!', '2024-05-24 14:00:00', 15),
(18, 'Excited to be here! My list of cafes is ready.', '2024-05-24 14:02:00', 15),
(17, 'Perfect! Can''t wait to see them.', '2024-05-24 14:03:00', 15);

-- Re-enable foreign key checks
-- SET FOREIGN_KEY_CHECKS = 1;
