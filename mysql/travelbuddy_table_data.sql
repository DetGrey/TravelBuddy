-- Seed data for travel_buddy
-- Assumes schema from create_db_tables.sql has been created.
USE travel_buddy;
-- =======================
-- user rows
-- =======================
INSERT INTO user (user_id, name, email, password_hash, birthdate) VALUES
(1, 'Allison Hill', 'allyhill95@gmail.com', 'af738b28e0a93deae4483d186d93ab0ae1300f80dbd1bf73b8b367a58cb3cb4b', '1995-09-05'),
(2, 'Cristian Santos', 'santos.cris65@yahoo.com', 'f3293232dc4f6f82f27fcb1f5bdf4517cd40e79a4f79ddd795c7d117d36557f3', '1965-07-29'),
(3, 'Gina Moore', 'gmoore1957@hotmail.com', '33e3f996588677ce376555d9705c6a4e0783924ef7b0f92b335fa735c429b4a7', '1957-08-31'),
(4, 'Kimberly Dudley', 'kim.dudley@outlook.com', '40f162b663df0eb38e907f235ea9e432809a00d97a969444ef4cc84eb3e16927', '1979-04-27'),
(5, 'Michele Williams', 'michelew78@gmail.com', 'd4b196b5cd9f19ce309caa8db9b07a56eec5076a3728092d52d18e7b58de0d78', '1978-11-20'),
(6, 'Derek Zuniga', 'derekzuniga76@protonmail.com', '06b21c08138098ffadc9fcfa075f38a7e9e4372e0d3cb8674dc8a32d352ace05', '1976-10-11'),
(7, 'Patty Perez', 'pattyperez98@gmail.com', '8bab06b75ea1b6d9d5d6ebb813d2ee54e55dee4cc38f4f7a33c32438ad4086f9', '1998-06-16'),
(8, 'Derek Wright', 'wright.derek64@yahoo.com', '528a0ba6937407022a0b6ab2b585d67c2d34910b599f40a76b083229b39904be', '1964-02-15'),
(9, 'Ann Williams', 'annw_1990@wmail.org', '561f847647774cc1bf4a600d5f412a15ecb011a5ea830076e96a78acada469e1', '1990-09-19'),
(10, 'Juan Dunlap', 'juandunlap06@fastmail.com', '869f59ba80f7ddebc5e783f2031b0921e505a7810ee71c9d27ac63b46eda8227', '2006-07-24'),
(11, 'Thomas Harrell', 'tommyh90@harrellmail.org', 'beae37ac789535f7cee792493bc81f78c9400a22f73183e6c8e5cca9ce50cfdd', '1990-10-20'),
(12, 'Linda Burns', 'lindaburns87@gmail.com', '0dd2b3752d462dc54fd19c5d455f191db04c757a7abf0401213f49600ec0a8d1', '1987-10-13'),
(13, 'Vanessa Patel', 'vanessa.patel@breezemail.net', 'ee812f64f9dfc12ca46bb74d063aeefa350a914638c212e8580f0e96f0a403d7', '1991-05-30'),
(14, 'Melissa Marquez', 'melmarquez59@gmail.com', '0df9731ff6cf9f9e8f6ef50977663da4e1346d9c019d8f3ad75ef9d8c8ceb5b0', '1959-01-25'),
(15, 'Kevin Hall', 'kevinhall70@outlook.com', '3436965836d671702082f5d5b210d3c0613393dd22099438136f964356534c05', '1970-11-27'),
(16, 'John Ryan', 'johnryan99@gmail.com', 'd1535114f0f71fea4592fbab95dcd6c0759257684b89e0c8bc80859454312ad3', '1999-07-22'),
(17, 'Jeff Owens', 'jeff.owens83@live.com', '20a872b8c9b0db8dce86954f4d9357d44e1910fba621feeaf2544f39dfe8a25c', '1983-06-02'),
(18, 'Joel Baxter', 'joelbaxter05@wmail.org', 'c7851c0f1530640d469066362549c79ff9663512e68519975f1389a37198061e', '2005-12-27'),
(19, 'Carol Tucker', 'caroltucker02@protonmail.com', '7b308ffa1ed446a0ed87fe1a06770e0a1c8231fca309d82603740e5e551c63a1', '2002-02-10'),
(20, 'Whitney Peters', 'whitneyp91@gmail.com', '3ef7ffcd0f61dfe0952cff46d4213565733ab109e96129b272a6c3cc7a355f4a', '1991-08-10'),
(21, 'Erik Williams', 'erikw59@yahoo.com', '1f08483864cf95da973af5c9f9050fa2b7bb0c15d730dcfb6d070d8e5e563214', '1959-05-03'),
(22, 'Cynthia Wilson', 'cynthiawilson81@outlook.com', '73ed167b935c625b233b952c37a7806e36be9a6a96f6048455436b3953143033', '1981-08-23'),
(23, 'Matthew Bryant', 'mattb78@gmail.com', '8b124a42d3eeb70314712b3e1cd4bb408059dce0dc3e35c34b552b69d04e9512', '1978-06-17'),
(24, 'Sherry Wood', 'sherrywood92@archernet.io', '18b3cfa4df9d496a842ad93ed7bf7e95d659ef5915efd4d51626e1aad5f9e5f3', '1992-06-13'),
(25, 'Matthew Mcmillan', 'mattmcmillan68@fastmail.com', 'ce9a588574674e0e04b212eeeeb95d0f7af041f87c85c1117057f3ab506317e8', '1968-07-25'),
(26, 'Sarah Wagner', 'sarahwagner77@yahoo.com', '5941a74f3096328368f9fb45e014298c1d8858b034b2488831d5a3a08057b149', '1977-05-29'),
(27, 'Mandy Green', 'mandyg01@gmail.com', '7d4ac77986d3a2c66a605ea641d231e8cfc3edca864dcdf0e4521ae7eedf19af', '2001-09-25'),
(28, 'Kimberly Davenport', 'kimdavenport69@outlook.com', '9eb243cb4f34c9b60fe0bab9ce4c7c23cf3627ddfccf28cb0cacaa601fb16003', '1969-01-02'),
(29, 'Mary Gomez', 'marygomez66@gmail.com', 'e84303888d739510bf26246c32123cbb12854ef01511d54360615518a1526720', '1966-10-04'),
(30, 'Allison Doyle', 'allydoyle02@doyledesigns.net', '9f945d9f0b5f0d334f915932aef318a81eca9828a238189e1d62a5b9b2e95380', '2002-06-07');

