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
(1, 'Eiffel Tower', 'Champ de Mars, 5 Av. Anatole France', '75007', 'Paris', '', 'France', 2.2945, 48.8584),
(2, 'Statue of Liberty', 'Liberty Island', '10004', 'New York', 'NY', 'USA', -74.0445, 40.6892),
(3, 'Great Wall', 'Huairou, Beijing, China', '', 'Beijing', '', 'China', 116.5704, 40.4319),
(4, 'Sydney Opera House', 'Bennelong Point, Sydney NSW 2000', '2000', 'Sydney', 'NSW', 'Australia', 151.2153, -33.8568),
(5, 'Christ the Redeemer', 'Parque Nacional da Tijuca - Alto da Boa Vista', '22241-125', 'Rio de Janeiro', 'RJ', 'Brazil', -43.2105, -22.9519),
(6, 'Machu Picchu', 'Aguas Calientes, Cusco, Peru', '', 'Cusco Region', '', 'Peru', -72.5450, -13.1631),
(7, 'Pyramids of Giza', 'Al Haram, Giza Governorate', '', 'Giza', '', 'Egypt', 31.1310, 29.9792),
(8, 'Mount Fuji', 'Kitayama, Fujinomiya, Shizuoka, Japan', '403-0005', 'Fujiyoshida', 'Yamanashi', 'Japan', 138.7278, 35.3606),
(9, 'Santorini', 'Santorini Island, Thira, Greece', '84700', 'Santorini', 'Cyclades', 'Greece', 25.4217, 36.3932),
(10, 'Banff National Park', '1 Banff Ave, Banff, AB T1L 1K2, Canada', 'T1L', 'Banff', 'Alberta', 'Canada', -115.5708, 51.1784),
(11, 'Table Mountain', 'Tafelberg Road, Cape Town, 8001, South Africa', '8001', 'Cape Town', 'Western Cape', 'South Africa', 18.4218, -33.9583),
(12, 'Taj Mahal', 'Dharmapuri, Forest Colony, Tajganj, Agra, Uttar Pradesh 282001, India', '282001', 'Agra', 'Uttar Pradesh', 'India', 78.0421, 27.1751),
(13, 'Angkor Wat', 'Angkor, Siem Reap, Cambodia', '17252', 'Siem Reap', '', 'Cambodia', 103.8697, 13.4125),
(14, 'Uluru', 'Uluru-Kata Tjuta National Park, Petermann, NT 0872, Australia', '0872', 'Uluru', 'NT', 'Australia', 131.0369, -25.3444),
(15, 'Petra', 'Wadi Musa, Jordan', '', 'Wadi Musa', '', 'Jordan', 35.4444, 30.3285),
(16, 'Grand Canyon', 'Grand Canyon Village, AZ 86023, USA', '86023', 'Grand Canyon', 'AZ', 'USA', -112.1158, 36.1016),
(17, 'Iguazu Falls', 'Foz do Iguaçu, State of Paraná, Brazil', '', 'Foz do Iguaçu', 'PR', 'Brazil', -54.4366, -25.6953),
(18, 'Salar de Uyuni', 'Salar de Uyuni, Potosí, Bolivia', '', 'Uyuni', 'Potosí', 'Bolivia', -67.4891, -20.2114),
(19, 'Hallstatt', 'Seestrasse 15, 4830 Hallstatt, Austria', '4830', 'Hallstatt', 'Upper Austria', 'Austria', 13.6499, 47.5622),
(20, 'Cinque Terre', 'Riomaggiore, La Spezia, Liguria, Italy', '19017', 'Riomaggiore', 'Liguria', 'Italy', 9.7371, 44.1065),
(21, 'Reykjavík Blue Lagoon', 'Norðurljósavegur 9, 240 Grindavík, Iceland', '240', 'Grindavík', '', 'Iceland', -22.4495, 63.8810),
(22, 'Bali Ubud', 'Ubud, Gianyar Regency, Bali, Indonesia', '80571', 'Ubud', 'Bali', 'Indonesia', 115.2631, -8.5065),
(23, 'Phuket Old Town', 'Talat Yai, Mueang Phuket District, Phuket 83000, Thailand', '83000', 'Phuket', '', 'Thailand', 98.3976, 7.8841),
(24, 'Queenstown', 'Queenstown, Otago, New Zealand', '9300', 'Queenstown', 'Otago', 'New Zealand', 168.6628, -45.0312),
(25, 'Zermatt', 'Bahnhofstrasse 1, 3920 Zermatt, Switzerland', '3920', 'Zermatt', 'Valais', 'Switzerland', 7.7479, 46.0207),
(26, 'Dubai Burj Khalifa', '1 Sheikh Mohammed bin Rashid Blvd - Downtown Dubai', '00000', 'Dubai', '', 'UAE', 55.2743, 25.1972),
(27, 'Hanoi Old Quarter', 'Hoàn Kiếm, Hanoi, Vietnam', '100000', 'Hanoi', '', 'Vietnam', 105.8524, 21.0285),
(28, 'Lisbon Alfama', 'Alfama, Lisbon, Portugal', '1100-585', 'Lisbon', '', 'Portugal', -9.1299, 38.7118),
(29, 'Louvre Museum', 'Rue de Rivoli', '75001', 'Paris', '', 'France', 2.3376, 48.8606),
(30, 'Mont Saint-Michel', '50170 Le Mont-Saint-Michel, France', '50170', 'Normandy', '', 'France', -1.5118, 48.6360),
(31, 'Versailles Palace', 'Place d''Armes, 78000 Versailles, France', '78000', 'Versailles', '', 'France', 2.1204, 48.8048),
(32, 'Times Square', 'Manhattan, NY 10036, USA', '10036', 'New York', 'NY', 'USA', -73.9855, 40.7580),
(33, 'Yosemite National Park', 'Yosemite National Park, CA 95389, USA', '95389', 'Yosemite Valley', 'CA', 'USA', -119.5383, 37.8651),
(34, 'Golden Gate Bridge', 'Golden Gate Bridge, San Francisco, CA 94129, USA', '94129', 'San Francisco', 'CA', 'USA', -122.4783, 37.8199),
(35, 'Kyoto Fushimi Inari Shrine', '68 Fukakusa Yabunouchicho, Fushimi Ward, Kyoto, 612-0882, Japan', '612-0882', 'Kyoto', 'Kyoto', 'Japan', 135.7727, 34.9671),
(36, 'Tokyo Shibuya Crossing', '1 Chome-2-1 Dogenzaka, Shibuya, Tokyo 150-0043, Japan', '150-0042', 'Tokyo', 'Tokyo', 'Japan', 139.6991, 35.6598),
(37, 'Nara Deer Park', '469 Zoshicho, Nara, 630-8211, Japan', '630-8211', 'Nara', 'Nara', 'Japan', 135.8398, 34.6851),
(38, 'Bondi Beach', 'Bondi Beach, NSW 2026, Australia', '2026', 'Sydney', 'NSW', 'Australia', 151.2743, -33.8909),
(39, 'Great Barrier Reef', 'Cairns, QLD, Australia', '4870', 'Cairns', 'QLD', 'Australia', 146.4019, -18.2871),
(40, 'Blue Mountains', 'Katoomba, NSW 2780, Australia', '2780', 'Katoomba', 'NSW', 'Australia', 150.3129, -33.7145),
(41, 'Sugarloaf Mountain', 'Av. Pasteur, 520 - Urca, Rio de Janeiro - RJ, 22290-270, Brazil', '22290-270', 'Rio de Janeiro', 'RJ', 'Brazil', -43.1560, -22.9490),
(42, 'Pelourinho Historic Center', 'Pelourinho, Salvador - BA, Brazil', '40026-280', 'Salvador', 'BA', 'Brazil', -38.5126, -12.9734),
(43, 'Amazon Rainforest Gateway', 'Manaus, State of Amazonas, Brazil', '69000-000', 'Manaus', 'AM', 'Brazil', -60.0218, -3.1190),
(44, 'Florence Duomo', 'Piazza del Duomo, 50122 Firenze FI, Italy', '50122', 'Florence', 'Tuscany', 'Italy', 11.2562, 43.7731),
(45, 'Venice Grand Canal', 'Grand Canal, Venice, Italy', '30100', 'Venice', 'Veneto', 'Italy', 12.3360, 45.4387),
(46, 'Colosseum', 'Piazza del Colosseo, 1, 00184 Roma RM, Italy', '00184', 'Rome', 'Lazio', 'Italy', 12.4922, 41.8902),
(47, 'Jaipur Amber Fort', 'Devisinghpura, Amer, Jaipur, Rajasthan 302001, India', '302001', 'Jaipur', 'Rajasthan', 'India', 75.8513, 26.9852),
(48, 'Kerala Backwaters', 'Alleppey, Kerala, India', '688001', 'Alleppey', 'Kerala', 'India', 76.3409, 9.4981),
(49, 'Varanasi Ghats', 'Varanasi, Uttar Pradesh, India', '221001', 'Varanasi', 'Uttar Pradesh', 'India', 82.9922, 25.3176),
(50, 'Mykonos Windmills', 'Mykonos 846 00, Greece', '84600', 'Mykonos', 'Cyclades', 'Greece', 25.3262, 37.4439),
(51, 'Delphi Archaeological Site', 'Delphi 330 54, Greece', '33054', 'Delphi', 'Phocis', 'Greece', 22.5020, 38.4820),
(52, 'Niagara Falls', 'Niagara Falls, ON, Canada', 'L2G', 'Niagara Falls', 'Ontario', 'Canada', -79.0441, 43.0896),
(53, 'Whistler Blackcomb', '4545 Blackcomb Way, Whistler, BC V0N 1B4, Canada', 'V0N', 'Whistler', 'British Columbia', 'Canada', -122.9548, 50.1163),
(54, 'Cape Point', 'Cape Point, Cape Town, 7975, South Africa', '7975', 'Cape Town', 'Western Cape', 'South Africa', 18.4975, -34.3568),
(55, 'Robben Island', 'Robben Island, Cape Town, 7400, South Africa', '7400', 'Cape Town', 'Western Cape', 'South Africa', 18.3965, -33.8055);

