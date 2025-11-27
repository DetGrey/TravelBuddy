# TravelBuddy
Final project for Databases for Developers

## Project structure
```
/travel_buddy/
├── src/
│   ├── TravelBuddy.Api/                        <-- ASP.NET Core Web API Host (Monolith Entry)
│   │   ├── Auth/                                # JWT tokens etc.
│   │   ├── Controllers/                         # API endpoints (e.g., UsersController)
│   │   └── Program.cs                           # Startup and DI configuration
│   │
│   ├── TravelBuddy.Migrator/                   <-- Migrator script from MySQL to MongoDB
│   │   ├── Models/                              # Document models for MongoDB
│   │   └── Program.cs                           # Migrating script
│   │
│   ├── Modules/
│   │   ├── TravelBuddy.Users/                  <-- Class Library (.NET 8)
│   │   │   ├── Models/                          # User entity
│   │   │   ├── Infrastructure/
│   │   │   │   └── UsersDbContext.cs
│   │   │   ├── DTOs/
│   │   │   ├── IUserRepository.cs
│   │   │   └── UserService.cs
│   │   │
│   │   ├── TravelBuddy.Trips/                  <-- Class Library
│   │   │   ├── Models/                          # Trip, TripAudit, Buddy, etc.
│   │   │   ├── Infrastructure/
│   │   │   │   └── TripsDbContext.cs
│   │   │   ├── DTOs/
│   │   │   └── TripService.cs, etc.
│   │   │
│   │   └── TravelBuddy.Messaging/              <-- Class Library
│   │       ├── Models/                          # Message, Conversation, etc.
│   │       ├── Infrastructure/
│   │       │   └── MessagingDbContext.cs
│   │       ├── DTOs/
│   │       └── MessagingService.cs, etc.
│   │
│   └── Shared/
│       └── TravelBuddy.SharedKernel/           <-- Class Library (Common types)
│           ├── Models/                          # SystemEventLog
│           └── Infrastructure/
│               └── SharedKernelDbContext.cs
│
├── mysql/
│   ├── 1_create_db_tables.sql                  <-- SQL script to initialize the database
│   ├── 2_stored_objects.sql                    <-- SQL script to create our stored objects
│   └── 3_seed_data.sql                         <-- SQL script to insert our generated data
│
└── *.md                                        <-- Markdown files for our notes
```


## Installation Guide: Local Test Environment Setup

This procedure details how to set up a fully independent test environment using local database instances for full operational capabilities.

### 1. Code Organization and Prerequisites

Your project components should be organized as follows. This structure assumes you have cloned this TravelBuddy repository to your device.

| Component | Location | Description |
| :--- | :--- | :--- |
| **API** (C\#) | `src/TravelBuddy.Api` | Main C\# application layer. |
| **Migrator** (C\#) | `src/TravelBuddy.Migrator` | C\# tool for MySQL to MongoDB data transfer. |
| **SQL Scripts** | `mysql/` | Setup files for the MySQL database. |

#### Prerequisites (Updated for Local Test Environment)

  * **.NET SDK** (e.g., .NET 8 or later)
  * **Local MySQL Server** (v8.0+ running locally or via Docker)
  * **Local MongoDB Server** (running locally or via Docker)
  * **MySQL Client/IDE** (e.g., MySQL Workbench)
  * **SQL files**: `1_create_db_tables.sql`, `2_stored_objects.sql`, `3_seed_data.sql`
  * **Database Credentials**: Connection strings must be updated in the C\# project configuration files to point to the **local** instances (e.g., `localhost` or `127.0.0.1`).

-----

### 2. Local Database Import (MySQL)

Follow these steps to set up the relational database schema and data on your **local MySQL instance**.

1.  **Start Local MySQL:** Ensure your local MySQL server is running.
2.  **Connect Locally:** Open your MySQL client and connect using your local server credentials (e.g., `localhost:3306`).
3.  **Run Setup Scripts (in order):** Execute the SQL files located in the `mysql/` folder sequentially:
      * **Schema & Tables:** Run `1_create_db_tables.sql`.
      * **Stored Logic:** Run `2_stored_objects.sql`.
      * **Seed Data:** Run `3_seed_data.sql`.

> **Verification:** Confirm the `travel_buddy` schema, tables, and stored procedures exist on the local MySQL instance.

-----

### 3. Local MongoDB Preparation

The MongoDB server only needs to be running locally, as the Migrator handles the initial data population.

1.  **Start Local MongoDB:** Ensure your local MongoDB server is running.
2.  **Create .env file:** Inside `src/TravelBuddy.Migrator` create a .env file with following variables and update values to your local information:
```bash
MYSQL_HOST=localhost
MYSQL_PORT=3306
MYSQL_USER=root
MYSQL_PASSWORD=YOUR_PASSWORD
MYSQL_DATABASE=travel_buddy

MONGO_CONNECTION=mongodb://localhost:27017
MONGO_DATABASE=travel_buddy_mongo
```

-----

### 4. Component Launch (Migrator & API)

With the local databases configured and running, launch the application components.

#### A. Run the Migrator

1.  **Navigate** to the migrator project folder:
    ```bash
    cd src/TravelBuddy.Migrator
    ```
2.  **Execute** the migrator. This reads data from the local MySQL instance and populates the local MongoDB instance.
    ```bash
    dotnet run
    ```
    Wait for the Migrator process to complete.

#### B. Run the API

1.  **Navigate** to the API project folder:
    ```bash
    cd src/TravelBuddy.Api
    ```
2.  **Setup env ConnectionStrings** for MySql connection but remember to update password (and the other values if you are not using the default ones):
    ```bash
    dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=127.0.0.1;Port=3306;Database=travel_buddy;User=root;Password=YOUR_PASSWORD;"
    ```
3.  **Execute** the API service:
    ```bash
    dotnet run
    ```
    The API should start up, connecting to your local database instances.

-----

### 5. Final Verification

  * Check the console output for `TravelBuddy.Api` to confirm it is running.
  * Verify the data copied by the migrator is present in the target collections on the **local MongoDB** instance.
  * Test the API endpoints to ensure full operational capabilities using your isolated environment.

