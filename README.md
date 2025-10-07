# TravelBuddy
Final project for Databases for Developers

## Database Installation Guide (MySQL 8.0+)
Here is how to import the database into a test environment with full functionality.

### Prerequisites
- MySQL Server v8.0 or later
- MySQL client or IDE (e.g., MySQL Workbench)
- SQL files (found under the `mysql/` folder):
  - 1_create_db_tables.sql
  - 2_stored_objects.sql
  - 3_seed_data.sql

### Installation Steps
1. **Open your MySQL client or IDE** and connect to your server.
2. **Create schema**. Run 1_create_db_tables.sql to set up the database and tables.
3. **Add stored logic**. Run 2_stored_objects.sql to create procedures, views, etc.
4. **Insert test data**. Run 3_seed_data.sql to populate the database.

> Each file includes `USE travel_buddy`, so no need to select the schema manually.

### Verification
- Confirm that the `travel_buddy` schema exists.
- Confirm all tables and stored objects exist.
- Run sample queries to test functionality. The `mysql/simple_queries.sql` file have some basic queries that could be used.