-- =======================
-- trip rows
-- =======================
INSERT INTO trip (trip_id, owner_id, max_buddies, start_date, end_date, description) VALUES
(1, 1, 4, '2025-06-17', '2025-06-25', 'Exploring ancient ruins and hidden cafés—solo but open to serendipity.'),
(2, 1, 5, '2026-08-18', '2026-08-30', 'Budget backpacking across mountain villages—let’s split snacks and stories.'),
(3, 1, 4, '2025-03-02', '2025-03-10', 'Seeking thrill-seekers for canyon dives and midnight bonfires.'),
(4, 2, 5, '2027-01-02', '2027-01-20', 'Family road trip with room for one more—must love board games.'),
(5, 2, 5, '2025-12-12', '2025-12-19', 'Romantic escape with room for fellow sunset chasers.'),
(6, 2, 5, '2025-11-07', '2025-11-12', 'Spontaneous couple’s getaway—open to group hikes and wine tastings.'),
(7, 3, 4, '2026-12-17', '2027-01-04', 'Forest trails, campfire chats, and zero Wi-Fi—nature lovers welcome.'),
(8, 3, 5, '2026-06-28', '2026-07-11', 'Solo trek through coastal towns—sharing costs and laughs encouraged.'),
(9, 3, 5, '2026-04-20', '2026-05-08', 'Art museums by day, jazz bars by night—culture crew wanted.'),
(10, 4, 5, '2025-12-20', '2025-12-31', 'Solo traveler chasing northern lights—bring your camera and cocoa.'),
(11, 5, 5, '2025-12-13', '2025-12-30', 'Street food crawl across three cities—forks optional, fun mandatory.'),
(12, 6, 5, '2025-02-06', '2025-02-19', 'Mountain meditation and waterfall hikes—peaceful souls preferred.'),
(13, 6, 5, '2027-10-23', '2027-10-31', 'Solo wanderer seeking spontaneous companions for city-hopping.'),
(14, 7, 5, '2025-10-28', '2025-11-11', 'Gastronomic adventure through spice markets and rooftop eateries.'),
(15, 7, 4, '2026-09-11', '2026-09-22', 'Family of five heading south—room for a storyteller or two.'),
(16, 7, 5, '2026-05-25', '2026-06-06', 'Solo journey through desert landscapes—sunrise hikes and stargazing.'),
(17, 8, 4, '2026-03-09', '2026-03-17', 'Friends reuniting for a cultural deep dive—open to new faces.'),
(18, 9, 5, '2025-04-18', '2025-05-04', 'Food tour with a twist—local chefs, secret recipes, and spice.'),
(19, 9, 5, '2026-03-10', '2026-03-26', 'Cultural immersion trip—temples, textiles, and tea ceremonies.'),
(20, 10, 5, '2026-06-30', '2026-07-07', 'Backpacking through waterfalls and hostels—let’s keep it light.'),
(21, 10, 5, '2027-09-24', '2027-10-06', 'Nature retreat with yoga mornings and trail mix afternoons.'),
(22, 10, 5, '2027-03-03', '2027-03-20', 'Digital nomads unite—co-working by day, exploring by night.'),
(23, 11, 4, '2026-09-13', '2026-09-28', 'Couple’s escape with room for a fellow dreamer.'),
(24, 12, 5, '2027-11-22', '2027-12-07', 'Cultural circuit with museum marathons and local dance nights.'),
(25, 13, 3, '2027-04-13', '2027-04-24', 'Foodie fiesta—tacos, tagines, and taste-testing galore.'),
(26, 14, 5, '2026-05-30', '2026-06-19', 'Trailblazing through national parks—boots, bugs, and bliss.'),
(27, 15, 4, '2025-02-12', '2025-02-25', 'Romantic road trip with space for a third wheel (with good vibes).'),
(28, 16, 5, '2025-06-29', '2025-07-12', 'Photography expedition—golden hours and moody skies guaranteed.'),
(29, 16, 5, '2026-02-10', '2026-02-27', 'Couple’s retreat with open trip_destination—join us for the unexpected.'),
(30, 16, 5, '2025-10-05', '2025-10-14', 'Nature escape with forest bathing and hammock naps.'),
(31, 17, 5, '2027-06-21', '2027-06-27', 'Backpacking duo seeking budget-savvy explorers.'),
(32, 18, 5, '2026-02-17', '2026-02-27', 'First-time traveler—open to tips, tricks, and travel buddies.'),
(33, 18, 5, '2025-03-01', '2025-03-08', 'Photo safari—sunrises, silhouettes, and shutter clicks.'),
(34, 18, 5, '2025-06-18', '2025-07-07', 'Cultural caravan—markets, music, and midnight strolls.'),
(35, 19, 5, '2027-07-16', '2027-07-21', 'Remote work week in paradise—Wi-Fi and waves included.'),
(36, 19, 5, '2026-11-07', '2026-11-20', 'Solo explorer seeking kindred spirits for shared adventures.'),
(37, 19, 4, '2026-06-22', '2026-07-06', 'Budget backpacking with beach bonfires and hostel hangs.'),
(38, 20, 3, '2026-05-03', '2026-05-08', 'Work-from-anywhere crew—bring your laptop and wanderlust.'),
(39, 20, 5, '2026-11-09', '2026-11-19', 'Solo mission to find the best view and worst coffee.'),
(40, 21, 5, '2027-07-31', '2027-08-14', 'First-time abroad—open to guidance and good company.'),
(41, 21, 5, '2027-03-06', '2027-03-17', 'Culinary crawl through three countries—forks up!'),
(42, 21, 5, '2025-06-29', '2025-07-09', 'Remote work retreat—cozy cafés and quiet corners.'),
(43, 22, 5, '2026-08-22', '2026-09-06', 'Family adventure with space for storytellers and snack-sharers.'),
(44, 23, 5, '2027-06-15', '2027-07-01', 'Couple’s journey—open to fellow romantics and wanderers.'),
(45, 24, 5, '2025-10-23', '2025-11-04', 'Solo trek with a twist—join for the laughs, stay for the views.'),
(46, 25, 5, '2026-01-31', '2026-02-20', 'Work and wander—co-working by day, wine by night.'),
(47, 26, 5, '2025-03-10', '2025-03-19', 'Remote work escape—bring your best playlists and productivity.'),
(48, 27, 5, '2026-11-09', '2026-11-22', 'First-time traveler—curious, cautious, and caffeinated.'),
(49, 28, 5, '2025-01-03', '2025-01-14', 'Couple’s trip with room for a third adventurer.'),
(50, 28, 3, '2027-03-19', '2027-04-02', 'Foodie tour through hidden gems—expect spice, stories, and surprises.'),
(51, 29, 5, '2027-04-17', '2027-05-06', 'Couple’s escape with flexible plans—open to spontaneous detours.'),
(52, 29, 5, '2027-12-03', '2027-12-15', 'Winter wanderlust—hot drinks, cold air, and cozy company.'),
(53, 30, 5, '2027-11-08', '2027-11-20', 'Cultural deep dive with local guides and off-the-map adventures.');