-- =======================
-- destination rows
-- =======================
INSERT INTO destination (destination_id, name, address, zip_code, city, state, country, longitude, latitude) VALUES
(1, 'Eiffel Tower', 'Champ de Mars', '75007', 'Paris', '', 'France', NULL, NULL),
(2, 'Statue of Liberty', 'Liberty Island', '10004', 'New York', 'NY', 'USA', NULL, NULL),
(3, 'Great Wall', 'Huairou', '', 'Beijing', '', 'China', NULL, NULL),
(4, 'Sydney Opera House', 'Bennelong Point', '2000', 'Sydney', 'NSW', 'Australia', NULL, NULL),
(5, 'Christ the Redeemer', 'Parque Nacional da Tijuca', '22241-125', 'Rio de Janeiro', 'RJ', 'Brazil', NULL, NULL),
(6, 'Machu Picchu', '', '', 'Cusco Region', '', 'Peru', NULL, NULL),
(7, 'Pyramids of Giza', '', '', 'Giza', '', 'Egypt', NULL, NULL),
(8, 'Mount Fuji', '', '403-0005', 'Fujiyoshida', 'Yamanashi', 'Japan', NULL, NULL),
(9, 'Santorini', '', '84700', 'Santorini', 'Cyclades', 'Greece', NULL, NULL),
(10, 'Banff National Park', '', 'T1L', 'Banff', 'Alberta', 'Canada', NULL, NULL),
(11, 'Table Mountain', '', '8001', 'Cape Town', 'Western Cape', 'South Africa', NULL, NULL),
(12, 'Taj Mahal', '', '282001', 'Agra', 'Uttar Pradesh', 'India', NULL, NULL),
(13, 'Angkor Wat', '', '17252', 'Siem Reap', '', 'Cambodia', NULL, NULL),
(14, 'Uluru', '', '0872', 'Uluru', 'NT', 'Australia', NULL, NULL),
(15, 'Petra', '', '', 'Wadi Musa', '', 'Jordan', NULL, NULL),
(16, 'Grand Canyon', '', '86023', 'Grand Canyon', 'AZ', 'USA', NULL, NULL),
(17, 'Iguazu Falls', '', '', 'Foz do Iguaçu', 'PR', 'Brazil', NULL, NULL),
(18, 'Salar de Uyuni', '', '', 'Uyuni', 'Potosí', 'Bolivia', NULL, NULL),
(19, 'Hallstatt', '', '4830', 'Hallstatt', 'Upper Austria', 'Austria', NULL, NULL),
(20, 'Cinque Terre', '', '19017', 'Riomaggiore', 'Liguria', 'Italy', NULL, NULL),
(21, 'Reykjavík Blue Lagoon', '', '240', 'Grindavík', '', 'Iceland', NULL, NULL),
(22, 'Bali Ubud', '', '80571', 'Ubud', 'Bali', 'Indonesia', NULL, NULL),
(23, 'Phuket Old Town', '', '83000', 'Phuket', '', 'Thailand', NULL, NULL),
(24, 'Queenstown', '', '9300', 'Queenstown', 'Otago', 'New Zealand', NULL, NULL),
(25, 'Zermatt', '', '3920', 'Zermatt', 'Valais', 'Switzerland', NULL, NULL),
(26, 'Dubai Burj Khalifa', '1 Sheikh Mohammed bin Rashid Blvd', '00000', 'Dubai', '', 'UAE', NULL, NULL),
(27, 'Hanoi Old Quarter', '', '100000', 'Hanoi', '', 'Vietnam', NULL, NULL),
(28, 'Lisbon Alfama', '', '1100-585', 'Lisbon', '', 'Portugal', NULL, NULL),
(29, 'Louvre Museum', 'Rue de Rivoli', '75001', 'Paris', '', 'France', NULL, NULL),
(30, 'Mont Saint-Michel', '', '50170', 'Normandy', '', 'France', NULL, NULL),
(31, 'Versailles Palace', '', '78000', 'Versailles', '', 'France', NULL, NULL),
(32, 'Times Square', '', '10036', 'New York', 'NY', 'USA', NULL, NULL),
(33, 'Yosemite National Park', '', '95389', 'Yosemite Valley', 'CA', 'USA', NULL, NULL),
(34, 'Golden Gate Bridge', '', '94129', 'San Francisco', 'CA', 'USA', NULL, NULL),
(35, 'Kyoto Fushimi Inari Shrine', '', '612-0882', 'Kyoto', 'Kyoto', 'Japan', NULL, NULL),
(36, 'Tokyo Shibuya Crossing', '', '150-0042', 'Tokyo', 'Tokyo', 'Japan', NULL, NULL),
(37, 'Nara Deer Park', '', '630-8211', 'Nara', 'Nara', 'Japan', NULL, NULL),
(38, 'Bondi Beach', '', '2026', 'Sydney', 'NSW', 'Australia', NULL, NULL),
(39, 'Great Barrier Reef', '', '4870', 'Cairns', 'QLD', 'Australia', NULL, NULL),
(40, 'Blue Mountains', '', '2780', 'Katoomba', 'NSW', 'Australia', NULL, NULL),
(41, 'Sugarloaf Mountain', '', '22290-270', 'Rio de Janeiro', 'RJ', 'Brazil', NULL, NULL),
(42, 'Pelourinho Historic Center', '', '40026-280', 'Salvador', 'BA', 'Brazil', NULL, NULL),
(43, 'Amazon Rainforest Gateway', '', '69000-000', 'Manaus', 'AM', 'Brazil', NULL, NULL),
(44, 'Florence Duomo', '', '50122', 'Florence', 'Tuscany', 'Italy', NULL, NULL),
(45, 'Venice Grand Canal', '', '30100', 'Venice', 'Veneto', 'Italy', NULL, NULL),
(46, 'Colosseum', '', '00184', 'Rome', 'Lazio', 'Italy', NULL, NULL),
(47, 'Jaipur Amber Fort', '', '302001', 'Jaipur', 'Rajasthan', 'India', NULL, NULL),
(48, 'Kerala Backwaters', '', '688001', 'Alleppey', 'Kerala', 'India', NULL, NULL),
(49, 'Varanasi Ghats', '', '221001', 'Varanasi', 'Uttar Pradesh', 'India', NULL, NULL),
(50, 'Mykonos Windmills', '', '84600', 'Mykonos', 'Cyclades', 'Greece', NULL, NULL),
(51, 'Delphi Archaeological Site', '', '33054', 'Delphi', 'Phocis', 'Greece', NULL, NULL),
(52, 'Niagara Falls', '', 'L2G', 'Niagara Falls', 'Ontario', 'Canada', NULL, NULL),
(53, 'Whistler Blackcomb', '', 'V0N', 'Whistler', 'British Columbia', 'Canada', NULL, NULL),
(54, 'Cape Point', '', '7975', 'Cape Town', 'Western Cape', 'South Africa', NULL, NULL),
(55, 'Robben Island', '', '7400', 'Cape Town', 'Western Cape', 'South Africa', NULL, NULL);

