USE travel_buddy;

-- =======================
-- user rows
-- =======================

INSERT INTO user (user_id, name, email, password_hash, birthdate) VALUES
(1, 'Allison Hill', 'allyhill95@gmail.com', 'pOuYQCyS06RWqXSfdoz7PA==.zKUHgXV44DoKsF4UcjZ5+ohWiIFba/eT0cAautQLUSY=', '1995-09-05'),
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
(30, 'Allison Doyle', 'allydoyle02@doyledesigns.net', '9f945d9f0b5f0d334f915932aef318a81eca9828a238189e1d62a5b9b2e95380', '2002-06-07'),
(31, 'Caleb Evans', 'caleb.evans@gmail.com', 'a1e948f2c3b5d7e8f0a1b2c3d4e5f6a1e948f2c3b5d7e8f0a1b2c3d4e5f6a1e9', '2001-08-15'),
(32, 'Olivia Green', 'oliviagreen2003@yahoo.com', 'b2c159a7e3d8c4f0b1a2c3d4e5f6b7c8d9e0f1a2c3b4d5e6f7a8b9c0d1e2f3a4', '2003-02-01'),
(33, 'Liam Scott', 'liam.scott@protonmail.com', 'c3d260b8f4a9d5e1c2b3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0u1v2w3x4y5', '1999-11-28'),
(34, 'Mia Carter', 'mia.carter@outlook.com', 'd4e371c9g5b0e6f2d3c4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0u1v2w3x4y5z6', '2004-04-19'),
(35, 'Ethan Miller', 'ethanmiller@hotmail.com', 'e5f482d0h6c1f7g3e4d5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0u1v2w3x4y5z6a7', '2005-07-22'),
(36, 'Ava Wilson', 'avawilson98@gmail.com', 'f6g593e1i7d2g8h4f5e6g7h8i9j0k1l2m3n4o5p6q7r8s9t0u1v2w3x4y5z6a7b8', '1998-08-08'),
(37, 'Noah Turner', 'noah.t@yahoo.com', '0e7d64f2j8e3h9i5g6f7g8h9i0j1k2l3m4n5o6p7q8r9s0t1u2v3w4x5y6z7a8c9', '2000-01-03'),
(38, 'Sophia Baker', 'sophiabaker@gmail.com', '1f8e75g3k9f4i0j6h7g8h9i0j1k2l3m4n5o6p7q8r9s0t1u2v3w4x5y6z7a8c9d0', '2002-05-11'),
(39, 'Jackson Hill', 'jackson.h@breezemail.net', '2g9f86h4l0g5j1k7i8h9i0j1k2l3m4n5o6p7q8r9s0t1u2v3w4x5y6z7a8c9d0e1', '1996-10-25'),
(40, 'Emma Lopez', 'emmalopez2001@gmail.com', '3h0g97i5m1h6k2l8j9i0j1k2l3m4n5o6p7q8r9s0t1u2v3w4x5y6z7a8c9d0e1f2', '2001-03-29'),
(41, 'Lucas King', 'lucasking@gmail.com', '4i1h08j6n2i7l3m9k0j1k2l3m4n5o6p7q8r9s0t1u2v3w4x5y6z7a8c9d0e1f2g3', '1997-07-14'),
(42, 'Chloe Adams', 'chloe.adams@outlook.com', '5j2i19k7o3j8m4n0l1k2l3m4n5o6p7q8r9s0t1u2v3w4x5y6z7a8c9d0e1f2g3h4', '2003-09-17'),
(43, 'Mason Clark', 'mason.c@gmail.com', '6k3j20l8p4k9n5o1m2l3m4n5o6p7q8r9s0t1u2v3w4x5y6z7a8c9d0e1f2g3h4i5', '2000-01-08'),
(44, 'Ella Robinson', 'ella.r@gmail.com', '7l4k31m9q5l0o6p2n3m4n5o6p7q8r9s0t1u2v3w4x5y6z7a8c9d0e1f2g3h4i5j6', '2005-11-23'),
(45, 'Aiden Wright', 'aiden.w@yahoo.com', '8m5l42n0r6m1p7q3o4n5o6p7q8r9s0t1u2v3w4x5y6z7a8c9d0e1f2g3h4i5j6k7', '2002-06-18'),
(46, 'Grace Lewis', 'grace.lewis@gmail.com', '9n6m53o1s7n2q8r4p5o6p7q8r9s0t1u2v3w4x5y6z7a8c9d0e1f2g3h4i5j6k7l8', '1997-02-09'),
(47, 'Jacob Hall', 'jacob.h@live.com', '0o7n64p2t8o3r9s5q6p7q8r9s0t1u2v3w4x5y6z7a8c9d0e1f2g3h4i5j6k7l8m9', '2004-04-12'),
(48, 'Lily Scott', 'lily.s@yahoo.com', '1p8o75q3u9p4s0t6r7q8r9s0t1u2v3w4x5y6z7a8c9d0e1f2g3h4i5j6k7l8m9n0', '2000-07-26'),
(49, 'Samuel King', 'samuel.k@gmail.com', '2q9p86r4v0q5t1u7s8r9s0t1u2v3w4x5y6z7a8c9d0e1f2g3h4i5j6k7l8m9n0o1', '1999-08-30'),
(50, 'Zoe Ramirez', 'zoe.r@gmail.com', '3r0q97s5w1r6u2v8t9s0t1u2v3w4x5y6z7a8c9d0e1f2g3h4i5j6k7l8m9n0o1p2', '2002-03-05'),
(51, 'Daniel Evans', 'daniel.e@yahoo.com', '4s1r08t6x2s7v3w9u0t1u2v3w4x5y6z7a8c9d0e1f2g3h4i5j6k7l8m9n0o1p2q3', '2005-05-18'),
(52, 'Sophie Reed', 'sophie.reed@gmail.com', '5t2s19u7y3t8w4x0v1u2v3w4x5y6z7a8c9d0e1f2g3h4i5j6k7l8m9n0o1p2q3r4', '2003-06-10'),
(53, 'Andrew Phillips', 'andrew.p@gmail.com', '6u3t20v8z4u9x5y1w2v3w4x5y6z7a8c9d0e1f2g3h4i5j6k7l8m9n0o1p2q3r4s5', '1998-02-02'),
(54, 'Lauren Baker', 'lauren.b@hotmail.com', '7v4u31w9a5v0y6z2x3w4x5y6z7a8c9d0e1f2g3h4i5j6k7l8m9n0o1p2q3r4s5t6', '2001-09-07'),
(55, 'Jack Murphy', 'jackmurphy@gmail.com', '8w5v42x0b6w1z7a3y4x5y6z7a8c9d0e1f2g3h4i5j6k7l8m9n0o1p2q3r4s5t6u7', '1996-03-24'),
(56, 'Scarlett Gray', 'scarlett.g@gmail.com', '9x6w53y1c7x2a8b4z5y6z7a8c9d0e1f2g3h4i5j6k7l8m9n0o1p2q3r4s5t6u7v8', '2003-12-19'),
(57, 'Adam Cooper', 'adam.cooper@yahoo.com', '0y7x64z2d8y3b9c5a6z7a8c9d0e1f2g3h4i5j6k7l8m9n0o1p2q3r4s5t6u7v8w9', '1999-04-14'),
(58, 'Isabella Henderson', 'isabella.h@outlook.com', '1z8y75a3e9z4c0d6b7a8c9d0e1f2g3h4i5j6k7l8m9n0o1p2q3r4s5t6u7v8w9x0', '2005-06-21'),
(59, 'William Sanchez', 'william.s@gmail.com', '2a9z86b4f0a5d1e7c8a9c0d1e2f3g4h5i6j7k8l9m0n1o2p3q4r5s6t7u8v9w0x1', '1997-07-28'),
(60, 'Grace Powell', 'grace.powell@yahoo.com', '3b0a97c5g1b6e2f8d9a0d1e2f3g4h5i6j7k8l9m0n1o2p3q4r5s6t7u8v9w0x1y2', '2002-08-01'),
(61, 'Julian White', 'julian.white@gmail.com', '4c1b08d6h2c7f3g9e0b1e2f3g4h5i6j7k8l9m0n1o2p3q4r5s6t7u8v9w0x1y2z3', '2000-10-09'),
(62, 'Hannah Morgan', 'hannah.morgan@gmail.com', '5d2c19e7i3d8g4h0f1c2f3g4h5i6j7k8l9m0n1o2p3q4r5s6t7u8v9w0x1y2z3a4', '2004-05-23'),
(63, 'Benjamin Carter', 'benjamin.c@gmail.com', '6e3d20f8j4e9h5i1g2d3g4h5i6j7k8l9m0n1o2p3q4r5s6t7u8v9w0x1y2z3a4b5', '1998-01-16'),
(64, 'Chloe Collins', 'chloe.collins@outlook.com', '7f4e31g9k5f0i6j2h3e4h5i6j7k8l9m0n1o2p3q4r5s6t7u8v9w0x1y2z3a4b5c6', '2002-06-25'),
(65, 'Leo Peterson', 'leopeterson@gmail.com', '8g5f42h0l6g1j7k3i4f5i6j7k8l9m0n1o2p3q4r5s6t7u8v9w0x1y2z3a4b5c6d7', '2003-11-29'),
(66, 'Madison Murphy', 'madisonmurphy@yahoo.com', '9h6g53i1m7h2k8l4j5g6j7k8l9m0n1o2p3q4r5s6t7u8v9w0x1y2z3a4b5c6d7e8', '1999-07-04'),
(67, 'Elijah Bailey', 'elijah.b@gmail.com', '0i7h64j2n8i3l9m5k6h7k8l9m0n1o2p3q4r5s6t7u8v9w0x1y2z3a4b5c6d7e8f9', '2001-04-18'),
(68, 'Avery Cooper', 'avery.c@hotmail.com', '1j8i75k3o9j4m0n6l7i8l9m0n1o2p3q4r5s6t7u8v9w0x1y2z3a4b5c6d7e8f9g0', '2005-09-02'),
(69, 'Tyler Henderson', 'tyler.h@yahoo.com', '2k9j86l4p0k5n1o7m8j9n0o1p2q3r4s5t6u7v8w9x0y1z2a3b4c5d6e7f8g9h0i1', '1996-03-26'),
(70, 'Victoria Coleman', 'victoriacoleman@gmail.com', '3l0k97m5q1l6o2p8n9k0o1p2q3r4s5t6u7v8w9x0y1z2a3b4c5d6e7f8g9h0i1j2', '2000-12-30'),
(71, 'Gabriel Ramirez', 'gabriel.r@live.com', '4m1l08n6r2m7p3q9o0l1p2q3r4s5t6u7v8w9x0y1z2a3b4c5d6e7f8g9h0i1j2k3', '2004-02-13'),
(72, 'Skylar Hall', 'skylar.h@gmail.com', '5n2m19o7s3n8q4r0p1m2q3r4s5t6u7v8w9x0y1z2a3b4c5d6e7f8g9h0i1j2k3l4', '2002-05-15'),
(73, 'Nathaniel Martinez', 'nathaniel.m@yahoo.com', '6o3n20p8t4o9r5s1q2n3r4s5t6u7v8w9x0y1z2a3b4c5d6e7f8g9h0i1j2k3l4m5', '1997-10-10'),
(74, 'Layla Young', 'layla.young@hotmail.com', '7p4o31q9u5p0s6t2r3o4s5t6u7v8w9x0y1z2a3b4c5d6e7f8g9h0i1j2k3l4m5n6', '2000-08-06'),
(75, 'Cameron Thompson', 'cameron.t@outlook.com', '8q5p42r0v6q1t7u3s4p5t6u7v8w9x0y1z2a3b4c5d6e7f8g9h0i1j2k3l4m5n6o7', '2003-11-21'),
(76, 'Stella Rodriguez', 'stella.r@gmail.com', '9r6q53s1w7r2u8v4t5q6u7v8w9x0y1z2a3b4c5d6e7f8g9h0i1j2k3l4m5n6o7p8', '2005-01-28'),
(77, 'Christian Miller', 'christian.m@gmail.com', '0s7r64t2x8s3v9w5u6r7v8w9x0y1z2a3b4c5d6e7f8g9h0i1j2k3l4m5n6o7p8q9', '1998-07-17'),
(78, 'Penelope Wright', 'penelope.w@yahoo.com', '1t8s75u3y9t4w0x6v7s8w9x0y1z2a3b4c5d6e7f8g9h0i1j2k3l4m5n6o7p8q9r0', '2002-09-09'),
(79, 'Owen Reed', 'owen.reed@gmail.com', '2u9t86v4z0u5x1y7w8t9x0y1z2a3b4c5d6e7f8g9h0i1j2k3l4m5n6o7p8q9r0s1', '1996-04-03'),
(80, 'Audrey Price', 'audrey.p@gmail.com', '3v0u97w5a1v6y2z8x9u0y1z2a3b4c5d6e7f8g9h0i1j2k3l4m5n6o7p8q9r0s1t2', '2001-06-20'),
(81, 'admin', 'admin@admin.com', 'cdxClq8EjaSkmM/2z84ymQ==.0UDe6cg/oeDIXSEVywmmH5l6SQ8h2XlB5ypY8OwePDM=', '2001-06-20');