-- =======================
-- trip_destination rows
-- =======================
INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(1, 3, 1, '2025-06-17', '2025-06-19', 1, 'Exploring ancient ruins and hidden cafés along the Great Wall.'),
(2, 23, 1, '2025-06-20', '2025-06-23', 2, 'Wandering through the charming streets of Phuket Old Town.'),
(3, 2, 2, '2026-08-18', '2026-08-22', 1, 'Budget backpacking with a group to the Statue of Liberty.'),
(4, 28, 2, '2026-08-23', '2026-08-25', 2, 'Sharing snacks and stories in the colorful Alfama district of Lisbon.'),
(5, 8, 3, '2025-03-02', '2025-03-05', 1, 'A thrill-seeker''s trip to Mount Fuji.'),
(6, 9, 3, '2025-03-06', '2025-03-10', 2, 'Enjoying midnight bonfires on the beaches of Santorini.'),
(7, 24, 4, '2027-01-02', '2027-01-08', 1, 'A family road trip to Queenstown, with room for one more storyteller.'),
(8, 19, 4, '2027-01-09', '2027-01-14', 2, 'Finding peace and tranquility in the picturesque village of Hallstatt.'),
(9, 26, 5, '2025-12-12', '2025-12-17', 1, 'A romantic escape to Dubai, chasing sunsets at the Burj Khalifa.'),
(10, 16, 5, '2025-12-18', '2025-12-19', 2, 'The perfect end to a romantic trip at the Grand Canyon.'),
(11, 4, 6, '2025-11-07', '2025-11-12', 1, 'A spontaneous couple''s getaway, enjoying a wine tasting at the Sydney Opera House.'),
(12, 14, 7, '2026-12-17', '2026-12-19', 1, 'A nature lover''s trip to Uluru, with forest trails and campfire chats.'),
(13, 15, 7, '2026-12-20', '2026-12-22', 2, 'Exploring the trails of Petra and connecting with nature.'),
(14, 28, 7, '2026-12-23', '2026-12-25', 3, 'Celebrating Christmas in Lisbon, with festive cheer and historic streets.'),
(15, 24, 8, '2026-06-28', '2026-06-30', 1, 'A solo trek to the coastal towns of Queenstown.'),
(16, 11, 8, '2026-07-01', '2026-07-04', 2, 'Sharing laughs and costs at Table Mountain with a group of solo trekkers.'),
(17, 26, 8, '2026-07-05', '2026-07-08', 3, 'Concluding my solo adventure with a trek to the Burj Khalifa.'),
(18, 18, 9, '2026-04-20', '2026-04-23', 1, 'Our art and jazz tour starts at the surreal Salar de Uyuni.'),
(19, 15, 9, '2026-04-24', '2026-04-29', 2, 'Exploring the art and architecture of Petra on a cultural getaway.'),
(20, 9, 10, '2025-12-20', '2025-12-23', 1, 'Chasing sunsets and starry skies in Santorini.'),
(21, 15, 10, '2025-12-24', '2025-12-26', 2, 'A Christmas Eve exploration of the ancient city of Petra, a serene and magical experience.'),
(22, 26, 11, '2025-12-13', '2025-12-15', 1, 'Beginning our street food adventure at the iconic Burj Khalifa.'),
(23, 28, 11, '2025-12-16', '2025-12-18', 2, 'Savoring the authentic flavors of Lisbon as we explore its hidden culinary gems.'),
(24, 18, 11, '2025-12-19', '2025-12-25', 3, 'Concluding our culinary tour in the otherworldly salt flats of Salar de Uyuni.'),
(25, 3, 12, '2025-02-06', '2025-02-09', 1, 'Finding a peaceful retreat on the Great Wall, perfect for reflection and quiet moments.'),
(26, 25, 12, '2025-02-10', '2025-02-13', 2, 'A tranquil escape to Zermatt, with its serene mountain views perfect for meditation.'),
(27, 16, 13, '2027-10-23', '2027-10-28', 1, 'City-hopping with a visit to the Grand Canyon, a truly inspiring and unforgettable stop.'),
(28, 7, 13, '2027-10-29', '2027-10-31', 2, 'Ending our journey at the Pyramids of Giza, a monumental cultural experience.'),
(29, 13, 14, '2025-10-28', '2025-11-02', 1, 'Starting our gastronomic adventure with an exploration of Angkor Wat, a feast for the eyes and the soul.'),
(30, 1, 14, '2025-11-03', '2025-11-07', 2, 'Enjoying a cultural feast in Paris with a visit to the Eiffel Tower.'),
(31, 29, 14, '2025-11-08', '2025-11-12', 3, 'A delicious detour through the Louvre Museum on our cultural feast.'),
(32, 14, 15, '2026-09-18', '2026-09-22', 2, 'A family trip to the majestic Uluru, a truly humbling and beautiful experience.'),
(33, 7, 16, '2026-05-25', '2026-05-28', 1, 'Beginning our solo journey through the desert with a visit to the historic Pyramids of Giza.'),
(34, 10, 16, '2026-05-29', '2026-05-31', 2, 'Stargazing and hiking in Banff National Park, a perfect way to embrace the solitude of nature.'),
(35, 24, 17, '2026-03-09', '2026-03-11', 1, 'A cultural deep dive with friends, beginning with the stunning landscapes of Queenstown.'),
(36, 18, 17, '2026-03-12', '2026-03-14', 2, 'Exploring the unique, vibrant beauty of Salar de Uyuni with good company.'),
(37, 2, 17, '2026-03-15', '2026-03-17', 3, 'Ending our reunion trip at the iconic Statue of Liberty, celebrating our friendship and new memories.'),
(38, 17, 18, '2025-04-18', '2025-04-20', 1, 'A food tour with a twist, starting at the breathtaking Iguazu Falls, a natural wonder to behold.'),
(39, 28, 18, '2025-04-21', '2025-04-27', 2, 'Savoring local recipes and scenic drives in Lisbon''s Alfama district, a feast for the senses.'),
(40, 6, 18, '2025-04-28', '2025-04-30', 3, 'Tasting sustainable living in the ancient city of Machu Picchu, a harmonious blend of culture and nature.'),
(41, 3, 19, '2026-03-10', '2026-03-12', 1, 'Our cultural immersion trip begins at the majestic Great Wall, a symbol of history and tradition.'),
(42, 20, 19, '2026-03-13', '2026-03-16', 2, 'Exploring the colorful and charming villages of Cinque Terre, a perfect setting for a cultural retreat.'),
(43, 4, 20, '2026-06-30', '2026-07-06', 1, 'Backpacking and hostel stays in Sydney, a perfect way to experience the city and its iconic Opera House.'),
(44, 2, 21, '2027-09-24', '2027-09-30', 1, 'A city retreat to New York, enjoying the peacefulness of the Statue of Liberty amidst the bustling city.'),
(45, 20, 21, '2027-10-01', '2027-10-06', 2, 'Yoga and trail mix afternoons in Cinque Terre, finding balance and beauty in nature and culture.'),
(46, 11, 22, '2027-03-03', '2027-03-07', 1, 'Co-working and exploring in Cape Town, with stunning views of Table Mountain as a daily inspiration.'),
(47, 9, 22, '2027-03-08', '2027-03-11', 2, 'Exploring the creative scene in Santorini, the perfect backdrop for digital nomads to thrive.'),
(48, 7, 22, '2027-03-12', '2027-03-16', 3, 'Finding inspiration in the ancient Pyramids of Giza, a humbling and thought-provoking co-working location.'),
(49, 22, 22, '2027-03-17', '2027-03-20', 4, 'Our work and travel journey concludes in the tranquil setting of Bali Ubud.'),
(50, 22, 23, '2026-09-13', '2026-09-17', 1, 'A couples escape to Bali Ubud, finding peace and inspiration in its natural beauty.'),
(51, 21, 23, '2026-09-18', '2026-09-23', 2, 'Concluding our escape with a romantic dip in the Reykjavík Blue Lagoon.'),
(52, 25, 24, '2027-11-22', '2027-11-27', 1, 'Our cultural circuit begins in Zermatt, a serene and picturesque start.'),
(53, 3, 24, '2027-11-28', '2027-12-04', 2, 'Participating in a civic engagement seminar at the Great Wall, a truly unique and inspiring experience.'),
(54, 1, 24, '2027-12-05', '2027-12-07', 3, 'Ending our circuit with a visit to the Eiffel Tower, a romantic and memorable conclusion.'),
(55, 3, 25, '2027-04-13', '2027-04-16', 1, 'A foodie fiesta starting with a visit to the Great Wall, enjoying delicious local cuisine along the way.'),
(56, 18, 25, '2027-04-17', '2027-04-23', 2, 'Continuing our culinary journey in the ethereal landscapes of Salar de Uyuni.'),
(57, 5, 26, '2026-05-30', '2026-06-02', 1, 'Trailblazing through national parks, starting with a visit to Christ the Redeemer and its breathtaking views.'),
(58, 12, 26, '2026-06-03', '2026-06-07', 2, 'Exploring the majestic Taj Mahal, a true highlight of our adventure.'),
(59, 3, 26, '2026-06-08', '2026-06-12', 3, 'Hiking the Great Wall, enjoying the serene views as we conclude our park-hopping trip.'),
(60, 15, 27, '2025-02-12', '2025-02-18', 1, 'A romantic road trip to the ancient city of Petra, a beautiful and historic setting.'),
(61, 27, 27, '2025-02-19', '2025-02-23', 2, 'Concluding our romantic getaway in the charming Hanoi Old Quarter.'),
(62, 26, 28, '2025-06-29', '2025-07-05', 1, 'A photography expedition starting at the breathtaking Burj Khalifa, capturing golden hours and moody skies.'),
(63, 21, 28, '2025-07-06', '2025-07-12', 2, 'A truly magical photography experience at the Reykjavík Blue Lagoon.'),
(64, 4, 29, '2026-02-10', '2026-02-15', 1, 'A couples retreat to the Sydney Opera House, a perfect backdrop for the unexpected.'),
(65, 3, 29, '2026-02-16', '2026-02-20', 2, 'Exploring the Great Wall together, sharing new experiences and making memories.'),
(66, 15, 29, '2026-02-21', '2026-02-27', 3, 'A romantic end to our retreat, exploring the magical city of Petra.'),
(67, 10, 30, '2025-10-05', '2025-10-09', 1, 'A nature escape to Banff National Park, with peaceful trails and serene landscapes.'),
(68, 16, 30, '2025-10-10', '2025-10-14', 2, 'Hammock naps and stargazing at the Grand Canyon, a truly breathtaking natural escape.'),
(69, 13, 31, '2027-06-21', '2027-06-27', 1, 'Backpacking with a buddy to Angkor Wat, exploring ancient ruins on a budget.'),
(70, 2, 32, '2026-02-17', '2026-02-27', 1, 'First-time traveler trip to the Statue of Liberty, a classic and inspiring start to my journey.'),
(71, 15, 33, '2025-03-01', '2025-03-08', 1, 'A photo safari to the ancient city of Petra, capturing the sunrises and silhouettes.'),
(72, 17, 34, '2025-06-18', '2025-06-25', 1, 'A cultural caravan starting at the breathtaking Iguazu Falls, a stunning start to our journey.'),
(73, 28, 34, '2025-06-26', '2025-07-01', 2, 'Exploring the markets and music of Lisbon, a sensory journey.'),
(74, 26, 34, '2025-07-02', '2025-07-07', 3, 'Ending our cultural journey with a stroll through the bustling city of Dubai.'),
(75, 21, 35, '2027-07-16', '2027-07-21', 1, 'A remote work week at the stunning Reykjavík Blue Lagoon, with Wi-Fi and relaxation.'),
(76, 22, 36, '2026-11-07', '2026-11-14', 1, 'A solo adventure to Bali Ubud, a spiritual escape with a kindred spirit.'),
(77, 9, 36, '2026-11-15', '2026-11-20', 2, 'Concluding our journey in Santorini, a breathtaking end to our shared adventures.'),
(78, 2, 37, '2026-06-22', '2026-06-28', 1, 'Budget backpacking with urban hikes and city views near the Statue of Liberty.'),
(79, 16, 37, '2026-06-29', '2026-07-06', 2, 'Enjoying the rustic hostel life and beautiful views at the Grand Canyon.'),
(80, 24, 38, '2026-05-03', '2026-05-08', 1, 'A work-from-anywhere trip to Queenstown, a perfect blend of work and wanderlust.'),
(81, 19, 39, '2026-11-09', '2026-11-19', 1, 'A solo mission to Hallstatt, finding the best view in this fairytale town.'),
(82, 14, 40, '2027-07-31', '2027-08-07', 1, 'My first time abroad, starting with a visit to the sacred Uluru, a humbling and inspiring experience.'),
(83, 2, 40, '2027-08-08', '2027-08-14', 2, 'Concluding my first journey with a visit to the Statue of Liberty.'),
(84, 1, 41, '2027-03-06', '2027-03-09', 1, 'A culinary crawl through Paris, starting with a feast at the Eiffel Tower.'),
(85, 28, 41, '2027-03-10', '2027-03-13', 2, 'Tasting the best of Lisbon''s cuisine on our gastronomic tour.'),
(86, 26, 41, '2027-03-14', '2027-03-17', 3, 'A final feast in Dubai as we conclude our culinary crawl.'),
(87, 10, 42, '2025-06-29', '2025-07-09', 1, 'A remote work retreat to Banff National Park, with cozy cafés and quiet corners.'),
(88, 17, 43, '2026-08-22', '2026-08-29', 1, 'A family adventure to Iguazu Falls, a breathtaking place for storytelling.'),
(89, 19, 43, '2026-08-30', '2026-09-06', 2, 'Snack-sharing and story-telling in the beautiful town of Hallstatt.'),
(90, 22, 44, '2027-06-15', '2027-06-25', 1, 'A couple''s journey to Bali Ubud, a perfect place for romantic wanderers.'),
(91, 28, 44, '2027-06-26', '2027-07-01', 2, 'Our romantic journey concludes in the charming Alfama district of Lisbon.'),
(92, 11, 45, '2025-10-23', '2025-10-28', 1, 'A solo trek with a twist, starting at Table Mountain, a perfect place for laughs and views.'),
(93, 26, 45, '2025-10-29', '2025-11-04', 2, 'A solo trek to the Burj Khalifa, a breathtaking conclusion to my journey.'),
(94, 15, 46, '2026-01-31', '2026-02-09', 1, 'Working by day and exploring by night in the ancient city of Petra, a perfect blend.'),
(95, 3, 46, '2026-02-10', '2026-02-20', 2, 'Wine by night and work by day in the historic setting of the Great Wall.'),
(96, 26, 47, '2025-03-10', '2025-03-19', 1, 'A remote work escape to the stunning Burj Khalifa, the perfect place to be productive and inspired.'),
(97, 1, 48, '2026-11-09', '2026-11-15', 1, 'A first-time traveler''s journey to the Eiffel Tower, a curious and captivating start.'),
(98, 28, 48, '2026-11-16', '2026-11-22', 2, 'Ending my first trip in Lisbon, a city full of surprises and new experiences.'),
(99, 17, 49, '2025-01-03', '2025-01-07', 1, 'A couple''s trip with a third adventurer to Iguazu Falls, a truly breathtaking experience.'),
(100, 2, 49, '2025-01-08', '2025-01-14', 2, 'Concluding our trip at the Statue of Liberty, a symbol of adventure and freedom.'),
(101, 1, 50, '2027-03-19', '2027-03-24', 1, 'A foodie tour starting in Paris, with hidden gems and delightful surprises.'),
(102, 29, 50, '2027-03-25', '2027-04-02', 2, 'Exploring the Louvre Museum on our foodie tour, a feast for both the eyes and the palate.'),
(103, 9, 51, '2027-04-17', '2027-04-26', 1, 'A spontaneous couples getaway to Santorini, a perfect place for flexible plans and beautiful views.'),
(104, 21, 51, '2027-04-27', '2027-05-06', 2, 'A beautiful ending to our trip in the stunning Reykjavík Blue Lagoon.'),
(105, 11, 52, '2027-12-03', '2027-12-09', 1, 'Winter wanderlust starting with a visit to Table Mountain, enjoying the hot drinks and cold air.'),
(106, 25, 52, '2027-12-10', '2027-12-15', 2, 'A cozy end to our trip in the beautiful snowy town of Zermatt.'),
(107, 13, 53, '2027-11-08', '2027-11-13', 1, 'A cultural deep dive starting with the ancient wonders of Angkor Wat.'),
(108, 1, 53, '2027-11-14', '2027-11-20', 2, 'Ending our journey in Paris, with local guides and off-the-map adventures near the Eiffel Tower.');