-- =======================
-- trip rows
-- =======================
INSERT INTO trip (trip_id, owner_id, max_buddies, start_date, end_date, description) VALUES
(1, 1, 1, '2025-06-17', '2025-06-25', 'Exploring ancient ruins and hidden cafés—solo but open to serendipity.'),
(2, 1, 4, '2026-08-18', '2026-08-30', 'Budget backpacking across mountain villages—let’s split snacks and stories.'),
(3, 1, 9, '2025-03-02', '2025-03-10', 'Seeking thrill-seekers for canyon dives and midnight bonfires.'),
(4, 2, 1, '2027-01-02', '2027-01-20', 'Family road trip with room for one more—must love board games.'),
(5, 2, 4, '2025-12-12', '2025-12-19', 'Romantic escape with room for fellow sunset chasers.'),
(6, 2, 9, '2025-11-07', '2025-11-12', 'Spontaneous couple’s getaway—open to group hikes and wine tastings.'),
(7, 3, 4, '2026-12-17', '2027-01-04', 'Forest trails, campfire chats, and zero Wi-Fi—nature lovers welcome.'),
(8, 3, 1, '2026-06-28', '2026-07-11', 'Solo trek through coastal towns—sharing costs and laughs encouraged.'),
(9, 3, 6, '2026-04-20', '2026-05-08', 'Art museums by day, jazz bars by night—culture crew wanted.'),
(10, 4, 6, '2025-12-20', '2025-12-31', 'Solo traveler chasing northern lights—bring your camera and cocoa.'),
(11, 5, 2, '2025-12-13', '2025-12-30', 'Street food crawl across three cities—forks optional, fun mandatory.'),
(12, 6, 1, '2025-02-06', '2025-02-19', 'Mountain meditation and waterfall hikes—peaceful souls preferred.'),
(13, 6, 7, '2027-10-23', '2027-10-31', 'Solo wanderer seeking spontaneous companions for city-hopping.'),
(14, 7, 10, '2025-10-28', '2025-11-11', 'Gastronomic adventure through spice markets and rooftop eateries.'),
(15, 7, 2, '2026-09-11', '2026-09-22', 'Family of five heading south—room for a storyteller or two.'),
(16, 7, 5, '2026-05-25', '2026-06-06', 'Solo journey through desert landscapes—sunrise hikes and stargazing.'),
(17, 8, 7, '2026-03-09', '2026-03-17', 'Friends reuniting for a cultural deep dive—open to new faces.'),
(18, 9, 3, '2025-04-18', '2025-05-04', 'Food tour with a twist—local chefs, secret recipes, and spice.'),
(19, 9, 4, '2026-03-10', '2026-03-26', 'Cultural immersion trip—temples, textiles, and tea ceremonies.'),
(20, 10, 10, '2026-06-30', '2026-07-07', 'Backpacking through waterfalls and hostels—let’s keep it light.'),
(21, 10, 3, '2027-09-24', '2027-10-06', 'Nature retreat with yoga mornings and trail mix afternoons.'),
(22, 10, 5, '2027-03-03', '2027-03-20', 'Digital nomads unite—co-working by day, exploring by night.'),
(23, 11, 1, '2026-09-13', '2026-09-28', 'Couple’s escape with room for a fellow dreamer.'),
(24, 12, 7, '2027-11-22', '2027-12-07', 'Cultural circuit with museum marathons and local dance nights.'),
(25, 13, 10, '2027-04-13', '2027-04-24', 'Foodie fiesta—tacos, tagines, and taste-testing galore.'),
(26, 14, 7, '2026-05-30', '2026-06-19', 'Trailblazing through national parks—boots, bugs, and bliss.'),
(27, 15, 3, '2025-02-12', '2025-02-25', 'Romantic road trip with space for a third wheel (with good vibes).'),
(28, 16, 10, '2025-06-29', '2025-07-12', 'Photography expedition—golden hours and moody skies guaranteed.'),
(29, 16, 6, '2026-02-10', '2026-02-27', 'Couple’s retreat with open itinerary—join us for the unexpected.'),
(30, 16, 9, '2025-10-05', '2025-10-14', 'Nature escape with forest bathing and hammock naps.'),
(31, 17, 2, '2027-06-21', '2027-06-27', 'Backpacking duo seeking budget-savvy explorers.'),
(32, 18, 7, '2026-02-17', '2026-02-27', 'First-time traveler—open to tips, tricks, and travel buddies.'),
(33, 18, 7, '2025-03-01', '2025-03-08', 'Photo safari—sunrises, silhouettes, and shutter clicks.'),
(34, 18, 9, '2025-06-18', '2025-07-07', 'Cultural caravan—markets, music, and midnight strolls.'),
(35, 19, 2, '2027-07-16', '2027-07-21', 'Remote work week in paradise—Wi-Fi and waves included.'),
(36, 19, 6, '2026-11-07', '2026-11-20', 'Solo explorer seeking kindred spirits for shared adventures.'),
(37, 19, 7, '2026-06-22', '2026-07-06', 'Budget backpacking with beach bonfires and hostel hangs.'),
(38, 20, 5, '2026-05-03', '2026-05-08', 'Work-from-anywhere crew—bring your laptop and wanderlust.'),
(39, 20, 9, '2026-11-09', '2026-11-19', 'Solo mission to find the best view and worst coffee.'),
(40, 21, 9, '2027-07-31', '2027-08-14', 'First-time abroad—open to guidance and good company.'),
(41, 21, 3, '2027-03-06', '2027-03-17', 'Culinary crawl through three countries—forks up!'),
(42, 21, 9, '2025-06-29', '2025-07-09', 'Remote work retreat—cozy cafés and quiet corners.'),
(43, 22, 8, '2026-08-22', '2026-09-06', 'Family adventure with space for storytellers and snack-sharers.'),
(44, 23, 5, '2027-06-15', '2027-07-01', 'Couple’s journey—open to fellow romantics and wanderers.'),
(45, 24, 10, '2025-10-23', '2025-11-04', 'Solo trek with a twist—join for the laughs, stay for the views.'),
(46, 25, 2, '2026-01-31', '2026-02-20', 'Work and wander—co-working by day, wine by night.'),
(47, 26, 8, '2025-03-10', '2025-03-19', 'Remote work escape—bring your best playlists and productivity.'),
(48, 27, 9, '2026-11-09', '2026-11-22', 'First-time traveler—curious, cautious, and caffeinated.'),
(49, 28, 9, '2025-01-03', '2025-01-14', 'Couple’s trip with room for a third adventurer.'),
(50, 28, 7, '2027-03-19', '2027-04-02', 'Foodie tour through hidden gems—expect spice, stories, and surprises.'),
(51, 29, 2, '2027-04-17', '2027-05-06', 'Couple’s escape with flexible plans—open to spontaneous detours.'),
(52, 29, 2, '2027-12-03', '2027-12-15', 'Winter wanderlust—hot drinks, cold air, and cozy company.'),
(53, 30, 10, '2027-11-08', '2027-11-20', 'Cultural deep dive with local guides and off-the-map adventures.');