UPDATE user SET role = 'admin' where email = 'admin@admin.com';
-- ------------------------------------------------------------------------------------------------------------------------------------------------

-- =======================
-- destination rows
-- =======================

INSERT INTO destination (destination_id, name, state, country, longitude, latitude) VALUES
(1, 'Paris', 'Île-de-France', 'France', 2.3522, 48.8566),
(2, 'Nice', 'Provence-Alpes-Côte d''Azur', 'France', 7.2620, 43.7102),
(3, 'Bordeaux', 'Nouvelle-Aquitaine', 'France', -0.5792, 44.8378),
(4, 'New York City', 'NY', 'USA', -74.0060, 40.7128),
(5, 'Los Angeles', 'CA', 'USA', -118.2437, 34.0522),
(6, 'Honolulu', 'HI', 'USA', -157.8583, 21.3069),
(7, 'Beijing', '', 'China', 116.4074, 39.9042),
(8, 'Shanghai', '', 'China', 121.4737, 31.2304),
(9, 'Sydney', 'NSW', 'Australia', 151.2093, -33.8688),
(10, 'Melbourne', 'VIC', 'Australia', 144.9631, -37.8136),
(11, 'Great Barrier Reef', 'QLD', 'Australia', 146.4019, -18.2871),
(12, 'Rio de Janeiro', 'RJ', 'Brazil', -43.1729, -22.9068),
(13, 'Iguazu Falls', 'Paraná', 'Brazil', -54.4366, -25.6953),
(14, 'Cusco', 'Cusco', 'Peru', -71.9702, -13.5320),
(15, 'Lima', 'Lima', 'Peru', -77.0428, -12.0464),
(16, 'Cairo', '', 'Egypt', 31.2357, 30.0444),
(17, 'Luxor', '', 'Egypt', 32.6391, 25.6969),
(18, 'Tokyo', 'Tokyo', 'Japan', 139.6917, 35.6895),
(19, 'Kyoto', 'Kyoto', 'Japan', 135.7681, 35.0116),
(20, 'Mount Fuji', 'Yamanashi', 'Japan', 138.7278, 35.3606),
(21, 'Santorini', 'South Aegean', 'Greece', 25.4217, 36.3932),
(22, 'Athens', 'Attica', 'Greece', 23.7275, 37.9838),
(23, 'Mykonos', 'South Aegean', 'Greece', 25.3287, 37.4467),
(24, 'Banff National Park', 'Alberta', 'Canada', -115.5708, 51.1784),
(25, 'Vancouver', 'British Columbia', 'Canada', -123.1207, 49.2827),
(26, 'Toronto', 'Ontario', 'Canada', -79.3832, 43.6532),
(27, 'Cape Town', 'Western Cape', 'South Africa', 18.4241, -33.9249),
(28, 'Kruger National Park', 'Mpumalanga', 'South Africa', 31.5085, -25.0717),
(29, 'Agra', 'Uttar Pradesh', 'India', 78.0076, 27.1767),
(30, 'Jaipur', 'Rajasthan', 'India', 75.7873, 26.9124),
(31, 'Goa', 'Goa', 'India', 73.9920, 15.2993),
(32, 'Siem Reap', '', 'Cambodia', 103.8587, 13.3640),
(33, 'Phnom Penh', '', 'Cambodia', 104.9282, 11.5564),
(34, 'Wadi Musa', 'Ma''an', 'Jordan', 35.4746, 30.3155),
(35, 'Amman', 'Amman', 'Jordan', 35.9238, 31.9539),
(36, 'Grand Canyon National Park', 'AZ', 'USA', -112.1129, 36.1016),
(37, 'Yellowstone National Park', 'WY', 'USA', -110.5885, 44.4280),
(38, 'Uyuni', 'Potosí', 'Bolivia', -66.8250, -20.4578),
(39, 'La Paz', 'La Paz', 'Bolivia', -68.1336, -16.4897),
(40, 'Hallstatt', 'Upper Austria', 'Austria', 13.6499, 47.5622),
(41, 'Vienna', 'Vienna', 'Austria', 16.3738, 48.2082),
(42, 'Salzburg', 'Salzburg', 'Austria', 13.0550, 47.8095),
(43, 'Cinque Terre', 'Liguria', 'Italy', 9.7371, 44.1065),
(44, 'Rome', 'Lazio', 'Italy', 12.4964, 41.9028),
(45, 'Venice', 'Veneto', 'Italy', 12.3155, 45.4408),
(46, 'Reykjavik', '', 'Iceland', -21.9426, 64.0754),
(47, 'The Golden Circle', '', 'Iceland', -20.3013, 64.2552),
(48, 'Ubud', 'Bali', 'Indonesia', 115.2631, -8.5065),
(49, 'Jakarta', 'Jakarta', 'Indonesia', 106.8456, -6.2088),
(50, 'Phuket', 'Phuket', 'Thailand', 98.3923, 7.8805),
(51, 'Bangkok', '', 'Thailand', 100.5018, 13.7563),
(52, 'Chiang Mai', 'Chiang Mai', 'Thailand', 98.9818, 18.7061),
(53, 'Queenstown', 'Otago', 'New Zealand', 168.6628, -45.0312),
(54, 'Auckland', 'Auckland', 'New Zealand', 174.7633, -36.8485),
(55, 'Zermatt', 'Valais', 'Switzerland', 7.7479, 46.0207),
(56, 'Zurich', 'Zurich', 'Switzerland', 8.5417, 47.3769),
(57, 'Interlaken', 'Bern', 'Switzerland', 7.8633, 46.6865),
(58, 'Dubai', '', 'UAE', 55.2708, 25.2048),
(59, 'Abu Dhabi', '', 'UAE', 54.3773, 24.4539),
(60, 'Hanoi', '', 'Vietnam', 105.8524, 21.0285),
(61, 'Ho Chi Minh City', '', 'Vietnam', 106.6297, 10.8231),
(62, 'Lisbon', '', 'Portugal', -9.1393, 38.7223),
(63, 'Porto', '', 'Portugal', -8.6110, 41.1496),
(64, 'Prague', '', 'Czech Republic', 14.4378, 50.0755),
(65, 'Český Krumlov', 'South Bohemian Region', 'Czech Republic', 14.3168, 48.8139),
(66, 'Amsterdam', 'North Holland', 'Netherlands', 4.8952, 52.3702),
(67, 'Rotterdam', 'South Holland', 'Netherlands', 4.4792, 51.9244),
(68, 'Barcelona', 'Catalonia', 'Spain', 2.1734, 41.3851),
(69, 'Madrid', 'Community of Madrid', 'Spain', -3.7038, 40.4168),
(70, 'Seville', 'Andalusia', 'Spain', -5.9845, 37.3891),
(71, 'Istanbul', '', 'Turkey', 28.9784, 41.0082),
(72, 'Cappadocia', 'Nevşehir', 'Turkey', 34.8540, 38.6473),
(73, 'Antalya', 'Antalya', 'Turkey', 30.7133, 36.8969),
(74, 'Budapest', '', 'Hungary', 19.0402, 47.4979),
(75, 'Edinburgh', '', 'Scotland', -3.1883, 55.9533),
(76, 'The Scottish Highlands', '', 'Scotland', -4.2274, 57.4778),
(77, 'Dublin', '', 'Ireland', -6.2603, 53.3498),
(78, 'Galway', 'County Galway', 'Ireland', -9.0568, 53.2707);

-- ------------------------------------------------------------------------------------------------------------------------------------------------

-- =======================
-- Trips
-- =======================