-- =======================
-- buddy rows
-- =======================
INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, note, request_status) VALUES
(1, 7, 1, 4, 'Vegetarian—open to food suggestions.', 'rejected'),
(2, 24, 1, 2, 'Happy to share accommodation.', 'accepted'),
(3, 10, 2, 3, 'Traveling with kids, hope that\'s okay.', 'accepted'),
(4, 15, 3, 4, 'Prefer early starts.', 'rejected'),
(5, 7, 3, 5, 'Prefer early starts.', 'rejected'),
(6, 6, 3, 1, 'Happy to share accommodation.', 'rejected'),
(7, 3, 4, 2, 'Happy to share accommodation.', 'pending'),
(8, 26, 4, 2, 'Can bring extra gear if needed.', 'pending'),
(9, 2, 4, 2, 'Prefer early starts.', 'rejected'),
(10, 15, 5, 4, 'Traveling with kids, hope that\'s okay.', 'rejected'),
(11, 16, 6, 4, 'Traveling with kids, hope that\'s okay.', 'pending'),
(12, 21, 6, 1, 'Looking forward to it.', 'accepted'),
(13, 8, 6, 2, 'Prefer early starts.', 'pending'),
(14, 18, 6, 2, 'Looking forward to it.', 'accepted'),
(15, 26, 7, 4, 'Flexible with dates and budget.', 'accepted'),
(16, 20, 7, 5, 'Vegetarian—open to food suggestions.', 'rejected'),
(17, 29, 8, 2, 'Prefer early starts.', 'accepted'),
(18, 9, 8, 2, 'Happy to share accommodation.', 'rejected'),
(19, 16, 8, 2, 'Happy to share accommodation.', 'accepted'),
(20, 3, 8, 3, 'Traveling with kids, hope that\'s okay.', 'accepted'),
(21, 11, 9, 5, 'Looking forward to it.', 'pending'),
(22, 5, 9, 2, 'Vegetarian—open to food suggestions.', 'rejected'),
(23, 5, 9, 2, 'Looking forward to it.', 'accepted'),
(24, 11, 10, 5, 'Prefer early starts.', 'accepted'),
(25, 2, 10, 2, 'Vegetarian—open to food suggestions.', 'accepted'),
(26, 29, 10, 5, 'Excited to join!', 'rejected'),
(27, 13, 10, 4, 'Excited to join!', 'accepted'),
(28, 25, 11, 4, 'Vegetarian—open to food suggestions.', 'rejected'),
(29, 24, 11, 5, 'Traveling with kids, hope that\'s okay.', 'accepted'),
(30, 8, 11, 3, 'Vegetarian—open to food suggestions.', 'accepted'),
(31, 13, 12, 3, 'Vegetarian—open to food suggestions.', 'rejected'),
(32, 27, 13, 4, 'Can bring extra gear if needed.', 'rejected'),
(33, 18, 13, 1, 'Vegetarian—open to food suggestions.', 'rejected'),
(34, 3, 14, 4, 'Can bring extra gear if needed.', 'accepted'),
(35, 2, 15, 3, 'Vegetarian—open to food suggestions.', 'accepted'),
(36, 7, 15, 4, 'Flexible with dates and budget.', 'accepted'),
(37, 9, 16, 4, 'Happy to share accommodation.', 'pending'),
(38, 16, 16, 1, 'Excited to join!', 'accepted'),
(39, 8, 16, 1, 'Excited to join!', 'pending'),
(40, 8, 16, 2, 'Excited to join!', 'rejected'),
(41, 8, 17, 2, 'Prefer early starts.', 'rejected'),
(42, 4, 17, 5, 'Traveling with kids, hope that\'s okay.', 'accepted'),
(43, 25, 18, 3, 'Can bring extra gear if needed.', 'rejected'),
(44, 20, 18, 1, 'Can bring extra gear if needed.', 'accepted'),
(45, 4, 18, 5, 'Excited to join!', 'accepted'),
(46, 13, 19, 2, 'Looking forward to it.', 'rejected'),
(47, 23, 19, 2, 'Looking forward to it.', 'rejected'),
(48, 25, 19, 3, 'Looking forward to it.', 'rejected'),
(49, 26, 19, 1, 'Flexible with dates and budget.', 'rejected'),
(50, 22, 20, 3, 'Looking forward to it.', 'rejected'),
(51, 21, 20, 3, 'Excited to join!', 'accepted'),
(52, 27, 20, 4, 'Looking forward to it.', 'accepted'),
(53, 12, 20, 4, 'Can bring extra gear if needed.', 'accepted'),
(54, 24, 21, 5, 'Happy to share accommodation.', 'rejected'),
(55, 26, 21, 5, 'Prefer early starts.', 'accepted'),
(56, 27, 22, 5, 'Happy to share accommodation.', 'accepted'),
(57, 28, 22, 2, 'Looking forward to it.', 'accepted'),
(58, 29, 22, 4, 'Traveling with kids, hope that\'s okay.', 'accepted'),
(59, 19, 22, 5, 'Vegetarian—open to food suggestions.', 'accepted'),
(60, 16, 23, 3, 'Can bring extra gear if needed.', 'accepted'),
(61, 12, 24, 3, 'Flexible with dates and budget.', 'accepted'),
(62, 29, 24, 5, 'Happy to share accommodation.', 'rejected'),
(63, 17, 25, 2, 'Looking forward to it.', 'pending'),
(64, 16, 26, 5, 'Traveling with kids, hope that\'s okay.', 'rejected'),
(65, 16, 26, 4, 'Prefer early starts.', 'pending'),
(66, 3, 26, 3, 'Traveling with kids, hope that\'s okay.', 'accepted'),
(67, 23, 26, 2, 'Happy to share accommodation.', 'rejected'),
(68, 16, 27, 5, 'Flexible with dates and budget.', 'accepted'),
(69, 24, 27, 5, 'Flexible with dates and budget.', 'accepted'),
(70, 23, 27, 4, 'Happy to share accommodation.', 'accepted'),
(71, 8, 28, 1, 'Traveling with kids, hope that\'s okay.', 'accepted'),
(72, 4, 28, 5, 'Can bring extra gear if needed.', 'pending'),
(73, 7, 28, 4, 'Happy to share accommodation.', 'rejected'),
(74, 4, 29, 2, 'Happy to share accommodation.', 'pending'),
(75, 12, 29, 2, 'Happy to share accommodation.', 'pending'),
(76, 23, 29, 5, 'Can bring extra gear if needed.', 'accepted'),
(77, 2, 30, 5, 'Happy to share accommodation.', 'rejected'),
(80, 11, 32, 2, 'Excited to join!', 'accepted'),
(81, 28, 32, 4, 'Looking forward to it.', 'pending'),
(82, 13, 32, 4, 'Looking forward to it.', 'rejected'),
(83, 21, 32, 1, 'Can bring extra gear if needed.', 'pending'),
(84, 3, 33, 2, 'Looking forward to it.', 'rejected'),
(85, 25, 33, 4, 'Traveling with kids, hope that\'s okay.', 'rejected'),
(86, 13, 33, 4, 'Prefer early starts.', 'accepted'),
(87, 10, 34, 5, 'Excited to join!', 'rejected'),
(88, 24, 34, 1, 'Traveling with kids, hope that\'s okay.', 'rejected'),
(89, 7, 34, 3, 'Looking forward to it.', 'pending'),
(90, 8, 34, 2, 'Looking forward to it.', 'pending'),
(91, 14, 35, 4, 'Prefer early starts.', 'accepted'),
(92, 8, 36, 3, 'Happy to share accommodation.', 'rejected'),
(93, 3, 37, 2, 'Happy to share accommodation.', 'rejected'),
(94, 19, 37, 2, 'Vegetarian—open to food suggestions.', 'pending'),
(95, 18, 37, 2, 'Can bring extra gear if needed.', 'accepted'),
(96, 27, 37, 2, 'Looking forward to it.', 'pending'),
(97, 26, 38, 3, 'Happy to share accommodation.', 'accepted'),
(98, 4, 38, 4, 'Happy to share accommodation.', 'rejected'),
(99, 9, 39, 5, 'Prefer early starts.', 'accepted'),
(100, 3, 39, 5, 'Excited to join!', 'accepted'),
(101, 24, 39, 3, 'Happy to share accommodation.', 'pending'),
(102, 3, 39, 2, 'Excited to join!', 'rejected'),
(103, 19, 40, 1, 'Can bring extra gear if needed.', 'accepted'),
(104, 17, 40, 4, 'Happy to share accommodation.', 'pending'),
(105, 19, 40, 4, 'Prefer early starts.', 'pending'),
(106, 12, 41, 4, 'Flexible with dates and budget.', 'accepted'),
(107, 22, 41, 1, 'Can bring extra gear if needed.', 'accepted'),
(108, 14, 41, 4, 'Happy to share accommodation.', 'rejected'),
(109, 13, 41, 5, 'Excited to join!', 'accepted'),
(110, 11, 42, 3, 'Flexible with dates and budget.', 'pending'),
(111, 28, 43, 5, 'Excited to join!', 'rejected'),
(112, 28, 43, 5, 'Prefer early starts.', 'accepted'),
(113, 2, 43, 2, 'Flexible with dates and budget.', 'rejected'),
(114, 25, 43, 4, 'Prefer early starts.', 'pending'),
(115, 9, 44, 5, 'Can bring extra gear if needed.', 'accepted'),
(116, 15, 44, 4, 'Looking forward to it.', 'pending'),
(117, 23, 45, 2, 'Happy to share accommodation.', 'rejected'),
(118, 1, 45, 5, 'Vegetarian—open to food suggestions.', 'pending'),
(119, 27, 46, 1, 'Prefer early starts.', 'pending'),
(120, 21, 46, 2, 'Prefer early starts.', 'rejected'),
(121, 17, 47, 3, 'Vegetarian—open to food suggestions.', 'accepted'),
(122, 16, 47, 2, 'Prefer early starts.', 'rejected'),
(123, 5, 47, 4, 'Traveling with kids, hope that\'s okay.', 'rejected'),
(124, 28, 48, 1, 'Happy to share accommodation.', 'accepted'),
(125, 11, 48, 5, 'Happy to share accommodation.', 'pending'),
(126, 24, 49, 3, 'Prefer early starts.', 'pending'),
(127, 15, 49, 5, 'Prefer early starts.', 'accepted'),
(128, 11, 49, 5, 'Vegetarian—open to food suggestions.', 'accepted'),
(129, 28, 50, 2, 'Traveling with kids, hope that\'s okay.', 'rejected'),
(130, 13, 50, 2, 'Vegetarian—open to food suggestions.', 'pending'),
(131, 11, 50, 4, 'Vegetarian—open to food suggestions.', 'accepted'),
(132, 16, 51, 1, 'Can bring extra gear if needed.', 'rejected'),
(133, 19, 51, 3, 'Looking forward to it.', 'accepted'),
(134, 17, 52, 4, 'Excited to join!', 'rejected'),
(135, 14, 53, 2, 'Looking forward to it.', 'accepted'),
(136, 26, 53, 3, 'Flexible with dates and budget.', 'rejected'),
(137, 21, 54, 1, 'Flexible with dates and budget.', 'rejected'),
(138, 28, 54, 5, 'Vegetarian—open to food suggestions.', 'accepted'),
(139, 21, 54, 4, 'Excited to join!', 'rejected'),
(140, 3, 54, 2, 'Happy to share accommodation.', 'pending'),
(141, 14, 55, 1, 'Looking forward to it.', 'accepted'),
(142, 23, 56, 3, 'Excited to join!', 'pending'),
(143, 11, 56, 1, 'Happy to share accommodation.', 'accepted'),
(144, 14, 57, 2, 'Traveling with kids, hope that\'s okay.', 'rejected'),
(145, 14, 57, 5, 'Can bring extra gear if needed.', 'pending'),
(146, 6, 57, 1, 'Vegetarian—open to food suggestions.', 'rejected'),
(147, 16, 58, 5, 'Can bring extra gear if needed.', 'pending'),
(148, 15, 58, 3, 'Prefer early starts.', 'accepted'),
(149, 29, 59, 4, 'Happy to share accommodation.', 'rejected'),
(150, 3, 60, 4, 'Flexible with dates and budget.', 'rejected'),
(151, 10, 60, 4, 'Happy to share accommodation.', 'accepted'),
(152, 7, 61, 4, 'Prefer early starts.', 'pending'),
(153, 8, 61, 4, 'Flexible with dates and budget.', 'rejected'),
(154, 10, 61, 3, 'Excited to join!', 'rejected'),
(155, 9, 62, 1, 'Excited to join!', 'rejected'),
(156, 24, 62, 4, 'Happy to share accommodation.', 'pending'),
(157, 20, 62, 3, 'Traveling with kids, hope that\'s okay.', 'rejected'),
(158, 7, 62, 5, 'Happy to share accommodation.', 'rejected'),
(159, 21, 63, 1, 'Excited to join!', 'accepted'),
(160, 26, 63, 4, 'Excited to join!', 'rejected'),
(161, 24, 64, 2, 'Looking forward to it.', 'accepted'),
(162, 11, 64, 4, 'Can bring extra gear if needed.', 'pending'),
(163, 5, 64, 5, 'Flexible with dates and budget.', 'rejected'),
(164, 27, 65, 2, 'Happy to share accommodation.', 'accepted'),
(165, 26, 65, 3, 'Flexible with dates and budget.', 'pending'),
(166, 15, 65, 1, 'Can bring extra gear if needed.', 'pending'),
(167, 28, 66, 5, 'Flexible with dates and budget.', 'pending'),
(168, 26, 66, 4, 'Excited to join!', 'accepted'),
(169, 18, 66, 1, 'Prefer early starts.', 'accepted'),
(170, 22, 66, 3, 'Vegetarian—open to food suggestions.', 'rejected'),
(171, 4, 67, 2, 'Prefer early starts.', 'pending'),
(172, 20, 67, 5, 'Flexible with dates and budget.', 'rejected'),
(173, 8, 67, 1, 'Prefer early starts.', 'rejected'),
(174, 21, 68, 4, 'Looking forward to it.', 'pending'),
(175, 2, 68, 1, 'Happy to share accommodation.', 'accepted'),
(176, 4, 68, 1, 'Traveling with kids, hope that\'s okay.', 'rejected'),
(177, 13, 69, 4, 'Flexible with dates and budget.', 'rejected'),
(178, 24, 69, 5, 'Vegetarian—open to food suggestions.', 'rejected'),
(179, 29, 70, 4, 'Looking forward to it.', 'accepted'),
(180, 20, 70, 4, 'Happy to share accommodation.', 'pending'),
(181, 7, 71, 4, 'Prefer early starts.', 'pending'),
(182, 28, 71, 3, 'Looking forward to it.', 'rejected'),
(183, 12, 71, 5, 'Flexible with dates and budget.', 'pending'),
(184, 9, 72, 2, 'Looking forward to it.', 'accepted'),
(185, 3, 72, 2, 'Excited to join!', 'pending'),
(186, 26, 72, 3, 'Traveling with kids, hope that\'s okay.', 'pending'),
(187, 26, 72, 5, 'Traveling with kids, hope that\'s okay.', 'pending'),
(188, 19, 73, 2, 'Traveling with kids, hope that\'s okay.', 'accepted'),
(189, 25, 73, 2, 'Excited to join!', 'accepted'),
(190, 5, 74, 5, 'Happy to share accommodation.', 'pending'),
(191, 4, 74, 1, 'Can bring extra gear if needed.', 'pending'),
(192, 26, 75, 2, 'Flexible with dates and budget.', 'pending'),
(193, 6, 75, 3, 'Excited to join!', 'pending'),
(194, 24, 75, 4, 'Looking forward to it.', 'rejected'),
(195, 16, 76, 4, 'Flexible with dates and budget.', 'rejected'),
(196, 15, 77, 5, 'Traveling with kids, hope that\'s okay.', 'rejected'),
(197, 24, 78, 5, 'Happy to share accommodation.', 'accepted'),
(198, 2, 79, 4, 'Vegetarian—open to food suggestions.', 'accepted'),
(199, 16, 80, 4, 'Looking forward to it.', 'pending'),
(200, 20, 81, 2, 'Looking forward to it.', 'pending'),
(201, 9, 81, 5, 'Flexible with dates and budget.', 'accepted'),
(202, 20, 81, 5, 'Happy to share accommodation.', 'accepted'),
(203, 4, 82, 1, 'Traveling with kids, hope that\'s okay.', 'accepted'),
(204, 15, 82, 2, 'Vegetarian—open to food suggestions.', 'accepted'),
(205, 27, 82, 4, 'Vegetarian—open to food suggestions.', 'accepted'),
(206, 24, 82, 1, 'Flexible with dates and budget.', 'accepted'),
(207, 22, 83, 3, 'Flexible with dates and budget.', 'pending'),
(208, 22, 83, 4, 'Looking forward to it.', 'pending'),
(209, 27, 83, 1, 'Looking forward to it.', 'accepted'),
(210, 24, 84, 3, 'Can bring extra gear if needed.', 'rejected'),
(211, 19, 85, 5, 'Flexible with dates and budget.', 'rejected'),
(212, 14, 86, 3, 'Vegetarian—open to food suggestions.', 'rejected'),
(213, 10, 87, 5, 'Happy to share accommodation.', 'accepted'),
(214, 19, 88, 5, 'Traveling with kids, hope that\'s okay.', 'pending'),
(215, 8, 89, 1, 'Flexible with dates and budget.', 'rejected'),
(216, 12, 89, 1, 'Happy to share accommodation.', 'rejected'),
(217, 8, 89, 4, 'Excited to join!', 'rejected'),
(218, 30, 89, 3, 'Excited to join!', 'pending'),
(219, 23, 90, 3, 'Flexible with dates and budget.', 'accepted'),
(220, 1, 90, 2, 'Can bring extra gear if needed.', 'rejected'),
(221, 22, 90, 4, 'Looking forward to it.', 'pending'),
(222, 3, 91, 5, 'Traveling with kids, hope that\'s okay.', 'accepted'),
(223, 15, 92, 3, 'Can bring extra gear if needed.', 'accepted'),
(224, 10, 92, 3, 'Looking forward to it.', 'pending'),
(225, 5, 92, 2, 'Excited to join!', 'rejected'),
(226, 3, 92, 3, 'Prefer early starts.', 'rejected'),
(227, 16, 93, 5, 'Prefer early starts.', 'accepted'),
(228, 9, 93, 2, 'Looking forward to it.', 'accepted'),
(229, 14, 93, 1, 'Happy to share accommodation.', 'rejected'),
(230, 22, 93, 5, 'Prefer early starts.', 'rejected'),
(231, 2, 94, 2, 'Vegetarian—open to food suggestions.', 'rejected'),
(232, 2, 94, 1, 'Traveling with kids, hope that\'s okay.', 'accepted'),
(233, 7, 94, 2, 'Happy to share accommodation.', 'accepted'),
(234, 4, 95, 1, 'Prefer early starts.', 'rejected'),
(235, 14, 95, 2, 'Can bring extra gear if needed.', 'accepted'),
(236, 18, 95, 2, 'Flexible with dates and budget.', 'pending'),
(237, 28, 96, 1, 'Vegetarian—open to food suggestions.', 'pending'),
(238, 15, 96, 1, 'Flexible with dates and budget.', 'rejected'),
(239, 14, 96, 5, 'Vegetarian—open to food suggestions.', 'rejected'),
(240, 21, 96, 4, 'Happy to share accommodation.', 'pending'),
(241, 1, 97, 3, 'Can bring extra gear if needed.', 'rejected'),
(242, 15, 97, 3, 'Looking forward to it.', 'accepted'),
(243, 28, 97, 1, 'Traveling with kids, hope that\'s okay.', 'accepted'),
(244, 19, 97, 4, 'Looking forward to it.', 'accepted'),
(245, 24, 98, 3, 'Traveling with kids, hope that\'s okay.', 'accepted'),
(246, 25, 98, 2, 'Looking forward to it.', 'rejected'),
(247, 21, 98, 1, 'Traveling with kids, hope that\'s okay.', 'accepted'),
(248, 24, 99, 2, 'Traveling with kids, hope that\'s okay.', 'pending'),
(249, 5, 99, 3, 'Traveling with kids, hope that\'s okay.', 'pending'),
(250, 20, 99, 2, 'Looking forward to it.', 'pending'),
(251, 15, 100, 5, 'Prefer early starts.', 'rejected'),
(252, 30, 100, 5, 'Flexible with dates and budget.', 'rejected'),
(253, 11, 100, 2, 'Prefer early starts.', 'pending'),
(254, 16, 100, 4, 'Happy to share accommodation.', 'accepted'),
(255, 12, 101, 5, 'Looking forward to it.', 'accepted'),
(256, 15, 102, 1, 'Excited to join!', 'accepted'),
(257, 27, 102, 3, 'Looking forward to it.', 'rejected'),
(258, 28, 102, 1, 'Vegetarian—open to food suggestions.', 'accepted'),
(259, 19, 102, 5, 'Excited to join!', 'accepted'),
(260, 11, 103, 5, 'Prefer early starts.', 'rejected'),
(261, 5, 103, 1, 'Prefer early starts.', 'pending'),
(262, 23, 104, 1, 'Can bring extra gear if needed.', 'pending'),
(263, 8, 104, 4, 'Prefer early starts.', 'rejected'),
(264, 17, 104, 5, 'Can bring extra gear if needed.', 'accepted'),
(265, 30, 105, 3, 'Vegetarian—open to food suggestions.', 'accepted'),
(266, 25, 105, 3, 'Excited to join!', 'rejected'),
(267, 21, 105, 3, 'Looking forward to it.', 'accepted'),
(268, 18, 106, 4, 'Happy to share accommodation.', 'accepted'),
(269, 11, 107, 1, 'Can bring extra gear if needed.', 'accepted'),
(270, 10, 107, 4, 'Can bring extra gear if needed.', 'rejected'),
(271, 10, 108, 5, 'Vegetarian—open to food suggestions.', 'rejected');

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