-- =======================
-- itinerary rows
-- =======================
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (1, 3, 1, '2025-06-17', '2025-06-19', 1, 'Learn about the region''s economy and business culture.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (2, 23, 1, '2025-06-20', '2025-06-23', 2, 'Join public events and explore cultural heritage.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (3, 2, 2, '2026-08-18', '2026-08-22', 1, 'Discover the academic charm and historic landmarks of the city.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (4, 28, 2, '2026-08-23', '2026-08-25', 2, 'Enjoy a nature-filled escape with wildlife and scenic trails.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (5, 8, 3, '2025-03-02', '2025-03-05', 1, 'Experience the local cuisine and vibrant street life.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (6, 9, 3, '2025-03-06', '2025-03-10', 2, 'Dive into the tech scene and explore modern innovations.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (7, 24, 4, '2027-01-02', '2027-01-08', 1, 'Immerse yourself in music, art, and cultural experiences.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (8, 19, 4, '2027-01-09', '2027-01-14', 2, 'Take time to reflect and connect with the local traditions.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (9, 26, 5, '2025-12-12', '2025-12-17', 1, 'Set personal goals and enjoy peaceful surroundings.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (10, 16, 5, '2025-12-18', '2025-12-19', 2, 'Attend a major event and explore the local highlights.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (11, 4, 6, '2025-11-07', '2025-11-12', 1, 'Learn about the region''s economy and business culture.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (12, 14, 7, '2026-12-17', '2026-12-19', 1, 'Visit museums and explore the artistic side of the city.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (13, 15, 7, '2026-12-20', '2026-12-22', 2, 'Participate in local workshops and cultural exchanges.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (14, 28, 7, '2026-12-23', '2026-12-25', 3, 'Tour historical sites and enjoy traditional performances.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (15, 24, 8, '2026-06-28', '2026-06-30', 1, 'Stroll through charming neighborhoods and local shops.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (16, 11, 8, '2026-07-01', '2026-07-04', 2, 'Engage in meaningful conversations and cultural learning.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (17, 26, 8, '2026-07-05', '2026-07-08', 3, 'Explore spiritual landmarks and peaceful retreats.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (18, 18, 9, '2026-04-20', '2026-04-23', 1, 'Attend leadership seminars and networking events.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (19, 15, 9, '2026-04-24', '2026-04-29', 2, 'Join in local festivals and community celebrations.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (20, 9, 10, '2025-12-20', '2025-12-23', 1, 'Visit family and reconnect with your roots.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (21, 15, 10, '2025-12-24', '2025-12-26', 2, 'Watch films and explore the local creative scene.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (22, 26, 11, '2025-12-13', '2025-12-15', 1, 'Learn about local strategies and business practices.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (23, 28, 11, '2025-12-16', '2025-12-18', 2, 'Take a break and enjoy the calm of a small town.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (24, 18, 11, '2025-12-19', '2025-12-25', 3, 'Capture scenic views and enjoy outdoor activities.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (25, 3, 12, '2025-02-06', '2025-02-09', 1, 'Read, relax, and enjoy peaceful surroundings.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (26, 25, 12, '2025-02-10', '2025-02-13', 2, 'Engage in scientific discussions and community projects.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (27, 16, 13, '2027-10-23', '2027-10-28', 1, 'Meet local authors and attend literary events.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (28, 7, 13, '2027-10-29', '2027-10-31', 2, 'Watch performances and explore the film culture.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (29, 13, 14, '2025-10-28', '2025-11-02', 1, 'Understand the local economy and social structure.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (30, 1, 14, '2025-11-03', '2025-11-07', 2, 'Join public events and explore cultural heritage.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (32, 14, 15, '2026-09-18', '2026-09-22', 2, 'Attend a professional development workshop and explore local education centers.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (33, 7, 16, '2026-05-25', '2026-05-28', 1, 'Enjoy a cultural performance and explore the local art scene.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (34, 10, 16, '2026-05-29', '2026-05-31', 2, 'Visit financial districts and learn about economic growth.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (35, 24, 17, '2026-03-09', '2026-03-11', 1, 'Participate in a leadership summit and networking event.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (36, 18, 17, '2026-03-12', '2026-03-14', 2, 'Explore environmental initiatives and sustainability projects.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (37, 2, 17, '2026-03-15', '2026-03-17', 3, 'Attend a regional policy forum and cultural exchange.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (38, 17, 18, '2025-04-18', '2025-04-20', 1, 'Visit historical landmarks and learn about local heritage.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (39, 28, 18, '2025-04-21', '2025-04-27', 2, 'Explore the western countryside and enjoy scenic drives.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (40, 6, 18, '2025-04-28', '2025-04-30', 3, 'Tour local farms and experience sustainable living.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (41, 3, 19, '2026-03-10', '2026-03-12', 1, 'Sample local cuisine and enjoy a culinary tour.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (42, 20, 19, '2026-03-13', '2026-03-16', 2, 'Explore botanical gardens and nature trails.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (43, 4, 20, '2026-06-30', '2026-07-06', 1, 'Attend a national conference and explore the capital.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (44, 2, 21, '2027-09-24', '2027-09-30', 1, 'Join a cultural immersion program and meet local families.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (45, 20, 21, '2027-10-01', '2027-10-06', 2, 'Participate in a strategic planning retreat.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (46, 11, 22, '2027-03-03', '2027-03-07', 1, 'Attend a policy roundtable and meet local officials.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (47, 9, 22, '2027-03-08', '2027-03-11', 2, 'Join a business delegation and explore trade opportunities.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (48, 7, 22, '2027-03-12', '2027-03-16', 3, 'Explore urban development and infrastructure projects.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (49, 22, 22, '2027-03-17', '2027-03-20', 4, 'Meet with legal professionals and attend a public forum.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (50, 22, 23, '2026-09-13', '2026-09-17', 1, 'Join a travel writing workshop and explore coastal towns.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (51, 21, 23, '2026-09-18', '2026-09-23', 2, 'Attend an industry expo and innovation showcase.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (52, 25, 24, '2027-11-22', '2027-11-27', 1, 'Explore public art installations and meet local artists.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (53, 3, 24, '2027-11-28', '2027-12-04', 2, 'Participate in a civic engagement seminar.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (54, 1, 24, '2027-12-05', '2027-12-07', 3, 'Join a music festival and enjoy live performances.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (55, 3, 25, '2027-04-13', '2027-04-16', 1, 'Attend a corporate retreat and team-building event.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (56, 18, 25, '2027-04-17', '2027-04-23', 2, 'Join a road safety awareness campaign.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (57, 5, 26, '2026-05-30', '2026-06-02', 1, 'Participate in a community mural project.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (58, 12, 26, '2026-06-03', '2026-06-07', 2, 'Attend a political science lecture series.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (59, 3, 26, '2026-06-08', '2026-06-12', 3, 'Explore government programs and public services.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (60, 15, 27, '2025-02-12', '2025-02-18', 1, 'Join a defense policy roundtable and security briefing.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (61, 27, 27, '2025-02-19', '2025-02-23', 2, 'Tour investment hubs and meet with entrepreneurs.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (62, 26, 28, '2025-06-29', '2025-07-05', 1, 'Explore local newsrooms and learn about media production.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (63, 21, 28, '2025-07-06', '2025-07-10', 2, 'Attend a political fundraiser and explore civic engagement.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (64, 17, 28, '2025-07-11', '2025-07-12', 3, 'Visit historic churches and enjoy local cuisine.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (65, 9, 29, '2026-02-10', '2026-02-12', 1, 'Join a financial literacy seminar and community roundtable.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (66, 4, 29, '2026-02-13', '2026-02-19', 2, 'Tour architectural landmarks and regional museums.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (67, 9, 30, '2025-10-05', '2025-10-11', 1, 'Explore government buildings and policy centers.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (68, 10, 30, '2025-10-12', '2025-10-14', 2, 'Visit a local business hub and meet entrepreneurs.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (69, 11, 31, '2027-06-21', '2027-06-25', 1, 'Tour historic neighborhoods and cultural centers.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (70, 7, 31, '2027-06-26', '2027-06-27', 2, 'Participate in a community service project.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (71, 9, 32, '2026-02-17', '2026-02-19', 1, 'Attend a healthcare innovation summit.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (72, 28, 32, '2026-02-20', '2026-02-25', 2, 'Join a wellness retreat and mindfulness workshop.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (73, 2, 32, '2026-02-26', '2026-02-27', 3, 'Meet local artists and attend a gallery opening.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (74, 1, 33, '2025-03-01', '2025-03-04', 1, 'Explore academic institutions and research centers.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (75, 11, 33, '2025-03-05', '2025-03-08', 2, 'Attend a public health conference and city tour.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (76, 24, 34, '2025-06-18', '2025-06-24', 1, 'Visit nonprofit organizations and social enterprises.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (77, 15, 34, '2025-06-25', '2025-06-30', 2, 'Participate in a youth leadership program.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (78, 1, 35, '2027-07-16', '2027-07-19', 1, 'Join a storytelling workshop and cultural exchange.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (79, 4, 35, '2027-07-20', '2027-07-21', 2, 'Attend a creative writing retreat.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (80, 27, 36, '2026-11-07', '2026-11-13', 1, 'Explore local art studios and meet painters.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (81, 12, 36, '2026-11-14', '2026-11-20', 2, 'Join a social justice panel and networking event.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (82, 14, 37, '2026-06-22', '2026-06-24', 1, 'Participate in a nature conservation project.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (83, 5, 37, '2026-06-25', '2026-06-29', 2, 'Attend a film screening and director Q&A.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (84, 26, 38, '2026-05-03', '2026-05-07', 1, 'Join a medical ethics seminar and hospital tour.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (85, 22, 39, '2026-11-09', '2026-11-11', 1, 'Explore public health campaigns and wellness fairs.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (86, 8, 39, '2026-11-12', '2026-11-16', 2, 'Visit a local market and culinary school.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (87, 28, 40, '2027-07-31', '2027-08-03', 1, 'Attend an education policy forum.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (88, 14, 40, '2027-08-04', '2027-08-07', 2, 'Join a media literacy workshop.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (89, 20, 40, '2027-08-08', '2027-08-11', 3, 'Participate in a songwriting retreat.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (90, 24, 40, '2027-08-12', '2027-08-14', 4, 'Attend a national security roundtable.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (91, 1, 41, '2027-03-06', '2027-03-10', 1, 'Join a peacebuilding seminar and cultural walk.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (92, 6, 41, '2027-03-11', '2027-03-16', 2, 'Explore fire safety programs and emergency services.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (93, 28, 42, '2025-06-29', '2025-07-03', 1, 'Participate in a disaster preparedness training.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (94, 24, 42, '2025-07-04', '2025-07-07', 2, 'Participate in a sustainability workshop and team-building retreat.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (95, 26, 42, '2025-07-08', '2025-07-09', 3, 'Join a career development seminar and networking event.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (96, 28, 43, '2026-08-22', '2026-08-25', 1, 'Attend a civic leadership summit and policy roundtable.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (97, 2, 43, '2026-08-26', '2026-08-29', 2, 'Explore campaign strategies and political engagement programs.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (98, 16, 43, '2026-08-30', '2026-09-04', 3, 'Participate in a public speaking and debate workshop.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (99, 10, 44, '2027-06-15', '2027-06-18', 1, 'Join an economic development forum and local business tour.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (100, 27, 44, '2027-06-19', '2027-06-22', 2, 'Attend a marketing and branding masterclass.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (101, 26, 44, '2027-06-23', '2027-06-25', 3, 'Explore peacebuilding initiatives and community outreach.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (102, 7, 45, '2025-10-23', '2025-10-25', 1, 'Join a family wellness retreat and parenting seminar.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (103, 13, 45, '2025-10-26', '2025-10-30', 2, 'Attend a veterans\' affairs panel and historical tour.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (104, 11, 45, '2025-10-31', '2025-11-04', 3, 'Participate in a conflict resolution and mediation workshop.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (105, 17, 46, '2026-01-31', '2026-02-06', 1, 'Join a motivational leadership bootcamp.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (106, 13, 46, '2026-02-07', '2026-02-11', 2, 'Attend a cybersecurity and digital safety seminar.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (107, 22, 46, '2026-02-12', '2026-02-14', 3, 'Participate in a health journalism and media ethics panel.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (108, 27, 46, '2026-02-15', '2026-02-17', 4, 'Join a global education and cultural diplomacy forum.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (109, 6, 47, '2025-03-10', '2025-03-12', 1, 'Attend a medical research symposium and innovation showcase.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (110, 19, 47, '2025-03-13', '2025-03-15', 2, 'Participate in a law enforcement and public safety workshop.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (111, 9, 47, '2025-03-16', '2025-03-19', 3, 'Join a youth empowerment and civic responsibility program.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (112, 12, 48, '2026-11-09', '2026-11-13', 1, 'Attend a data science and analytics bootcamp.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (113, 24, 48, '2026-11-14', '2026-11-19', 2, 'Participate in a mental health awareness and support retreat.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (114, 26, 48, '2026-11-20', '2026-11-22', 3, 'Join a public policy and governance roundtable.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (115, 4, 49, '2025-01-03', '2025-01-07', 1, 'Attend a winter wellness retreat and mindfulness workshop.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (116, 13, 49, '2025-01-08', '2025-01-10', 2, 'Join a science and innovation expo with expert panels.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (117, 19, 49, '2025-01-11', '2025-01-14', 3, 'Participate in a cultural heritage tour and local traditions showcase.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (118, 17, 50, '2027-03-19', '2027-03-25', 1, 'Attend an educational media conference and school visits.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (119, 26, 50, '2027-03-26', '2027-03-29', 2, 'Join a youth science fair and theory exploration lab.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (120, 14, 51, '2027-04-17', '2027-04-21', 1, 'Participate in a legal and political leadership seminar.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (121, 3, 51, '2027-04-22', '2027-04-28', 2, 'Explore academic institutions and attend guest lectures.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (122, 22, 51, '2027-04-29', '2027-05-03', 3, 'Join a productivity and time management bootcamp.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (123, 28, 52, '2027-12-03', '2027-12-09', 1, 'Attend an environmental sustainability summit.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (124, 4, 52, '2027-12-10', '2027-12-14', 2, 'Participate in a financial literacy and security workshop.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (125, 14, 53, '2027-11-08', '2027-11-12', 1, 'Join a public health and wellness awareness campaign.');
INSERT INTO itinerary (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES (126, 11, 53, '2027-11-13', '2027-11-19', 2, 'Attend a transportation and mobility innovation forum.');

-- =======================
-- buddy rows
-- =======================
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (1, 7, 1, 4, 'Vegetarian—open to food suggestions.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (2, 24, 1, 2, 'Happy to share accommodation.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (3, 10, 2, 3, 'Traveling with kids, hope that\'s okay.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (4, 15, 3, 4, 'Prefer early starts.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (5, 7, 3, 5, 'Prefer early starts.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (6, 6, 3, 1, 'Happy to share accommodation.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (7, 3, 4, 2, 'Happy to share accommodation.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (8, 26, 4, 2, 'Can bring extra gear if needed.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (9, 2, 4, 2, 'Prefer early starts.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (10, 15, 5, 4, 'Traveling with kids, hope that\'s okay.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (11, 16, 6, 4, 'Traveling with kids, hope that\'s okay.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (12, 21, 6, 1, 'Looking forward to it.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (13, 8, 6, 2, 'Prefer early starts.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (14, 18, 6, 2, 'Looking forward to it.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (15, 26, 7, 4, 'Flexible with dates and budget.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (16, 20, 7, 5, 'Vegetarian—open to food suggestions.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (17, 29, 8, 2, 'Prefer early starts.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (18, 9, 8, 2, 'Happy to share accommodation.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (19, 16, 8, 2, 'Happy to share accommodation.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (20, 3, 8, 3, 'Traveling with kids, hope that\'s okay.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (21, 11, 9, 5, 'Looking forward to it.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (22, 5, 9, 2, 'Vegetarian—open to food suggestions.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (23, 5, 9, 2, 'Looking forward to it.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (24, 11, 10, 5, 'Prefer early starts.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (25, 2, 10, 2, 'Vegetarian—open to food suggestions.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (26, 29, 10, 5, 'Excited to join!', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (27, 13, 10, 4, 'Excited to join!', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (28, 25, 11, 4, 'Vegetarian—open to food suggestions.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (29, 24, 11, 5, 'Traveling with kids, hope that\'s okay.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (30, 8, 11, 3, 'Vegetarian—open to food suggestions.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (31, 13, 12, 3, 'Vegetarian—open to food suggestions.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (32, 27, 13, 4, 'Can bring extra gear if needed.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (33, 18, 13, 1, 'Vegetarian—open to food suggestions.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (34, 3, 14, 4, 'Can bring extra gear if needed.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (35, 2, 15, 3, 'Vegetarian—open to food suggestions.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (36, 7, 15, 4, 'Flexible with dates and budget.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (37, 9, 16, 4, 'Happy to share accommodation.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (38, 16, 16, 1, 'Excited to join!', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (39, 8, 16, 1, 'Excited to join!', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (40, 8, 16, 2, 'Excited to join!', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (41, 8, 17, 2, 'Prefer early starts.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (42, 4, 17, 5, 'Traveling with kids, hope that\'s okay.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (43, 25, 18, 3, 'Can bring extra gear if needed.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (44, 20, 18, 1, 'Can bring extra gear if needed.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (45, 4, 18, 5, 'Excited to join!', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (46, 13, 19, 2, 'Looking forward to it.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (47, 23, 19, 2, 'Looking forward to it.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (48, 25, 19, 3, 'Looking forward to it.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (49, 26, 19, 1, 'Flexible with dates and budget.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (50, 22, 20, 3, 'Looking forward to it.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (51, 21, 20, 3, 'Excited to join!', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (52, 27, 20, 4, 'Looking forward to it.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (53, 12, 20, 4, 'Can bring extra gear if needed.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (54, 24, 21, 5, 'Happy to share accommodation.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (55, 26, 21, 5, 'Prefer early starts.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (56, 27, 22, 5, 'Happy to share accommodation.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (57, 28, 22, 2, 'Looking forward to it.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (58, 29, 22, 4, 'Traveling with kids, hope that\'s okay.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (59, 19, 22, 5, 'Vegetarian—open to food suggestions.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (60, 16, 23, 3, 'Can bring extra gear if needed.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (61, 12, 24, 3, 'Flexible with dates and budget.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (62, 29, 24, 5, 'Happy to share accommodation.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (63, 17, 25, 2, 'Looking forward to it.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (64, 16, 26, 5, 'Traveling with kids, hope that\'s okay.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (65, 16, 26, 4, 'Prefer early starts.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (66, 3, 26, 3, 'Traveling with kids, hope that\'s okay.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (67, 23, 26, 2, 'Happy to share accommodation.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (68, 16, 27, 5, 'Flexible with dates and budget.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (69, 24, 27, 5, 'Flexible with dates and budget.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (70, 23, 27, 4, 'Happy to share accommodation.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (71, 8, 28, 1, 'Traveling with kids, hope that\'s okay.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (72, 4, 28, 5, 'Can bring extra gear if needed.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (73, 7, 28, 4, 'Happy to share accommodation.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (74, 4, 29, 2, 'Happy to share accommodation.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (75, 12, 29, 2, 'Happy to share accommodation.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (76, 23, 29, 5, 'Can bring extra gear if needed.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (77, 2, 30, 5, 'Happy to share accommodation.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (78, 21, 31, 4, 'Looking forward to it.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (79, 19, 31, 3, 'Prefer early starts.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (80, 11, 32, 2, 'Excited to join!', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (81, 28, 32, 4, 'Looking forward to it.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (82, 13, 32, 4, 'Looking forward to it.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (83, 21, 32, 1, 'Can bring extra gear if needed.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (84, 3, 33, 2, 'Looking forward to it.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (85, 25, 33, 4, 'Traveling with kids, hope that\'s okay.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (86, 13, 33, 4, 'Prefer early starts.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (87, 10, 34, 5, 'Excited to join!', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (88, 24, 34, 1, 'Traveling with kids, hope that\'s okay.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (89, 7, 34, 3, 'Looking forward to it.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (90, 8, 34, 2, 'Looking forward to it.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (91, 14, 35, 4, 'Prefer early starts.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (92, 8, 36, 3, 'Happy to share accommodation.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (93, 3, 37, 2, 'Happy to share accommodation.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (94, 19, 37, 2, 'Vegetarian—open to food suggestions.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (95, 18, 37, 2, 'Can bring extra gear if needed.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (96, 27, 37, 2, 'Looking forward to it.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (97, 26, 38, 3, 'Happy to share accommodation.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (98, 4, 38, 4, 'Happy to share accommodation.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (99, 9, 39, 5, 'Prefer early starts.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (100, 3, 39, 5, 'Excited to join!', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (101, 24, 39, 3, 'Happy to share accommodation.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (102, 3, 39, 2, 'Excited to join!', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (103, 19, 40, 1, 'Can bring extra gear if needed.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (104, 17, 40, 4, 'Happy to share accommodation.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (105, 19, 40, 4, 'Prefer early starts.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (106, 12, 41, 4, 'Flexible with dates and budget.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (107, 22, 41, 1, 'Can bring extra gear if needed.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (108, 14, 41, 4, 'Happy to share accommodation.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (109, 13, 41, 5, 'Excited to join!', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (110, 11, 42, 3, 'Flexible with dates and budget.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (111, 28, 43, 5, 'Excited to join!', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (112, 28, 43, 5, 'Prefer early starts.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (113, 2, 43, 2, 'Flexible with dates and budget.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (114, 25, 43, 4, 'Prefer early starts.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (115, 9, 44, 5, 'Can bring extra gear if needed.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (116, 15, 44, 4, 'Looking forward to it.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (117, 23, 45, 2, 'Happy to share accommodation.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (118, 1, 45, 5, 'Vegetarian—open to food suggestions.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (119, 27, 46, 1, 'Prefer early starts.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (120, 21, 46, 2, 'Prefer early starts.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (121, 17, 47, 3, 'Vegetarian—open to food suggestions.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (122, 16, 47, 2, 'Prefer early starts.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (123, 5, 47, 4, 'Traveling with kids, hope that\'s okay.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (124, 28, 48, 1, 'Happy to share accommodation.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (125, 11, 48, 5, 'Happy to share accommodation.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (126, 24, 49, 3, 'Prefer early starts.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (127, 15, 49, 5, 'Prefer early starts.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (128, 11, 49, 5, 'Vegetarian—open to food suggestions.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (129, 28, 50, 2, 'Traveling with kids, hope that\'s okay.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (130, 13, 50, 2, 'Vegetarian—open to food suggestions.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (131, 11, 50, 4, 'Vegetarian—open to food suggestions.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (132, 16, 51, 1, 'Can bring extra gear if needed.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (133, 19, 51, 3, 'Looking forward to it.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (134, 17, 52, 4, 'Excited to join!', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (135, 14, 53, 2, 'Looking forward to it.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (136, 26, 53, 3, 'Flexible with dates and budget.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (137, 21, 54, 1, 'Flexible with dates and budget.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (138, 28, 54, 5, 'Vegetarian—open to food suggestions.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (139, 21, 54, 4, 'Excited to join!', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (140, 3, 54, 2, 'Happy to share accommodation.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (141, 14, 55, 1, 'Looking forward to it.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (142, 23, 56, 3, 'Excited to join!', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (143, 11, 56, 1, 'Happy to share accommodation.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (144, 14, 57, 2, 'Traveling with kids, hope that\'s okay.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (145, 14, 57, 5, 'Can bring extra gear if needed.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (146, 6, 57, 1, 'Vegetarian—open to food suggestions.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (147, 16, 58, 5, 'Can bring extra gear if needed.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (148, 15, 58, 3, 'Prefer early starts.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (149, 29, 59, 4, 'Happy to share accommodation.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (150, 3, 60, 4, 'Flexible with dates and budget.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (151, 10, 60, 4, 'Happy to share accommodation.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (152, 7, 61, 4, 'Prefer early starts.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (153, 8, 61, 4, 'Flexible with dates and budget.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (154, 10, 61, 3, 'Excited to join!', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (155, 9, 62, 1, 'Excited to join!', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (156, 24, 62, 4, 'Happy to share accommodation.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (157, 20, 62, 3, 'Traveling with kids, hope that\'s okay.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (158, 7, 62, 5, 'Happy to share accommodation.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (159, 21, 63, 1, 'Excited to join!', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (160, 26, 63, 4, 'Excited to join!', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (161, 24, 64, 2, 'Looking forward to it.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (162, 11, 64, 4, 'Can bring extra gear if needed.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (163, 5, 64, 5, 'Flexible with dates and budget.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (164, 27, 65, 2, 'Happy to share accommodation.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (165, 26, 65, 3, 'Flexible with dates and budget.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (166, 15, 65, 1, 'Can bring extra gear if needed.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (167, 28, 66, 5, 'Flexible with dates and budget.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (168, 26, 66, 4, 'Excited to join!', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (169, 18, 66, 1, 'Prefer early starts.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (170, 22, 66, 3, 'Vegetarian—open to food suggestions.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (171, 4, 67, 2, 'Prefer early starts.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (172, 20, 67, 5, 'Flexible with dates and budget.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (173, 8, 67, 1, 'Prefer early starts.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (174, 21, 68, 4, 'Looking forward to it.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (175, 2, 68, 1, 'Happy to share accommodation.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (176, 4, 68, 1, 'Traveling with kids, hope that\'s okay.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (177, 13, 69, 4, 'Flexible with dates and budget.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (178, 24, 69, 5, 'Vegetarian—open to food suggestions.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (179, 29, 70, 4, 'Looking forward to it.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (180, 20, 70, 4, 'Happy to share accommodation.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (181, 7, 71, 4, 'Prefer early starts.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (182, 28, 71, 3, 'Looking forward to it.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (183, 12, 71, 5, 'Flexible with dates and budget.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (184, 9, 72, 2, 'Looking forward to it.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (185, 3, 72, 2, 'Excited to join!', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (186, 26, 72, 3, 'Traveling with kids, hope that\'s okay.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (187, 26, 72, 5, 'Traveling with kids, hope that\'s okay.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (188, 19, 73, 2, 'Traveling with kids, hope that\'s okay.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (189, 25, 73, 2, 'Excited to join!', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (190, 5, 74, 5, 'Happy to share accommodation.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (191, 4, 74, 1, 'Can bring extra gear if needed.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (192, 26, 75, 2, 'Flexible with dates and budget.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (193, 6, 75, 3, 'Excited to join!', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (194, 24, 75, 4, 'Looking forward to it.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (195, 16, 76, 4, 'Flexible with dates and budget.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (196, 15, 77, 5, 'Traveling with kids, hope that\'s okay.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (197, 24, 78, 5, 'Happy to share accommodation.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (198, 2, 79, 4, 'Vegetarian—open to food suggestions.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (199, 16, 80, 4, 'Looking forward to it.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (200, 20, 81, 2, 'Looking forward to it.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (201, 9, 81, 5, 'Flexible with dates and budget.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (202, 20, 81, 5, 'Happy to share accommodation.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (203, 4, 82, 1, 'Traveling with kids, hope that\'s okay.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (204, 15, 82, 2, 'Vegetarian—open to food suggestions.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (205, 27, 82, 4, 'Vegetarian—open to food suggestions.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (206, 24, 82, 1, 'Flexible with dates and budget.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (207, 22, 83, 3, 'Flexible with dates and budget.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (208, 22, 83, 4, 'Looking forward to it.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (209, 27, 83, 1, 'Looking forward to it.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (210, 24, 84, 3, 'Can bring extra gear if needed.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (211, 19, 85, 5, 'Flexible with dates and budget.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (212, 14, 86, 3, 'Vegetarian—open to food suggestions.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (213, 10, 87, 5, 'Happy to share accommodation.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (214, 19, 88, 5, 'Traveling with kids, hope that\'s okay.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (215, 8, 89, 1, 'Flexible with dates and budget.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (216, 12, 89, 1, 'Happy to share accommodation.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (217, 8, 89, 4, 'Excited to join!', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (218, 30, 89, 3, 'Excited to join!', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (219, 23, 90, 3, 'Flexible with dates and budget.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (220, 1, 90, 2, 'Can bring extra gear if needed.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (221, 22, 90, 4, 'Looking forward to it.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (222, 3, 91, 5, 'Traveling with kids, hope that\'s okay.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (223, 15, 92, 3, 'Can bring extra gear if needed.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (224, 10, 92, 3, 'Looking forward to it.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (225, 5, 92, 2, 'Excited to join!', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (226, 3, 92, 3, 'Prefer early starts.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (227, 16, 93, 5, 'Prefer early starts.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (228, 9, 93, 2, 'Looking forward to it.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (229, 14, 93, 1, 'Happy to share accommodation.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (230, 22, 93, 5, 'Prefer early starts.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (231, 2, 94, 2, 'Vegetarian—open to food suggestions.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (232, 2, 94, 1, 'Traveling with kids, hope that\'s okay.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (233, 7, 94, 2, 'Happy to share accommodation.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (234, 4, 95, 1, 'Prefer early starts.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (235, 14, 95, 2, 'Can bring extra gear if needed.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (236, 18, 95, 2, 'Flexible with dates and budget.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (237, 28, 96, 1, 'Vegetarian—open to food suggestions.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (238, 15, 96, 1, 'Flexible with dates and budget.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (239, 14, 96, 5, 'Vegetarian—open to food suggestions.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (240, 21, 96, 4, 'Happy to share accommodation.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (241, 1, 97, 3, 'Can bring extra gear if needed.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (242, 15, 97, 3, 'Looking forward to it.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (243, 28, 97, 1, 'Traveling with kids, hope that\'s okay.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (244, 19, 97, 4, 'Looking forward to it.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (245, 24, 98, 3, 'Traveling with kids, hope that\'s okay.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (246, 25, 98, 2, 'Looking forward to it.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (247, 21, 98, 1, 'Traveling with kids, hope that\'s okay.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (248, 24, 99, 2, 'Traveling with kids, hope that\'s okay.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (249, 5, 99, 3, 'Traveling with kids, hope that\'s okay.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (250, 20, 99, 2, 'Looking forward to it.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (251, 15, 100, 5, 'Prefer early starts.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (252, 30, 100, 5, 'Flexible with dates and budget.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (253, 11, 100, 2, 'Prefer early starts.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (254, 16, 100, 4, 'Happy to share accommodation.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (255, 12, 101, 5, 'Looking forward to it.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (256, 15, 102, 1, 'Excited to join!', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (257, 27, 102, 3, 'Looking forward to it.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (258, 28, 102, 1, 'Vegetarian—open to food suggestions.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (259, 19, 102, 5, 'Excited to join!', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (260, 11, 103, 5, 'Prefer early starts.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (261, 5, 103, 1, 'Prefer early starts.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (262, 23, 104, 1, 'Can bring extra gear if needed.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (263, 8, 104, 4, 'Prefer early starts.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (264, 17, 104, 5, 'Can bring extra gear if needed.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (265, 30, 105, 3, 'Vegetarian—open to food suggestions.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (266, 25, 105, 3, 'Excited to join!', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (267, 21, 105, 3, 'Looking forward to it.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (268, 18, 106, 4, 'Happy to share accommodation.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (269, 11, 107, 1, 'Can bring extra gear if needed.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (270, 10, 107, 4, 'Can bring extra gear if needed.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (271, 10, 108, 5, 'Vegetarian—open to food suggestions.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (272, 27, 109, 2, 'Looking forward to it.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (273, 22, 109, 4, 'Flexible with dates and budget.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (274, 12, 109, 3, 'Can bring extra gear if needed.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (275, 25, 110, 4, 'Traveling with kids, hope that\'s okay.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (276, 5, 110, 2, 'Looking forward to it.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (277, 28, 110, 1, 'Excited to join!', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (278, 29, 111, 5, 'Can bring extra gear if needed.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (279, 13, 111, 2, 'Can bring extra gear if needed.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (280, 27, 111, 5, 'Can bring extra gear if needed.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (281, 2, 112, 4, 'Flexible with dates and budget.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (282, 24, 112, 2, 'Prefer early starts.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (283, 10, 112, 4, 'Traveling with kids, hope that\'s okay.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (284, 8, 112, 3, 'Prefer early starts.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (285, 22, 113, 5, 'Prefer early starts.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (286, 25, 113, 3, 'Vegetarian—open to food suggestions.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (287, 17, 113, 4, 'Can bring extra gear if needed.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (288, 28, 114, 3, 'Excited to join!', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (289, 16, 114, 3, 'Looking forward to it.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (290, 10, 115, 1, 'Can bring extra gear if needed.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (291, 29, 116, 5, 'Can bring extra gear if needed.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (292, 3, 116, 2, 'Prefer early starts.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (293, 30, 116, 1, 'Vegetarian—open to food suggestions.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (294, 13, 116, 5, 'Flexible with dates and budget.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (295, 3, 117, 3, 'Traveling with kids, hope that\'s okay.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (296, 11, 117, 1, 'Flexible with dates and budget.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (297, 5, 117, 1, 'Happy to share accommodation.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (298, 23, 117, 2, 'Prefer early starts.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (299, 29, 118, 1, 'Excited to join!', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (300, 27, 119, 2, 'Vegetarian—open to food suggestions.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (301, 25, 119, 3, 'Traveling with kids, hope that\'s okay.', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (302, 2, 120, 2, 'Vegetarian—open to food suggestions.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (303, 3, 121, 1, 'Prefer early starts.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (304, 18, 121, 1, 'Traveling with kids, hope that\'s okay.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (305, 5, 121, 3, 'Vegetarian—open to food suggestions.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (306, 20, 121, 3, 'Traveling with kids, hope that\'s okay.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (307, 6, 122, 1, 'Flexible with dates and budget.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (308, 17, 122, 5, 'Excited to join!', 'accepted');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (309, 28, 122, 4, 'Excited to join!', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (310, 13, 122, 3, 'Happy to share accommodation.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (311, 12, 123, 1, 'Flexible with dates and budget.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (312, 25, 124, 5, 'Flexible with dates and budget.', 'pending');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (313, 12, 125, 5, 'Flexible with dates and budget.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (314, 27, 126, 4, 'Prefer early starts.', 'declined');
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES (315, 6, 126, 2, 'Looking forward to it.', 'declined');