-- Trip 1: France's Cultural Heartbeat
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(1, 5, 'Trip to France', 4, '2025-06-15', '2025-06-29', 'A two-week journey through France, focusing on its rich cultural heritage. We’ll explore the famous landmarks, museums, and culinary delights of Paris before heading to the sun-drenched south.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(1, 1, 1, '2025-06-15', '2025-06-22', 1, 'Explore the iconic sights of Paris, including the Eiffel Tower, the Louvre Museum, and Notre Dame. Enjoy strolls along the Seine and savoring classic French pastries.'),
(2, 2, 1, '2025-06-22', '2025-06-29', 2, 'Relax on the French Riviera in Nice. We’ll spend time on the Promenade des Anglais, explore the charming Old Town (Vieux Nice), and take day trips to nearby coastal towns.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(1, 24, 1, 1, 'accepted'),
(2, 79, 1, 2, 'accepted'),
(3, 71, 1, 1, 'rejected'),
(4, 70, 2, 1, 'pending'),
(5, 69, 2, 1, 'accepted'),
(6, 43, 2, 1, 'accepted'),
(7, 29, 2, 1, 'rejected');

-- Trip 2: The Taste of France
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(2, 4, 'A Taste of France', 3, '2026-09-05', '2026-09-19', 'A culinary adventure across France. This trip is all about tasting the best of French cuisine, from Michelin-star restaurants in Paris to wine tasting in the Bordeaux region.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(3, 1, 2, '2026-09-05', '2026-09-12', 1, 'Focus on Parisian gastronomy. We’ll visit traditional markets, take a cooking class, and dine at classic French bistros and bakeries.'),
(4, 3, 2, '2026-09-12', '2026-09-19', 2, 'Discover the famous vineyards of Bordeaux. The trip includes guided wine tours, chateau visits, and enjoying the unique culture of this historic wine-producing city.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(8, 8, 3, 1, 'accepted'),
(9, 78, 3, 1, 'accepted'),
(10, 72, 3, 1, 'pending'),
(11, 25, 4, 1, 'accepted'),
(12, 32, 4, 1, 'accepted'),
(13, 13, 4, 2, 'pending'),
(14, 65, 4, 1, 'rejected');

-- Trip 3: A Southern French Getaway
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(3, 62, 'A Southern French Getaway', 5, '2027-07-10', '2027-07-24', 'A two-week summer escape to Southern France. We’ll soak up the sun on the beautiful beaches of Nice and enjoy the vibrant atmosphere before heading to the charm of Bordeaux.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(5, 2, 3, '2027-07-10', '2027-07-17', 1, 'Enjoy the stunning beaches and vibrant nightlife of Nice. We’ll visit the Cours Saleya market, hike to Castle Hill for panoramic views, and relax by the Mediterranean Sea.'),
(6, 3, 3, '2027-07-17', '2027-07-24', 2, 'Shift gears to the historic city of Bordeaux. This part of the trip is for leisurely walks through the old town, visiting the Cité du Vin, and enjoying local restaurants and cafes.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(15, 15, 5, 2, 'accepted'),
(16, 44, 5, 1, 'accepted'),
(17, 21, 5, 1, 'rejected'),
(18, 18, 5, 1, 'accepted'),
(19, 68, 6, 2, 'accepted'),
(20, 63, 6, 1, 'pending');

-- Trip 4: Parisian Romance & Riviera Glamour
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(4, 4, 'Parisian Romance & Riviera Glamour', 4, '2025-08-20', '2025-09-03', 'A romantic and luxurious trip combining the iconic sights of Paris with the glamorous coastal life of Nice. Perfect for couples or friends seeking both culture and relaxation.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(7, 1, 4, '2025-08-20', '2025-08-27', 1, 'Indulge in Parisian romance. We’ll enjoy a dinner cruise on the Seine, visit the Sacré-Cœur, and explore Montmartre’s artistic streets.'),
(8, 2, 4, '2025-08-27', '2025-09-03', 2, 'Experience the glamour of the French Riviera. We’ll relax on private beaches, explore the vibrant nightlife, and visit the Matisse Museum.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(21, 21, 7, 2, 'accepted'),
(22, 2, 7, 1, 'accepted'),
(23, 37, 7, 1, 'rejected'),
(24, 56, 8, 2, 'accepted'),
(25, 51, 8, 1, 'accepted'),
(26, 78, 8, 1, 'rejected');

-- Trip 5: The French Art & Wine Trail
INSERT INTO trip (trip_id, owner_id,trip_name, max_buddies, start_date, end_date, description) VALUES
(5, 66, 'French Art & Wine Trail', 3, '2026-05-10', '2026-05-24', 'An art and wine-focused trip through France. We will immerse ourselves in the great masterpieces of Paris before transitioning to the world-renowned vineyards of Bordeaux.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(9, 1, 5, '2026-05-10', '2026-05-17', 1, 'Focus on the art of Paris. We’ll visit the Musée d’Orsay and Centre Pompidou, and explore the street art of the Marais district.'),
(10, 3, 5, '2026-05-17', '2026-05-24', 2, 'Discover the art of winemaking in Bordeaux. We’ll tour grand chateaus, learn about different varietals, and enjoy the city’s architectural beauty and fine dining.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(27, 27, 9, 1, 'accepted'),
(28, 55, 9, 1, 'pending'),
(29, 29, 9, 1, 'accepted'),
(30, 30, 10, 1, 'accepted'),
(31, 50, 10, 2, 'accepted'),
(32, 60, 10, 1, 'rejected');

-- Trip 1: The American Megapolis Tour
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(9, 26, 'American Megapolis Tour', 4, '2025-05-10', '2025-05-24', 'A journey from the bustling streets of New York City to the glamour of Los Angeles. This trip is about experiencing two of America''s most iconic and contrasting urban centers.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(18, 4, 9, '2025-05-10', '2025-05-17', 1, 'Experience the non-stop energy of New York City. We’ll visit landmarks like the Empire State Building, Central Park, and the Statue of Liberty, and catch a Broadway show.'),
(19, 5, 9, '2025-05-17', '2025-05-24', 2, 'Explore the entertainment capital of the world, Los Angeles. We’ll tour Hollywood, walk the Walk of Fame, and enjoy the diverse neighborhoods of the city.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(54, 57, 18, 1, 'accepted'),
(55, 23, 18, 2, 'accepted'),
(56, 74, 18, 1, 'rejected'),
(57, 25, 19, 1, 'accepted'),
(58, 28, 19, 2, 'accepted'),
(59, 59, 19, 1, 'accepted');

-- Trip 2: West Coast to Paradise
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(10, 10, 'West Coast to Paradise', 5, '2026-07-15', '2026-07-29', 'A trip from the sunny beaches of Los Angeles to the tropical oasis of Honolulu. We will combine the fun of city sightseeing with the relaxation of a beach holiday.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(20, 5, 10, '2026-07-15', '2026-07-22', 1, 'Enjoy the sun-drenched coast of Los Angeles. We’ll hit up Santa Monica Pier, visit Venice Beach, and soak up the laid-back California vibe.'),
(21, 6, 10, '2026-07-22', '2026-07-29', 2, 'Unwind in Honolulu. The trip includes relaxing on Waikiki Beach, exploring the historic Pearl Harbor, and enjoying fresh local seafood and luaus.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(60, 60, 20, 2, 'accepted'),
(61, 2, 20, 1, 'accepted'),
(62, 11, 20, 1, 'rejected'),
(63, 63, 21, 1, 'accepted'),
(64, 48, 21, 2, 'pending'),
(65, 65, 21, 1, 'accepted'),
(66, 26, 21, 1, 'rejected');

-- Trip 3: Coast-to-Coast Explorer
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(11, 32, 'Coast-to-Coast Exploration', 4, '2027-09-01', '2027-09-22', 'A cross-country American adventure. We will explore the iconic landmarks of New York, the glamour of Los Angeles, and the tropical beauty of Honolulu.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(22, 4, 11, '2027-09-01', '2027-09-08', 1, 'Explore the cultural diversity and iconic landmarks of New York City. We’ll visit museums, explore different neighborhoods, and enjoy the city''s famous food scene.'),
(23, 5, 11, '2027-09-08', '2027-09-15', 2, 'Head to Los Angeles for a week of sightseeing and fun. Highlights include a visit to Universal Studios, exploring the coastal city of Santa Monica, and hiking to the Hollywood Sign.'),
(24, 6, 11, '2027-09-15', '2027-09-22', 3, 'End the trip with a relaxing week in Honolulu. We’ll go snorkeling at Hanauma Bay, enjoy a traditional luau, and simply unwind by the ocean.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(67, 6, 22, 1, 'accepted'),
(68, 65, 22, 1, 'accepted'),
(69, 43, 23, 2, 'accepted'),
(70, 74, 23, 1, 'accepted'),
(71, 72, 24, 1, 'accepted'),
(72, 38, 24, 2, 'accepted'),
(73, 45, 24, 1, 'rejected');

-- Trip 4: The Ultimate USA Getaway
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(12, 3, 'Ultimate USA Getaway', 6, '2025-10-10', '2025-10-31', 'A comprehensive tour of the United States. This trip is for those who want to see it all, from the East Coast''s vibrant city life to the Pacific''s tropical beauty.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(25, 4, 12, '2025-10-10', '2025-10-18', 1, 'Start the trip in New York City with an exploration of its many iconic neighborhoods and attractions.'),
(26, 5, 12, '2025-10-18', '2025-10-25', 2, 'Fly to Los Angeles to experience the laid-back West Coast culture and visit famous landmarks and theme parks.'),
(27, 6, 12, '2025-10-25', '2025-10-31', 3, 'Conclude the journey in Honolulu with plenty of time for beach activities, a visit to Pearl Harbor, and enjoying a relaxing island atmosphere.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(74, 24, 25, 2, 'accepted'),
(75, 75, 25, 1, 'accepted'),
(76, 2, 26, 1, 'accepted'),
(77, 11, 26, 2, 'accepted'),
(78, 38, 27, 1, 'pending'),
(79, 29, 27, 1, 'accepted'),
(80, 80, 27, 1, 'accepted');

-- Trip 5: The City & Beach Escape
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(13, 73, 'City & Beach Escape', 3, '2026-03-05', '2026-03-19', 'A perfect escape from city life to a tropical paradise. This trip combines the excitement of New York City with the ultimate relaxation of Honolulu.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(28, 4, 13, '2026-03-05', '2026-03-12', 1, 'Enjoy the culture and excitement of New York City, visiting museums, monuments, and iconic sights.'),
(29, 6, 13, '2026-03-12', '2026-03-19', 2, 'Unwind and enjoy the tropical beauty of Honolulu, from relaxing on the beach to exploring the island''s natural wonders.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(81, 33, 28, 1, 'accepted'),
(82, 77, 28, 2, 'accepted'),
(83, 21, 29, 1, 'accepted'),
(84, 14, 29, 1, 'accepted'),
(85, 4, 29, 1, 'accepted'),
(86, 5, 29, 1, 'rejected');

-- Trip 1: The Chinese Mega-Cities Tour
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(14, 12, 'Chinese Mega-Cities Tour', 4, '2025-09-01', '2025-09-15', 'A journey through China''s two most iconic cities. This trip contrasts Beijing''s historical landmarks with Shanghai''s futuristic cityscape.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(30, 7, 14, '2025-09-01', '2025-09-08', 1, 'Explore the ancient wonders of Beijing, including the Great Wall, the Forbidden City, and Tiananmen Square. We will also discover the city''s culinary delights.'),
(31, 8, 14, '2025-09-08', '2025-09-15', 2, 'Experience the modern marvels of Shanghai. We’ll visit the Bund for stunning skyline views, ascend the Shanghai Tower, and explore the trendy art districts.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(87, 76, 30, 2, 'accepted'),
(88, 43, 30, 1, 'accepted'),
(89, 25, 30, 1, 'rejected'),
(90, 67, 31, 1, 'accepted'),
(91, 74, 31, 2, 'accepted'),
(92, 7, 31, 1, 'accepted');

-- Trip 2: China's Cultural & Financial Hubs
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(15, 15, 'China''s Cultural and Financial Hubs ', 3, '2026-04-10', '2026-04-24', 'An in-depth tour of China''s historical and economic powerhouses. We will delve into Beijing''s imperial past and Shanghai''s modern, vibrant present.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(32, 7, 15, '2026-04-10', '2026-04-17', 1, 'Focus on Beijing''s cultural heritage. Our itinerary includes visits to the Summer Palace, the Temple of Heaven, and a walk through the historic hutongs.'),
(33, 8, 15, '2026-04-17', '2026-04-24', 2, 'Explore the financial hub of Shanghai. We will walk along the Bund, visit the Yu Garden, and enjoy the city’s world-class dining and nightlife.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(93, 56, 32, 1, 'accepted'),
(94, 71, 32, 2, 'accepted'),
(95, 21, 32, 1, 'rejected'),
(96, 10, 33, 1, 'accepted'),
(97, 28, 33, 1, 'accepted'),
(98, 14, 33, 1, 'accepted');

-- Trip 3: Ancient Wonders & Modern Marvels
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(16, 45, 'Ancient Wonders and Modern Marvels', 5, '2027-10-05', '2027-10-26', 'A comprehensive trip that contrasts ancient China with its modern advancements. We will delve into the history of Beijing and the future-forward energy of Shanghai.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(34, 7, 16, '2027-10-05', '2027-10-15', 1, 'An extended stay in Beijing to fully experience its imperial history and cultural richness. Activities include visiting the Great Wall, Forbidden City, and exploring the city’s vibrant art scene.'),
(35, 8, 16, '2027-10-15', '2027-10-26', 2, 'A longer stay in Shanghai to fully immerse in its modern lifestyle. We’ll take a Huangpu River cruise, explore the French Concession, and visit contemporary art galleries.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(99, 21, 34, 1, 'accepted'),
(100, 30, 34, 2, 'accepted'),
(101, 72, 34, 1, 'accepted'),
(102, 71, 35, 1, 'accepted'),
(103, 20, 35, 2, 'accepted'),
(104, 30, 35, 1, 'accepted'),
(105, 35, 35, 1, 'rejected');

-- Trip 4: The Best of China's East Coast
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(17, 73, 'The Best of China''s East Coast' , 4, '2025-11-20', '2025-12-04', 'A concise but complete tour of China''s most important eastern cities. This trip is for those who want to experience the main highlights of Beijing and Shanghai in two weeks.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(36, 7, 17, '2025-11-20', '2025-11-27', 1, 'Take in the majestic sights of Beijing. We will focus on the Forbidden City, the Temple of Heaven, and experiencing the local culture through a Peking Duck dinner.'),
(37, 8, 17, '2025-11-27', '2025-12-04', 2, 'Shift gears to modern Shanghai. We’ll visit the Oriental Pearl Tower, wander through the historic Bund, and shop on Nanjing Road.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(106, 6, 36, 1, 'accepted'),
(107, 32, 36, 1, 'accepted'),
(108, 8, 36, 1, 'accepted'),
(109, 59, 37, 2, 'accepted'),
(110, 39, 37, 1, 'accepted'),
(111, 47, 37, 1, 'rejected');

-- Trip 5: The China Discovery Trip
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(18, 3, 'China Discovery Trip', 5, '2026-06-15', '2026-06-29', 'An immersive journey into the heart of China. We will combine the historical richness of Beijing with the modern, cosmopolitan flair of Shanghai.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(38, 7, 18, '2026-06-15', '2026-06-22', 1, 'Focus on Beijing''s cultural and historical sites. We’ll visit the Great Wall, the Forbidden City, and explore the city''s hidden alleyways (hutongs) and vibrant food scene.'),
(39, 8, 18, '2026-06-22', '2026-06-29', 2, 'Experience modern China in Shanghai. We will explore the Bund at night, visit the Yu Garden, and enjoy the city’s impressive skyscrapers and shopping districts.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(112, 12, 38, 2, 'accepted'),
(113, 74, 38, 1, 'accepted'),
(114, 1, 38, 1, 'rejected'),
(115, 15, 39, 1, 'accepted'),
(116, 80, 39, 2, 'accepted'),
(117, 37, 39, 1, 'accepted'),
(118, 18, 39, 1, 'rejected');

-- Trip 1: The Iconic Australia East Coast Tour
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(19, 27, 'Iconic Australia East Coast Tour', 4, '2025-03-10', '2025-03-31', 'A classic Australian adventure combining the vibrant cities of Sydney and Melbourne with the natural wonder of the Great Barrier Reef.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(40, 9, 19, '2025-03-10', '2025-03-17', 1, 'Explore iconic Sydney landmarks like the Sydney Opera House and Harbour Bridge. We’ll also visit Bondi Beach and enjoy the city''s incredible dining scene.'),
(41, 10, 19, '2025-03-17', '2025-03-24', 2, 'Experience the cultural heart of Melbourne. We''ll explore hidden laneways, visit art galleries, and enjoy the city’s famous coffee culture.'),
(42, 11, 19, '2025-03-24', '2025-03-31', 3, 'Dive into the Great Barrier Reef. This part of the trip is focused on snorkeling or scuba diving to experience the incredible marine life and coral formations.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(119, 19, 40, 1, 'accepted'),
(120, 79, 40, 1, 'accepted'),
(121, 42, 40, 2, 'accepted'),
(122, 22, 41, 1, 'accepted'),
(123, 75, 41, 1, 'rejected'),
(124, 34, 42, 2, 'accepted'),
(125, 25, 42, 1, 'accepted');

-- Trip 2: The Ultimate Australian Nature & City Escape
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(20, 15, 'Ultimate Australian Nature & City Escape', 3, '2026-06-05', '2026-06-19', 'A trip offering the best of both worlds: the vibrant city life of Melbourne and the breathtaking natural beauty of the Great Barrier Reef.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(43, 10, 20, '2026-06-05', '2026-06-12', 1, 'Explore Melbourne''s vibrant culture, street art, and world-class coffee shops. We will also take a day trip to see the penguins at St Kilda.'),
(44, 11, 20, '2026-06-12', '2026-06-19', 2, 'Embark on a marine adventure at the Great Barrier Reef. The focus is on snorkeling, diving, and exploring the diverse coral and fish species.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(126, 17, 43, 1, 'accepted'),
(127, 16, 43, 1, 'accepted'),
(128, 21, 43, 1, 'accepted'),
(129, 34, 44, 2, 'accepted'),
(130, 29, 44, 1, 'accepted'),
(131, 68, 44, 1, 'rejected');

-- Trip 3: The Urban & Reef Explorer
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(21, 10, 'Urban & Reef Explorer',5, '2027-01-20', '2027-02-03', 'A trip for those who want to see Australia''s most iconic cityscape and its most breathtaking natural wonder. It combines the fun of Sydney with the beauty of the Great Barrier Reef.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(45, 9, 21, '2027-01-20', '2027-01-27', 1, 'Spend time exploring the famous sights of Sydney, including the Opera House, Harbour Bridge, and Darling Harbour. We''ll also take a ferry to Manly Beach.'),
(46, 11, 21, '2027-01-27', '2027-02-03', 2, 'Dive into the stunning marine world of the Great Barrier Reef. This part of the trip is dedicated to exploring the coral ecosystems and enjoying the tropical climate.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(132, 9, 45, 2, 'accepted'),
(133, 32, 45, 1, 'accepted'),
(134, 4, 45, 1, 'accepted'),
(135, 56, 46, 2, 'accepted'),
(136, 76, 46, 1, 'accepted'),
(137, 73, 46, 1, 'accepted'),
(138, 28, 46, 1, 'rejected');

-- Trip 4: The Southern & Northern Australia Trip
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(22, 68, 'Southern & Northern Australia Trip',4, '2025-09-15', '2025-10-06', 'A comprehensive journey from the vibrant southern cities of Australia to the tropical north. We''ll explore both urban culture and natural beauty on this trip.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(47, 10, 22, '2025-09-15', '2025-09-22', 1, 'Start in Melbourne, exploring its unique laneways, street art, and culinary scene. We will also visit the famous Queen Victoria Market.'),
(48, 9, 22, '2025-09-22', '2025-09-29', 2, 'Fly to Sydney to experience its iconic sights, including the Harbour Bridge, Opera House, and the beautiful beaches.'),
(49, 11, 22, '2025-09-29', '2025-10-06', 3, 'Head to the Great Barrier Reef for a week of marine adventures, including snorkeling and diving to see the breathtaking coral reef.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(139, 39, 47, 1, 'accepted'),
(140, 40, 47, 1, 'accepted'),
(141, 31, 47, 2, 'accepted'),
(142, 42, 48, 1, 'accepted'),
(143, 33, 48, 1, 'accepted'),
(144, 34, 49, 2, 'accepted'),
(145, 67, 49, 1, 'accepted'),
(146, 12, 49, 1, 'accepted');

-- Trip 5: The Australian East Coast Trilogy
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(23, 76, 'Australian East Coast Trilogy', 5, '2026-11-01', '2026-11-22', 'A complete exploration of Australia''s east coast, combining the cultural hubs of Sydney and Melbourne with the natural masterpiece of the Great Barrier Reef.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(50, 9, 23, '2026-11-01', '2026-11-08', 1, 'Dive into the city life of Sydney, from climbing the Harbour Bridge to relaxing at Bondi Beach and taking in the iconic Opera House.'),
(51, 10, 23, '2026-11-08', '2026-11-15', 2, 'Experience the arts and culture scene in Melbourne, exploring its famous coffee shops, vibrant street art, and diverse culinary offerings.'),
(52, 11, 23, '2026-11-15', '2026-11-22', 3, 'Conclude the trip with a life-changing visit to the Great Barrier Reef for snorkeling, diving, and observing the incredible marine biodiversity.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(147, 47, 50, 2, 'accepted'),
(148, 38, 50, 1, 'accepted'),
(149, 49, 50, 1, 'accepted'),
(150, 50, 51, 2, 'accepted'),
(151, 1, 51, 1, 'accepted'),
(152, 2, 52, 2, 'accepted'),
(153, 65, 52, 1, 'accepted'),
(154, 32, 52, 1, 'rejected');

-- Trip 1: Brazil's Natural Wonders & Urban Vibes
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(24, 68, 'Brazil''s Natural Wonders & Urban Vibes', 4, '2025-08-10', '2025-08-24', 'A perfect blend of Brazil''s iconic urban energy and its breathtaking natural beauty. This trip combines the vibrant culture of Rio with the awe-inspiring Iguazu Falls.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(53, 12, 24, '2025-08-10', '2025-08-17', 1, 'Experience the vibrant spirit of Rio de Janeiro. We’ll visit Christ the Redeemer and Sugarloaf Mountain, relax on Copacabana and Ipanema beaches, and explore the city''s samba scene.'),
(54, 13, 24, '2025-08-17', '2025-08-24', 2, 'Witness the spectacular Iguazu Falls. This part of the trip is dedicated to exploring the massive waterfalls from both the Brazilian side and taking a thrilling boat ride into the falls.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(155, 64, 53, 2, 'accepted'),
(156, 51, 53, 1, 'accepted'),
(157, 53, 53, 1, 'rejected'),
(158, 49, 54, 1, 'accepted'),
(159, 47, 54, 2, 'accepted'),
(160, 32, 54, 1, 'accepted');

-- Trip 2: The Brazilian Adventure
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(25, 56, 'Brazilian Adventure', 3, '2026-03-05', '2026-03-19', 'An adventure trip combining the festive atmosphere of Rio de Janeiro with the natural splendor of Iguazu Falls. We''ll explore urban landscapes and stunning nature.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(55, 12, 25, '2026-03-05', '2026-03-12', 1, 'Immerse in the culture of Rio de Janeiro. We will visit the Escadaria Selarón, explore the Santa Teresa neighborhood, and enjoy a day at the beach.'),
(56, 13, 25, '2026-03-12', '2026-03-19', 2, 'Experience the power of Iguazu Falls. We’ll take a helicopter tour for a bird''s-eye view, hike the trails, and feel the spray of the falls up close.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(161, 1, 55, 1, 'accepted'),
(162, 2, 55, 1, 'accepted'),
(163, 9, 55, 1, 'accepted'),
(164, 4, 56, 2, 'accepted'),
(165, 43, 56, 1, 'accepted'),
(166, 57, 56, 1, 'rejected');

-- Trip 3: Brazil's Best Kept Secret: The Falls & City Life
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(26, 48, 'Brazil''s Best Kept Secret: The Falls & City Life', 5, '2027-01-20', '2027-02-03', 'A captivating journey to witness one of the world''s greatest natural wonders and experience the lively atmosphere of Rio de Janeiro.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(57, 13, 26, '2027-01-20', '2027-01-27', 1, 'Start the trip with the magnificent Iguazu Falls. We will spend a full week exploring the falls from different angles and enjoying the surrounding subtropical rainforest.'),
(58, 12, 26, '2027-01-27', '2027-02-03', 2, 'Fly to Rio de Janeiro for a week of exploration. We’ll take a cable car to Sugarloaf Mountain, visit the iconic Christ the Redeemer statue, and enjoy the city’s famous beaches and vibrant nightlife.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(167, 2, 57, 2, 'accepted'),
(168, 8, 57, 1, 'accepted'),
(169, 11, 57, 1, 'accepted'),
(170, 24, 58, 2, 'accepted'),
(171, 39, 58, 1, 'accepted'),
(172, 27, 58, 1, 'accepted'),
(173, 80, 58, 1, 'rejected');

-- Trip 4: The Brazilian East Coast Trip
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(27, 27, 'Brazilian East Coast Trip',4, '2025-05-15', '2025-05-29', 'A concise but complete tour of Brazil''s most famous city and natural wonder. This trip is for those who want to experience the main highlights of Rio and Iguazu Falls in two weeks.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(59, 12, 27, '2025-05-15', '2025-05-22', 1, 'Enjoy the stunning sights of Rio de Janeiro. We will focus on the famous beaches of Ipanema and Copacabana, and explore the city''s lively music and food scene.'),
(60, 13, 27, '2025-05-22', '2025-05-29', 2, 'Shift gears to the natural wonder of Iguazu Falls. We’ll take a boat tour to get close to the falls and explore the surrounding national park, with its diverse flora and fauna.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(174, 74, 59, 1, 'accepted'),
(175, 11, 59, 1, 'accepted'),
(176, 65, 59, 1, 'accepted'),
(177, 77, 60, 2, 'accepted'),
(178, 52, 60, 1, 'accepted'),
(179, 59, 60, 1, 'rejected');

-- Trip 5: The Ultimate Brazil Experience
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(28, 48, 'Ultimate Brazil Experience', 5, '2026-07-10', '2026-07-24', 'A complete exploration of Brazil, combining the cultural hub of Rio de Janeiro with the natural masterpiece of Iguazu Falls.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(61, 12, 28, '2026-07-10', '2026-07-17', 1, 'Dive into the city life of Rio, from climbing Sugarloaf Mountain to relaxing at its iconic beaches and enjoying the vibrant nightlife.'),
(62, 13, 28, '2026-07-17', '2026-07-24', 2, 'Conclude the trip with a life-changing visit to Iguazu Falls for a boat tour, hiking, and exploring the incredible power of the falls from various viewpoints.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(180, 38, 61, 2, 'accepted'),
(181, 31, 61, 1, 'accepted'),
(182, 66, 61, 1, 'accepted'),
(183, 69, 62, 2, 'accepted'),
(184, 52, 62, 1, 'accepted'),
(185, 48, 62, 1, 'accepted'),
(186, 11, 62, 1, 'rejected');

-- Perjalanan 1: Petualangan Kuno & Kota Modern
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(29, 64, 'Petualangan Kuno & Kota Modern',4, '2025-09-05', '2025-09-26', 'Perjalanan yang menggabungkan keajaiban sejarah Cusco dengan kehidupan modern di Lima. Perpaduan sempurna antara budaya kuno dan suasana kota kontemporer.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(63, 14, 29, '2025-09-05', '2025-09-19', 1, 'Habiskan dua minggu di Cusco untuk menjelajahi reruntuhan Inca di sekitarnya, termasuk benteng Sacsayhuamán dan situs-situs di Lembah Suci. Ini adalah titik awal untuk mendaki Machu Picchu.'),
(64, 15, 29, '2025-09-19', '2025-09-26', 2, 'Akhiri perjalanan di Lima. Kita akan menikmati masakan kelas dunia, mengunjungi distrik Miraflores, dan menjelajahi arsitektur kolonial di pusat kota.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(187, 20, 63, 2, 'accepted'),
(188, 48, 63, 1, 'accepted'),
(189, 75, 63, 1, 'rejected'),
(190, 62, 64, 1, 'accepted'),
(191, 41, 64, 2, 'accepted'),
(192, 29, 64, 1, 'accepted');

-- Perjalanan 2: Jantung Budaya & Ibu Kota Kuliner
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(30, 75, 'Jantung Budaya & Ibu Kota Kuliner',3, '2026-04-10', '2026-04-24', 'Perjalanan yang fokus pada kekayaan budaya dan masakan Peru. Kita akan menyelami sejarah kuno Cusco sebelum menikmati hidangan gastronomi kelas dunia di Lima.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(65, 14, 30, '2026-04-10', '2026-04-17', 1, 'Mulailah di Cusco, jantung Kekaisaran Inca. Kita akan mengunjungi Kuil Matahari, Katedral Cusco, dan menjelajahi gang-gang sempit yang menawan.'),
(66, 15, 30, '2026-04-17', '2026-04-24', 2, 'Pindah ke Lima untuk petualangan kuliner. Rencana termasuk mencicipi ceviche, mencoba hidangan tradisional Peru, dan mengunjungi pasar lokal.'),
(67, 14, 30, '2026-04-24', '2026-04-24', 3, 'Kembali ke Cusco untuk satu hari terakhir, menikmati suasana dan membeli oleh-oleh.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(193, 33, 65, 1, 'accepted'),
(194, 39, 65, 1, 'accepted'),
(195, 55, 65, 1, 'rejected'),
(196, 16, 66, 2, 'accepted'),
(197, 27, 66, 1, 'accepted'),
(198, 80, 67, 1, 'accepted');

-- Perjalanan 3: Tur Utama Peru
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(31, 15, 'Tur Utama Peru',5, '2027-01-20', '2027-02-03', 'Perjalanan yang memukau untuk merasakan yang terbaik dari Peru, menggabungkan energi ibu kota dan pesona kuno Pegunungan Andes.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(68, 15, 31, '2027-01-20', '2027-01-27', 1, 'Mulai perjalanan di Lima untuk menjelajahi kekayaan budayanya. Kita akan mengunjungi museum, berjalan-jalan di sepanjang garis pantai Pasifik, dan makan di beberapa restoran terbaik di dunia.'),
(69, 14, 31, '2027-01-27', '2027-02-03', 2, 'Terbang ke Cusco untuk petualangan di pegunungan. Rencana termasuk mendaki ke Machu Picchu, menjelajahi reruntuhan kuno, dan merasakan budaya asli Andes.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(199, 39, 68, 2, 'accepted'),
(200, 47, 68, 1, 'accepted'),
(201, 13, 68, 1, 'rejected'),
(202, 50, 69, 2, 'accepted'),
(203, 53, 69, 1, 'accepted'),
(204, 61, 69, 1, 'accepted');

-- Perjalanan 4: Penjelajahan Inca & Pesisir Peru
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(32, 59, 'Penjelajahan Inca & Pesisir Peru',4, '2025-10-15', '2025-10-29', 'Perjalanan dari pegunungan Andes yang tinggi ke garis pantai Pasifik. Kombinasi yang sempurna antara situs sejarah dan pengalaman pantai.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(70, 14, 32, '2025-10-15', '2025-10-22', 1, 'Menjelajahi keajaiban kuno Cusco. Fokus akan berada pada situs-situs Inca yang megah dan pasar-pasar lokal yang ramai.'),
(71, 15, 32, '2025-10-22', '2025-10-29', 2, 'Pindah ke Lima untuk menikmati masakan yang luar biasa dan pemandangan laut. Kita akan menjelajahi distrik-distrik modern dan merasakan suasana santai di pantai.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(205, 45, 70, 1, 'accepted'),
(206, 16, 70, 1, 'accepted'),
(207, 27, 70, 1, 'accepted'),
(208, 38, 71, 2, 'accepted'),
(209, 9, 71, 1, 'accepted'),
(210, 10, 71, 1, 'rejected');

-- Perjalanan 5: Perjalanan Kuliner & Sejarah Peru
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(33, 71, 'Perjalanan Kuliner & Sejarah Peru', 5, '2026-08-01', '2026-08-22', 'Perjalanan mendalam yang merayakan sejarah dan masakan Peru. Kita akan meluangkan waktu untuk mengeksplorasi dua kota terpenting di Peru secara mendalam.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(72, 14, 33, '2026-08-01', '2026-08-11', 1, 'Habiskan 10 hari penuh di Cusco untuk mendalami budaya Andes. Kita akan mengunjungi berbagai situs arkeologi, termasuk Machu Picchu, dan belajar tentang sejarah Inca.'),
(73, 15, 33, '2026-08-11', '2026-08-22', 2, 'Pindah ke Lima untuk fokus pada masakan. Rencana termasuk tur makanan, kelas memasak, dan mengunjungi pasar lokal untuk mencicipi bahan-bahan segar.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(211, 7, 72, 2, 'accepted'),
(212, 12, 72, 1, 'accepted'),
(213, 13, 72, 1, 'accepted'),
(214, 14, 73, 2, 'accepted'),
(215, 15, 73, 1, 'accepted'),
(216, 16, 73, 1, 'accepted'),
(217, 17, 73, 1, 'rejected');

-- Trip 1: The Land of the Pharaohs
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(34, 6, 'The Land of the Pharaohs', 4, '2025-10-10', '2025-10-24', 'A two-week journey into the heart of ancient Egypt. We will explore the iconic Pyramids of Giza in Cairo before heading south to the magnificent temples of Luxor.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(74, 16, 34, '2025-10-10', '2025-10-17', 1, 'Explore Cairo, the largest city in Africa. We’ll visit the Pyramids of Giza and the Sphinx, the Egyptian Museum, and the vibrant Khan el-Khalili bazaar.'),
(75, 17, 34, '2025-10-17', '2025-10-24', 2, 'Journey to Luxor to see the Valley of the Kings, the Temple of Karnak, and the Temple of Hatshepsut. A felucca ride on the Nile is also on the agenda.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(218, 48, 74, 2, 'accepted'),
(219, 49, 74, 1, 'accepted'),
(220, 40, 74, 1, 'rejected'),
(221, 41, 75, 1, 'accepted'),
(222, 72, 75, 2, 'accepted'),
(223, 53, 75, 1, 'accepted');

-- Trip 2: Nile River Journey
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(35, 76, 'Nile River Journey', 3, '2026-03-05', '2026-03-19', 'A trip focused on the historical heart of Egypt. We will visit the iconic monuments of Cairo and then explore Luxor, often as part of a classic Nile cruise.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(76, 16, 35, '2026-03-05', '2026-03-12', 1, 'Spend a week in Cairo exploring its magnificent historical sites, including the Pyramids of Giza and the Egyptian Museum. We’ll also wander through the old Islamic quarter.'),
(77, 17, 35, '2026-03-12', '2026-03-19', 2, 'Travel to Luxor and visit the temples of Karnak and Luxor. This segment of the trip is perfect for a relaxing Nile cruise, allowing for exploration of the ancient city and nearby temples.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(224, 75, 76, 1, 'accepted'),
(225, 44, 76, 1, 'accepted'),
(226, 45, 76, 1, 'accepted'),
(227, 55, 77, 2, 'accepted'),
(228, 33, 77, 1, 'accepted'),
(229, 32, 77, 1, 'rejected');

-- Trip 3: Egypt’s Historical Capitals
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(36, 1, 'Egypt’s Historical Capitals', 5, '2027-01-20', '2027-02-03', 'A captivating journey that contrasts modern Egypt with its ancient past. We will explore the bustling life of Cairo before diving into the magnificent ruins of ancient Thebes (Luxor).');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(78, 16, 36, '2027-01-20', '2027-01-27', 1, 'Begin the trip in Cairo, exploring modern life and ancient sites like the Citadel of Saladin and the Hanging Church. A day trip to the Pyramids is also on the itinerary.'),
(79, 17, 36, '2027-01-27', '2027-02-03', 2, 'Travel to Luxor to explore the Temple of Luxor at night, the massive complex of Karnak, and the tombs in the Valley of the Kings. We''ll also take a hot air balloon ride over the West Bank.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(230, 79, 78, 2, 'accepted'),
(231, 45, 78, 1, 'accepted'),
(232, 63, 78, 1, 'rejected'),
(233, 23, 79, 2, 'accepted'),
(234, 22, 79, 1, 'accepted'),
(235, 4, 79, 1, 'accepted');

-- Trip 4: The Egyptian Discovery Trip
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(37, 65, 'Egyptian Discovery Trip', 4, '2025-02-15', '2025-03-01', 'A two-week comprehensive tour of Egypt''s most famous historical sites. This trip is for those who want to experience the main highlights of Cairo and Luxor.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(80, 16, 37, '2025-02-15', '2025-02-22', 1, 'Explore the stunning sights of Cairo, including the Pyramids of Giza, the Sphinx, and the historic Cairo Citadel.'),
(81, 17, 37, '2025-02-22', '2025-03-01', 2, 'Travel to Luxor to see the iconic Valley of the Kings, the Temple of Hatshepsut, and enjoy a traditional Egyptian dinner and show.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(236, 43, 80, 1, 'accepted'),
(237, 70, 80, 1, 'accepted'),
(238, 56, 80, 1, 'accepted'),
(239, 15, 81, 2, 'accepted'),
(240, 19, 81, 1, 'accepted'),
(241, 21, 81, 1, 'rejected');

-- Trip 5: The Egyptian Heritage Tour
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(38, 3, 'Egyptian Heritage Tour', 5, '2026-11-10', '2026-11-24', 'An immersive journey into the heart of Egypt''s ancient history. This trip provides an in-depth exploration of the fascinating sites in both Cairo and Luxor.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(82, 16, 38, '2026-11-10', '2026-11-17', 1, 'Dive into the rich history of Cairo, visiting the Pyramids, the Egyptian Museum, and exploring Old Cairo with its Coptic and Islamic landmarks.'),
(83, 17, 38, '2026-11-17', '2026-11-24', 2, 'Conclude the trip with a comprehensive tour of Luxor, including a visit to the Valley of the Kings, the Temple of Karnak, and a relaxing cruise along the Nile River.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(242, 43, 82, 2, 'accepted'),
(243, 12, 82, 1, 'accepted'),
(244, 65, 82, 1, 'accepted'),
(245, 8, 83, 2, 'accepted'),
(246, 67, 83, 1, 'accepted'),
(247, 1, 83, 1, 'accepted'),
(248, 2, 83, 1, 'rejected');

-- Trip 1: The Ultimate Japan Trilogy
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(44, 25, 'Ultimate Japan Trilogy', 4, '2026-05-15', '2026-06-05', 'A comprehensive journey that combines the vibrant life of Tokyo, the serene beauty of Kyoto, and the majestic presence of Mount Fuji.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(94, 18, 44, '2026-05-15', '2026-05-22', 1, 'Explore the dynamic capital of Tokyo. We''ll visit iconic neighborhoods like Shibuya and Shinjuku, try incredible street food, and experience the city''s pop culture.'),
(95, 19, 44, '2026-05-22', '2026-05-29', 2, 'Travel to Kyoto to immerse in ancient traditions. We’ll see temples, serene bamboo groves, and traditional tea houses, and hopefully spot a geisha.'),
(96, 20, 44, '2026-05-29', '2026-06-05', 3, 'Experience the majestic beauty of Mount Fuji. We’ll hike the surrounding trails, visit the serene Five Lakes region, and take in breathtaking views of the iconic peak.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(281, 12, 94, 2, 'accepted'),
(282, 18, 94, 1, 'accepted'),
(283, 22, 94, 1, 'rejected'),
(284, 30, 95, 1, 'accepted'),
(285, 34, 95, 2, 'accepted'),
(286, 41, 95, 1, 'accepted'),
(287, 48, 96, 2, 'accepted'),
(288, 55, 96, 1, 'accepted'),
(289, 61, 96, 1, 'rejected');

-- Trip 2: Nature & Tradition Tour
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(45, 15, 'Nature & Tradition Tour', 3, '2025-07-10', '2025-07-24', 'A journey for nature and culture lovers. This trip starts at the iconic Mount Fuji before delving into the historical heart of Kyoto and ending in the bustling capital, Tokyo.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(97, 20, 45, '2025-07-10', '2025-07-14', 1, 'Enjoy the stunning scenery and tranquility of Mount Fuji. We will explore the Fuji Five Lakes area and take a scenic bus tour.'),
(98, 19, 45, '2025-07-14', '2025-07-20', 2, 'Travel to Kyoto to explore its ancient temples, beautiful gardens, and traditional districts. We’ll visit the Fushimi Inari Shrine and Kiyomizu-dera Temple.'),
(99, 18, 45, '2025-07-20', '2025-07-24', 3, 'End the trip in Tokyo. We''ll explore the urban landscape, from the vibrant Shibuya Crossing to the serene Meiji Shrine, and experience the city''s incredible food scene.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(290, 3, 97, 1, 'accepted'),
(291, 7, 97, 1, 'accepted'),
(292, 10, 97, 1, 'accepted'),
(293, 3, 98, 2, 'accepted'),
(294, 19, 98, 1, 'accepted'),
(295, 3, 99, 1, 'accepted'),
(296, 28, 99, 2, 'accepted'),
(297, 32, 99, 1, 'rejected');

-- Trip 3: The City & Mountain Escape
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(46, 16, 'City & Mountain Escape', 5, '2027-04-20', '2027-05-04', 'A captivating journey that contrasts modern Tokyo with the majestic beauty of Mount Fuji. It''s the perfect trip for those who want a mix of urban excitement and natural serenity.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(100, 18, 46, '2027-04-20', '2027-04-27', 1, 'Begin the trip in Tokyo, exploring modern life and ancient sites like the Imperial Palace and Senso-ji Temple.'),
(101, 20, 46, '2027-04-27', '2027-05-04', 2, 'Travel to Mount Fuji to experience its breathtaking natural beauty. We’ll hike some of the lower trails, take a scenic ropeway ride, and visit the iconic Chureito Pagoda for stunning views.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(298, 45, 100, 2, 'accepted'),
(299, 50, 100, 1, 'accepted'),
(300, 52, 100, 1, 'rejected'),
(301, 57, 101, 2, 'accepted'),
(302, 63, 101, 1, 'accepted'),
(303, 67, 101, 1, 'accepted');

-- Trip 4: The Historical & Natural Path
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(47, 47, 'Historical & Natural Path', 4, '2025-11-20', '2025-12-04', 'A journey focused on Japan''s historical heart and its most iconic natural landmark. This trip is for those who want a blend of culture and peace.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(102, 19, 47, '2025-11-20', '2025-11-27', 1, 'Enjoy the serene and traditional atmosphere of Kyoto. We will explore the iconic Golden Pavilion, the Arashiyama Bamboo Grove, and the historic Gion district.'),
(103, 20, 47, '2025-11-27', '2025-12-04', 2, 'Shift gears to the majestic Mount Fuji. We’ll take a scenic boat tour on Lake Ashi, visit the Hakone Open-Air Museum, and relax in a traditional onsen (hot spring).');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(304, 71, 102, 1, 'accepted'),
(305, 75, 102, 1, 'accepted'),
(306, 78, 102, 1, 'accepted'),
(307, 4, 103, 2, 'accepted'),
(308, 8, 103, 1, 'accepted'),
(309, 14, 103, 1, 'rejected');

-- Trip 5: Japan's Golden Route Adventure
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(48, 71, 'Japan''s Golden Route Adventure', 5, '2026-09-01', '2026-09-22', 'An immersive journey through Japan''s Golden Route. This trip combines the bustling energy of Tokyo, the serene beauty of Mount Fuji, and the historical charm of Kyoto.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(104, 18, 48, '2026-09-01', '2026-09-08', 1, 'Begin the trip in Tokyo. We''ll explore its diverse neighborhoods, from the serene gardens of the Imperial Palace to the vibrant nightlife of Ginza.'),
(105, 20, 48, '2026-09-08', '2026-09-15', 2, 'Next, we will head to Mount Fuji for a week of natural beauty and outdoor activities. We’ll visit the Aokigahara Forest and take in the views from Lake Kawaguchi.'),
(106, 19, 48, '2026-09-15', '2026-09-22', 3, 'Conclude the trip with an in-depth exploration of Kyoto. We’ll visit the famous temples, historic districts, and experience the city''s peaceful atmosphere.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(310, 21, 104, 2, 'accepted'),
(311, 29, 104, 1, 'accepted'),
(312, 33, 104, 1, 'rejected'),
(313, 38, 105, 2, 'accepted'),
(314, 44, 105, 1, 'accepted'),
(315, 49, 105, 1, 'accepted'),
(316, 56, 106, 2, 'accepted'),
(317, 60, 106, 1, 'accepted'),
(318, 65, 106, 1, 'rejected');

-- Trip 1: The Classic Greece Tour
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(49, 42, 'Classic Greece Tour', 4, '2025-06-15', '2025-07-06', 'A comprehensive journey through Greece, from the historical heart of Athens to the iconic beauty of Santorini and the vibrant beaches of Mykonos.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(107, 22, 49, '2025-06-15', '2025-06-22', 1, 'Explore the ancient history of Athens. We''ll visit the Acropolis, the Parthenon, and the historic Plaka neighborhood.'),
(108, 21, 49, '2025-06-22', '2025-06-29', 2, 'Travel to Santorini to enjoy its famous sunsets, whitewashed villages, and stunning caldera views. We''ll explore Oia and Fira.'),
(109, 23, 49, '2025-06-29', '2025-07-06', 3, 'End the trip in Mykonos, where we’ll relax on the beach, explore Little Venice, and enjoy the lively nightlife.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(319, 15, 107, 2, 'accepted'),
(320, 25, 107, 1, 'accepted'),
(321, 35, 107, 1, 'rejected'),
(322, 40, 108, 1, 'accepted'),
(323, 50, 108, 2, 'accepted'),
(324, 60, 108, 1, 'accepted'),
(325, 65, 109, 2, 'accepted'),
(326, 75, 109, 1, 'accepted'),
(327, 80, 109, 1, 'rejected');

-- Trip 2: Greek Island Hopping
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(50, 79, 'Greek Island Hopping', 3, '2026-08-10', '2026-08-24', 'A true island-hopping experience focusing on two of Greece''s most famous islands. This trip combines the romantic beauty of Santorini with the vibrant fun of Mykonos.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(110, 21, 50, '2026-08-10', '2026-08-17', 1, 'Spend a week in Santorini, exploring the unique beaches, enjoying a wine tasting tour, and capturing stunning photos of the island''s iconic architecture.'),
(111, 23, 50, '2026-08-17', '2026-08-24', 2, 'Head to Mykonos to enjoy its energetic party scene, relax on its famous beaches, and explore the charming windmills and narrow streets of Mykonos Town.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(328, 5, 110, 1, 'accepted'),
(329, 10, 110, 1, 'accepted'),
(330, 20, 110, 1, 'rejected'),
(331, 30, 111, 2, 'accepted'),
(332, 45, 111, 1, 'accepted'),
(333, 55, 111, 1, 'accepted');

-- Trip 3: History & Romance Getaway
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(51, 1, 'History & Romance Getaway', 5, '2027-09-01', '2027-09-15', 'A captivating journey that contrasts the historical marvels of Athens with the breathtaking, romantic scenery of Santorini. Perfect for history lovers and couples.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(112, 22, 51, '2027-09-01', '2027-09-08', 1, 'Begin the trip in Athens, immersing in its rich past. We''ll explore the Acropolis Museum, the Ancient Agora, and the Temple of Olympian Zeus.'),
(113, 21, 51, '2027-09-08', '2027-09-15', 2, 'Travel to Santorini for a week of relaxation and romance. We''ll take a catamaran cruise around the caldera, visit a local winery, and watch the legendary sunset from Oia.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(334, 4, 112, 2, 'accepted'),
(335, 8, 112, 1, 'accepted'),
(336, 12, 112, 1, 'accepted'),
(337, 18, 113, 2, 'accepted'),
(338, 22, 113, 1, 'accepted'),
(339, 28, 113, 1, 'rejected');

-- Trip 4: Athens & Mykonos: The City & Party Trip
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(52, 80, 'Athens & Mykonos: The City & Party Trip',4, '2025-05-20', '2025-06-03', 'A dynamic trip that combines the rich history of Athens with the energetic nightlife and beautiful beaches of Mykonos. It''s a perfect balance of culture and fun.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(114, 22, 52, '2025-05-20', '2025-05-27', 1, 'Explore the stunning sights of Athens, including the Acropolis, the Temple of Poseidon at Cape Sounion, and the charming streets of Plaka.'),
(115, 23, 52, '2025-05-27', '2025-06-03', 2, 'Shift gears to Mykonos. We''ll spend our days lounging at famous beaches like Paradise Beach and our nights enjoying the island''s world-renowned party scene.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(340, 32, 114, 1, 'accepted'),
(341, 38, 114, 1, 'accepted'),
(342, 44, 114, 1, 'accepted'),
(343, 49, 115, 2, 'accepted'),
(344, 56, 115, 1, 'accepted'),
(345, 62, 115, 1, 'rejected');

-- Trip 5: The Ultimate Greek Escape
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(53, 25, 'Ultimate Greek Escape',5, '2026-07-01', '2026-07-22', 'An immersive journey designed to hit the best of Greece''s highlights. We''ll visit Athens, Mykonos, and Santorini for a full Greek experience.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(116, 22, 53, '2026-07-01', '2026-07-08', 1, 'Begin the trip in Athens to explore its ancient ruins and modern cultural scene.'),
(117, 23, 53, '2026-07-08', '2026-07-15', 2, 'Next, we''ll fly to Mykonos to enjoy the beautiful beaches, iconic windmills, and lively nightlife.'),
(118, 21, 53, '2026-07-15', '2026-07-22', 3, 'Conclude the trip with a week in Santorini, taking in the world-famous sunsets, exploring charming villages, and relaxing by the sea.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(346, 68, 116, 2, 'accepted'),
(347, 72, 116, 1, 'accepted'),
(348, 76, 116, 1, 'rejected'),
(349, 1, 117, 2, 'accepted'),
(350, 7, 117, 1, 'accepted'),
(351, 11, 117, 1, 'accepted'),
(352, 19, 118, 2, 'accepted'),
(353, 29, 118, 1, 'accepted'),
(354, 39, 118, 1, 'rejected');

-- Trip 1: The Cross-Canada Explorer
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(54, 19, 'Cross-Canada Explorer', 4, '2025-08-05', '2025-08-26', 'An unforgettable journey across Canada, showcasing the urban culture of Toronto, the stunning landscapes of Banff, and the Pacific charm of Vancouver.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(119, 26, 54, '2025-08-05', '2025-08-12', 1, 'Explore the multicultural city of Toronto. We’ll visit the CN Tower, explore the Distillery District, and enjoy a day trip to Niagara Falls.'),
(120, 24, 54, '2025-08-12', '2025-08-19', 2, 'Journey to Banff National Park for hiking, sightseeing, and enjoying the breathtaking views of Lake Louise and Moraine Lake.'),
(121, 25, 54, '2025-08-19', '2025-08-26', 3, 'Conclude the trip in Vancouver, where we''ll explore Stanley Park, Gastown, and take a ferry to Granville Island Market.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(355, 15, 119, 2, 'accepted'),
(356, 25, 119, 1, 'accepted'),
(357, 35, 119, 1, 'rejected'),
(358, 40, 120, 1, 'accepted'),
(359, 50, 120, 2, 'accepted'),
(360, 60, 120, 1, 'accepted'),
(361, 65, 121, 2, 'accepted'),
(362, 75, 121, 1, 'accepted'),
(363, 80, 121, '1', 'rejected');

-- Trip 2: Western Canada Adventure
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(55, 78, 'Western Canada Adventure',3, '2026-07-10', '2026-07-24', 'A perfect journey combining the stunning Rocky Mountains with the vibrant coastal city of Vancouver. This trip is all about nature and urban exploration.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(122, 24, 55, '2026-07-10', '2026-07-17', 1, 'Spend a week in Banff National Park, hiking, taking scenic drives, and exploring the charming town of Banff.'),
(123, 25, 55, '2026-07-17', '2026-07-24', 2, 'Travel to Vancouver to enjoy its mix of natural beauty and urban excitement. We''ll explore Grouse Mountain and the city''s diverse culinary scene.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(364, 5, 122, 1, 'accepted'),
(365, 10, 122, 1, 'accepted'),
(366, 20, 122, 1, 'rejected'),
(367, 30, 123, 2, 'accepted'),
(368, 45, 123, 1, 'accepted'),
(369, 55, 123, 1, 'accepted');

-- Trip 3: The Urban Canada Tour
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(56, 56, 'Urban Canada Tour',5, '2027-04-10', '2027-04-24', 'A captivating journey that contrasts the cultural hub of Toronto with the scenic coastal city of Vancouver, showcasing the diversity of Canada''s urban centers.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(124, 26, 56, '2027-04-10', '2027-04-17', 1, 'Begin the trip in Toronto, exploring its famous landmarks, diverse neighborhoods like Kensington Market, and its world-class art and food scene.'),
(125, 25, 56, '2027-04-17', '2027-04-24', 2, 'Travel to Vancouver to enjoy a more relaxed, nature-focused urban experience. We''ll explore the city''s beautiful parks and take a walk along the seawall.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(370, 4, 124, 2, 'accepted'),
(371, 8, 124, 1, 'accepted'),
(372, 12, 124, 1, 'accepted'),
(373, 18, 125, 2, 'accepted'),
(374, 22, 125, 1, 'accepted'),
(375, 28, 125, 1, 'rejected');

-- Trip 4: Mountains & Metropolis Getaway
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(57, 43, 'Mountains & Metropolis Getaway', 4, '2025-09-15', '2025-09-29', 'A trip offering a dramatic contrast between the majestic Rocky Mountains and the bustling urban energy of Toronto. It''s a perfect mix of peace and excitement.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(126, 24, 57, '2025-09-15', '2025-09-22', 1, 'Enjoy a week in Banff National Park, hiking trails, taking in the serene beauty of the lakes, and wildlife spotting.'),
(127, 26, 57, '2025-09-22', '2025-09-29', 2, 'Fly to Toronto for a week of city exploration. We''ll visit the Royal Ontario Museum, take in a Blue Jays game, and experience the city''s diverse culinary scene.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(376, 32, 126, 1, 'accepted'),
(377, 38, 126, 1, 'accepted'),
(378, 44, 126, 1, 'accepted'),
(379, 49, 127, 2, 'accepted'),
(380, 56, 127, 1, 'accepted'),
(381, 62, 127, 1, 'rejected');

-- Trip 5: The Ultimate Canadian Journey
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(58, 55, 'Ultimate Canadian Journey',5, '2026-06-01', '2026-06-22', 'A complete exploration of Canada''s best, combining the coastal city of Vancouver, the multicultural hub of Toronto, and the stunning natural beauty of Banff.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(128, 25, 58, '2026-06-01', '2026-06-08', 1, 'Begin the trip in Vancouver. We''ll explore the city, visit Granville Island Market, and take a stroll along the seawall.'),
(129, 26, 58, '2026-06-08', '2026-06-15', 2, 'Next, we''ll fly to Toronto to enjoy its arts, culture, and vibrant nightlife.'),
(130, 24, 58, '2026-06-15', '2026-06-22', 3, 'Conclude the trip in Banff National Park for a week of hiking, sightseeing, and enjoying the incredible scenery of the Canadian Rockies.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(382, 68, 128, 2, 'accepted'),
(383, 72, 128, 1, 'accepted'),
(384, 76, 128, 1, 'rejected'),
(385, 1, 129, 2, 'accepted'),
(386, 7, 129, 1, 'accepted'),
(387, 11, 129, 1, 'accepted'),
(388, 19, 130, 2, 'accepted'),
(389, 29, 130, 1, 'accepted'),
(390, 39, 130, 1, 'rejected');

-- Trip 1: The Thailand Grand Tour
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(59, 11, 'Thailand Grand Tour',4, '2026-02-10', '2026-03-03', 'A complete journey through Thailand, showcasing the urban culture of Bangkok, the peaceful charm of Chiang Mai, and the stunning beaches of Phuket.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(131, 51, 59, '2026-02-10', '2026-02-17', 1, 'Explore the dynamic capital of Bangkok. We’ll visit the Grand Palace, Wat Arun, and enjoy a vibrant street food tour.'),
(132, 52, 59, '2026-02-17', '2026-02-24', 2, 'Journey to Chiang Mai to immerse in ancient traditions. We’ll see temples, visit an elephant sanctuary, and explore the night bazaar.'),
(133, 50, 59, '2026-02-24', '2026-03-03', 3, 'Conclude the trip in Phuket, where we''ll relax on the beach, go island hopping to Phi Phi Islands, and enjoy the island''s famous nightlife.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(391, 15, 131, 2, 'accepted'),
(392, 25, 131, 1, 'accepted'),
(393, 35, 131, 1, 'rejected'),
(394, 40, 132, 1, 'accepted'),
(395, 50, 132, 2, 'accepted'),
(396, 60, 132, 1, 'accepted'),
(397, 65, 133, 2, 'accepted'),
(398, 75, 133, 1, 'accepted'),
(399, 80, 133, 1, 'rejected');

-- Trip 2: Northern & Southern Thailand
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(60, 16, 'Northern & Southern Thailand',3, '2025-11-05', '2025-11-19', 'A journey combining the serene, mountainous culture of Northern Thailand with the vibrant, tropical beaches of the South. A perfect blend of relaxation and adventure.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(134, 52, 60, '2025-11-05', '2025-11-12', 1, 'Spend a week in Chiang Mai, exploring the temples and markets within the Old City walls and taking a cooking class to master Thai cuisine.'),
(135, 50, 60, '2025-11-12', '2025-11-19', 2, 'Travel to Phuket to enjoy its stunning beaches, boat tours to nearby islands, and a relaxing day exploring the beautiful Old Town.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(400, 5, 134, 1, 'accepted'),
(401, 10, 134, 1, 'accepted'),
(402, 20, 134, 1, 'accepted'),
(403, 30, 135, 2, 'accepted'),
(404, 45, 135, 1, 'accepted'),
(405, 55, 135, 1, 'rejected');

-- Trip 3: City & Beaches Getaway
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(61, 33, 'City & Beaches Getaway',5, '2027-04-10', '2027-04-24', 'A captivating journey that contrasts the cultural hub of Bangkok with the tropical, coastal beauty of Phuket. The perfect trip for those seeking both urban excitement and relaxation.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(136, 51, 61, '2027-04-10', '2027-04-17', 1, 'Begin the trip in Bangkok, exploring its famous landmarks, diverse markets, and the vibrant nightlife along Sukhumvit Road.'),
(137, 50, 61, '2027-04-17', '2027-04-24', 2, 'Travel to Phuket to enjoy a relaxing, beach-focused experience. We''ll explore Patong Beach, visit the Big Buddha, and take in a stunning sunset at Promthep Cape.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(406, 4, 136, 2, 'accepted'),
(407, 8, 136, 1, 'accepted'),
(408, 12, 136, 1, 'rejected'),
(409, 18, 137, 2, 'accepted'),
(410, 22, 137, 1, 'accepted'),
(411, 28, 137, 1, 'accepted');

-- Trip 4: Culture & Capital Trip
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(62, 77, 'Culture & Capital Trip',4, '2025-09-15', '2025-09-29', 'A trip offering a deep dive into Thailand''s rich culture and urban energy. This is a perfect mix of ancient history and modern city life, from Chiang Mai to Bangkok.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(138, 52, 62, '2025-09-15', '2025-09-22', 1, 'Enjoy a week in Chiang Mai, exploring the Old City temples like Wat Phra Singh and Wat Chedi Luang, and taking a Thai cooking class.'),
(139, 51, 62, '2025-09-22', '2025-09-29', 2, 'Fly to Bangkok for a week of city exploration. We''ll visit the Grand Palace, cruise on the Chao Phraya River, and explore the trendy neighborhoods of the city.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(412, 32, 138, 1, 'accepted'),
(413, 38, 138, 1, 'accepted'),
(414, 44, 138, 1, 'accepted'),
(415, 49, 139, 2, 'accepted'),
(416, 56, 139, 1, 'accepted'),
(417, 62, 139, 1, 'rejected');

-- Trip 5: The Ultimate Thailand Escape
INSERT INTO trip (trip_id, owner_id, trip_name, max_buddies, start_date, end_date, description) VALUES
(63, 23, 'Ultimate Thailand Escape',5, '2026-06-01', '2026-06-22', 'A complete exploration of Thailand''s best, combining the tropical beaches of Phuket, the vibrant hub of Bangkok, and the cultural heart of Chiang Mai.');

INSERT INTO trip_destination (trip_destination_id, destination_id, trip_id, start_date, end_date, sequence_number, description) VALUES
(140, 50, 63, '2026-06-01', '2026-06-08', 1, 'Begin the trip in Phuket. We''ll explore the island''s beautiful beaches, enjoy water sports, and take a trip to the famous James Bond Island.'),
(141, 51, 63, '2026-06-08', '2026-06-15', 2, 'Next, we''ll fly to Bangkok to enjoy its diverse cuisine, iconic temples, and bustling city life.'),
(142, 52, 63, '2026-06-15', '2026-06-22', 3, 'Conclude the trip in Chiang Mai for a week of exploring ancient temples, visiting an ethical elephant sanctuary, and hiking in the surrounding mountains.');

INSERT INTO buddy (buddy_id, user_id, trip_destination_id, person_count, request_status) VALUES
(418, 68, 140, 2, 'accepted'),
(419, 72, 140, 1, 'accepted'),
(420, 76, 140, 1, 'rejected'),
(421, 1, 141, 2, 'accepted'),
(422, 7, 141, 1, 'accepted'),
(423, 11, 141, 1, 'accepted'),
(424, 19, 142, 2, 'accepted'),
(425, 29, 142, 1, 'accepted'),
(426, 39, 142, 1, 'rejected');

-- ------------------------------------------------------------------------------------------------------------------------------------------------

-- =====================================
-- 1. Private Conversations (No Trip Destination)
-- =====================================

-- Convo 1: Sarah Wagner (ID 26) and Kimberly Davenport (ID 28)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (1, NULL, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (1, 26), (1, 28);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(26, 'Hey Kim, I was wondering if you''ve ever used this app to plan a trip with someone?', '2025-09-24 09:00:00', 1),
(28, 'Hi Sarah, yes I have! It''s a great way to meet like-minded people. What are you looking to do?', '2025-09-24 09:02:00', 1),
(26, 'I''m thinking of a solo trip but open to a travel buddy if the right person comes along. Have you had any luck with that?', '2025-09-24 09:05:00', 1);

-- Convo 2: Mandy Green (ID 27) and William Sanchez (ID 59)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (2, NULL, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (2, 27), (2, 59);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(59, 'Hey Mandy, I saw you were interested in hiking. Are you planning any big trips?', '2025-08-14 10:30:00', 2),
(27, 'Hi William, not at the moment, but I''m always looking for new trails. Have you been to any good ones lately?', '2025-08-14 13:12:00', 2),
(59, 'I just got back from a great trip to Zion National Park. The trails there are amazing! I can send you some photos if you want to see them.', '2025-08-14 18:35:00', 2);

-- Convo 3: Zoe Ramirez (ID 50) and Jack Murphy (ID 55)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (3, NULL, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (3, 50), (3, 55);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(50, 'Hey Jack, you mentioned you like to visit art museums. Are you planning any trips to a city with a good art scene?', '2025-07-02 14:00:00', 3),
(55, 'Hi Zoe, I''m thinking about Paris next year. The Louvre is a must-see for me!', '2025-07-02 14:02:00', 3),
(50, 'That''s on my bucket list, too! Let me know if you want to swap notes on good spots. I''ve found a few local galleries that look incredible.', '2025-07-03 09:15:00', 3);

-- Convo 4: Daniel Evans (ID 51) and Grace Powell (ID 60)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (4, NULL, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (4, 51), (4, 60);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(51, 'Hi Grace, I saw your profile and we have similar interests in adventure travel.', '2025-05-10 11:30:00', 4),
(60, 'Oh, that''s great! What kind of adventure are you into?', '2025-05-10 11:32:00', 4),
(51, 'Mostly rock climbing and surfing. I''m looking for a new place to try both. Do you have any suggestions?', '2025-05-11 08:20:00', 4);

-- Convo 5: Mason Clark (ID 43) and Julian White (ID 61)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (5, NULL, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (5, 43), (5, 61);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(61, 'Hey Mason, I''m new to the app. Any tips on finding a good travel buddy?', '2025-01-18 16:40:00', 5),
(43, 'Just be honest about what you''re looking for and check out people''s profiles. It worked for me!', '2025-01-18 17:41:00', 5),
(61, 'Thanks, that''s helpful! I''m hoping to find someone to go to a music festival with. Do you know of any festivals coming up?', '2025-01-19 09:45:00', 5);

-- =====================================
-- 2. Private Conversations (Trip-Specific)
-- =====================================

-- Convo 6: Trip to Paris (TD 1, Owner: ID 5, Buddy: ID 24)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (6, 1, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (6, 5), (6, 24);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(5, 'So excited for our Paris trip! Did you hear back about the Louvre tickets?', '2025-06-16 10:00:00', 6),
(24, 'Not yet, still waiting. But I''ve got a list of the best cafes we have to try!', '2025-06-16 10:01:00', 6),
(5, 'Perfect! That''s exactly what I wanted to hear. Can''t wait to see your list. I''m ready for some macarons!', '2025-06-16 10:05:00', 6);

-- Convo 7: Trip to Nice (TD 2, Owner: ID 5, Buddy: ID 70)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (7, 2, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (7, 5), (7, 70);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(70, 'Hey Michele, excited about Nice! I found a great spot for sunset photos on the Promenade des Anglais.', '2025-06-23 11:30:00', 7),
(5, 'Oh, that''s awesome! Let''s make sure we go there. Have you checked the weather forecast? I hope it''s sunny!', '2025-06-23 11:32:00', 7),
(70, 'Yep, looks like it''s going to be perfect. No clouds in sight!', '2025-06-23 11:35:00', 7);

-- Convo 8: Trip to Paris (TD 3, Owner: ID 4, Buddy: ID 8)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (8, 3, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (8, 4), (8, 8);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(8, 'Hi Kim, I''m so excited for our Parisian food trip! Have you started looking at restaurants yet?', '2025-03-06 14:15:00', 8),
(4, 'I have a few in mind! I found a great bistro near our hotel. The reviews are fantastic.', '2025-03-06 14:16:00', 8),
(8, 'Nice! Let''s make a list and start making reservations soon. I hear it can get busy.', '2025-03-07 10:20:00', 8);

-- Convo 9: Trip to Honolulu (TD 21, Owner: ID 10, Buddy: ID 63)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (9, 21, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (9, 10), (9, 63);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(63, 'Hey Juan, I can''t wait for our Honolulu trip! I''ve been looking at some of the best spots for surfing.', '2025-07-12 09:00:00', 9),
(10, 'I know! I found a good deal on a surfing lesson for both of us if you''re interested. Do you have any gear we should bring?', '2025-07-12 13:45:00', 9),
(63, 'Sounds good to me. Let''s book it! I think all the gear is included in the lesson price.', '2025-07-13 08:10:00', 9);

-- Convo 10: Trip to New York City (TD 28, Owner: ID 73, Buddy: ID 33)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (10, 28, FALSE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (10, 73), (10, 33);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(33, 'Hey Nathaniel, for our NYC trip, have you looked into Broadway shows at all?', '2025-02-04 16:40:00', 10),
(73, 'I have! I''m hoping to catch a matinee. Have you got a preference?', '2025-02-04 17:05:00', 10),
(33, 'I''m open to anything. Let me know what you find! I''m also interested in checking out some jazz clubs. Do you know of any good ones?', '2025-02-05 09:15:00', 10);

-- =====================================
-- 3. Group Conversations (Trip-Specific)
-- =====================================
-- Convo 11: Trip to Phuket (TD 140, Owner: ID 23, Buddies: ID 49, 56, 62)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (11, 140, TRUE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (11, 23), (11, 49), (11, 56), (11, 62);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(23, 'Welcome everyone to the Phuket trip chat! I''m so excited for our Ultimate Thailand Escape.', '2025-06-02 10:00:00', 11),
(49, 'Hey guys! I''m ready to unwind on the beautiful beaches. Have we decided on a resort yet?', '2025-06-02 13:45:00', 11),
(56, 'I''ve been looking at all the water sports we can do. Can''t wait! Do you think we''ll have time for a full day of snorkeling?', '2025-06-03 09:20:00', 11),
(62, 'I''ve already started a list of islands we should visit. I think a trip to Phi Phi Island is a must!', '2025-06-03 18:10:00', 11);

-- Convo 12: Trip to Bangkok (TD 141, Owner: ID 23, Buddies: ID 49, 56, 62)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (12, 141, TRUE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (12, 23), (12, 7), (12, 11), (12, 1);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(23, 'Next up, Bangkok! We can coordinate our visits to temples and night markets here.', '2025-06-09 11:00:00', 12),
(11, 'I can''t wait to see the Grand Palace! What''s the best way to get around the city, do you think?', '2025-06-09 14:30:00', 12),
(1, 'I''m ready to eat all the street food! I found a food tour that looks awesome. Should I book it?', '2025-06-10 08:15:00', 12),
(7, 'I found a great walking tour we could do! I think we should also try to fit in a river cruise.', '2025-06-10 19:05:00', 12);

-- Convo 13: Trip to Chiang Mai (TD 142, Owner: ID 23, Buddies: ID 49, 56, 62)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (13, 142, TRUE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (13, 23), (13, 49), (13, 56), (13, 62);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(23, 'Final stop: Chiang Mai! Let''s discuss our final week here.', '2025-06-16 13:00:00', 13),
(49, 'I''m so ready for a relaxing end to the trip. I''ve heard the massages are incredible! Should we book one for our last day?', '2025-06-16 17:20:00', 13),
(56, 'I found a good elephant sanctuary we can volunteer at. It''s an ethical one. What do you all think?', '2025-06-17 09:10:00', 13),
(62, 'Let''s make a plan to visit the night bazaar! I heard it''s the best place to get souvenirs.', '2025-06-17 20:45:00', 13);

-- Convo 14: Trip to Los Angeles (TD 19, Owner: ID 26, Buddies: ID 25, 28, 59)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (14, 19, TRUE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (14, 26), (14, 25), (14, 28), (14, 59);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(26, 'Hey team, let''s plan for our time in Los Angeles!', '2025-05-17 11:00:00', 14),
(25, 'I am ready to tour Hollywood and walk the Walk of Fame! Have you guys looked at any celebrity house tours?', '2025-05-17 11:01:00', 14),
(28, 'I can''t wait to explore the diverse neighborhoods. I''m especially looking forward to the art scene in the Arts District.', '2025-05-17 11:02:00', 14),
(59, 'It''s going to be so much fun! I also found a great spot for tacos near Venice Beach.', '2025-05-17 11:03:00', 14);

-- Convo 15: Trip to Los Angeles (TD 23, Owner: ID 32, Buddies: ID 43, 74)
INSERT INTO conversation (conversation_id, trip_destination_id, is_group) VALUES (15, 23, TRUE);
INSERT INTO conversation_participant (conversation_id, user_id) VALUES (15, 32), (15, 43), (15, 74);
INSERT INTO message (sender_id, content, sent_at, conversation_id) VALUES
(32, 'Alright team, now for Los Angeles!', '2025-02-08 11:30:00', 15),
(43, 'I am so excited to visit Universal Studios! Is it better to go during the week or on a weekend?', '2025-02-08 15:10:00', 15),
(74, 'I am looking forward to hiking to the Hollywood Sign! I found a trail that has great views.', '2025-02-09 09:25:00', 15);

-- ------- IMMEDIATE EXECUTION OF EVENTS (for testing seed data)
UPDATE conversation
SET is_archived = TRUE
WHERE conversation_id IN (
    SELECT m.conversation_id
    FROM message m
    GROUP BY m.conversation_id
    HAVING MAX(m.sent_at) <= NOW() - INTERVAL 1 WEEK
);

UPDATE trip SET is_archived = true WHERE end_date < NOW();

UPDATE trip_destination SET is_archived = true WHERE end_date < NOW();