-- Convo 6: Trip to Machu Picchu (TD 6, Owner: ID 3, Buddy: ID 13)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (6, 6, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (6, 3), (6, 13);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(3, 'So excited for our Machu Picchu adventure! I''m already dreaming about chasing sunsets on the cliffs.', '2024-05-15 10:00:00', 6),
(13, 'I know! It''s going to be a dreamlike experience for any thrill-seeker. I can''t wait!', '2024-05-15 10:01:00', 6),
(3, 'We''ll need to find a spot for a perfect photo op.', '2024-05-15 10:05:00', 6);

-- Convo 7: Trip to Queenstown (TD 7, Owner: ID 4, Buddy: ID 15)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (7, 7, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (7, 4), (7, 15);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(4, 'Hi! Just wanted to touch base about our Queenstown family adventure. Everything still good to go on your end?', '2024-05-16 11:30:00', 7),
(15, 'Yep! The kids are so excited about the stunning landscapes and endless fun.', '2024-05-16 11:32:00', 7),
(4, 'Awesome! It''s going to be so much fun.', '2024-05-16 11:35:00', 7);

-- Convo 8: Trip to Hallstatt (TD 8, Owner: ID 4, Buddy: ID 16)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (8, 8, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (8, 4), (8, 16);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(4, 'Hey there! Have you started looking for family-friendly activities in Hallstatt yet?', '2024-05-17 10:00:00', 8),
(16, 'Not yet, I''ve been so busy. But I''m looking forward to the picturesque village and finding some peace and tradition.', '2024-05-17 10:01:00', 8),
(4, 'Me too. It''s going to be a perfect family retreat.', '2024-05-17 10:05:00', 8);

-- Convo 9: Trip to Dubai (TD 9, Owner: ID 5, Buddy: ID 1)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (9, 9, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (9, 5), (9, 1);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(1, 'Hi! I''m so happy about our romantic escape to Dubai. The Burj Khalifa as a backdrop is going to be amazing!', '2024-05-18 15:00:00', 9),
(5, 'I know, right? It''s going to be incredible. The city is so stunning.', '2024-05-18 15:02:00', 9),
(1, 'Totally agree! It''s going to be so romantic.', '2024-05-18 15:05:00', 9);

-- Convo 10: Trip to Grand Canyon (TD 10, Owner: ID 5, Buddy: ID 2)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (10, 10, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (10, 5), (10, 2);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(2, 'Hey, looking forward to the Grand Canyon! Have you decided on a spot to watch the sunset?', '2024-05-19 09:30:00', 10),
(5, 'I''ve been looking at a few spots, but I think the breathtaking awe will be everywhere.', '2024-05-19 09:32:00', 10),
(2, 'Sounds good. This trip is going to be truly memorable.', '2024-05-19 09:35:00', 10);

-- =====================================
-- 3. Group Conversations (Trip-Specific)
-- =====================================

-- Convo 11: Trip to Sydney (TD 11, Owner: ID 6, Buddies: ID 11, 12)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (11, 11, TRUE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (11, 6), (11, 11), (11, 12);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(6, 'Welcome everyone to the Sydney trip chat! We can coordinate our spontaneous romantic outing here.', '2024-05-20 10:00:00', 11),
(11, 'Hey guys! Looking forward to experiencing the Sydney Opera House with a loved one.', '2024-05-20 10:01:00', 11),
(12, 'Hi all! I''ve already started a list of all the best spots to grab a romantic bite.', '2024-05-20 10:02:00', 11),
(6, 'Perfect, let''s start a document with that list!', '2024-05-20 10:05:00', 11);

-- Convo 12: Trip to Uluru (TD 12, Owner: ID 7, Buddies: ID 13, 14)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (12, 12, TRUE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (12, 7), (12, 13), (12, 14);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(7, 'Hey team, welcome to the Uluru trip chat! We can coordinate plans here.', '2024-05-21 11:00:00', 12),
(13, 'Sounds good. I''m definitely up for connecting with nature in this sacred place.', '2024-05-21 11:02:00', 12),
(14, 'Count me in for finding peace! I also found a few good trails we could visit.', '2024-05-21 11:04:00', 12);

-- Convo 13: Trip to Petra (TD 13, Owner: ID 7, Buddies: ID 15, 16)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (13, 13, TRUE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (13, 7), (13, 15), (13, 16);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(7, 'Alright everyone, our spiritual journey to Petra is a go! Let''s start planning some spots to visit.', '2024-05-22 13:00:00', 13),
(15, 'Yes! First stop: finding tranquility and wonder in this ancient city!', '2024-05-22 13:01:00', 13),
(16, 'I second that! And maybe a local guide to tell us about the history?', '2024-05-22 13:03:00', 13),
(7, 'Sounds like a great first day plan to me!', '2024-05-22 13:05:00', 13);

-- Convo 14: Trip to Lisbon (TD 14, Owner: ID 7, Buddy: ID 17) - Group chat of two
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (14, 14, TRUE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (14, 7), (14, 17);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(7, 'Hey, just wanted to make this the official group chat for our Christmas in Lisbon trip!', '2024-05-23 09:00:00', 14),
(17, 'Sounds good! I''m ready to enjoy the festive atmosphere and rich history of Alfama.', '2024-05-23 09:01:00', 14),
(7, 'Agreed. Let''s get planning!', '2024-05-23 09:02:00', 14);

-- Convo 15: Trip to Queenstown (TD 15, Owner: ID 8, Buddy: ID 18) - Group chat of two
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (15, 15, TRUE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (15, 8), (15, 18);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(8, 'Welcome to the Queenstown solo trek group chat!', '2024-05-24 14:00:00', 15),
(18, 'Excited to be here! Can''t wait to find a new discovery in this stunning town.', '2024-05-24 14:02:00', 15),
(8, 'Perfect! Can''t wait to see your photos.', '2024-05-24 14:03:00', 15);

