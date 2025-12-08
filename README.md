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
  * **Bash terminal**
  * **Database Credentials**: Connection strings must be updated in the C\# project configuration files to point to the **local** instances (e.g., `localhost` or `127.0.0.1`).

-----

#### Add the env variables

1.  **Create .env file:** Inside `src/TravelBuddy.Migrator` create a .env file with following variables and update values to your local information:
```bash
MYSQL_HOST=localhost
MYSQL_PORT=3307
MYSQL_USER=migrator
MYSQL_PASSWORD=migrator_password
MYSQL_DATABASE=travel_buddy

MONGO_CONNECTION=mongodb://root:password@localhost:27017
MONGO_DATABASE=travel_buddy_mongo

NEO4J_URI=bolt://localhost:7687
NEO4J_USER=neo4j
NEO4J_PASSWORD=password
```

2. **Add connection strings:** Inside `src/TravelBuddy.Api` run following in a bash terminal

Setup env ConnectionStrings, but remember to update password (and the other values if you are not using the default ones):
```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3307;Database=travel_buddy;User=backend_api;Pwd=backend_api_password;"

dotnet user-secrets set "ConnectionStrings:MongoDbConnection" "mongodb://root:password@localhost:27017/travel_buddy?authSource=admin"

dotnet user-secrets set "ConnectionStrings:Neo4jUri" "bolt://localhost:7687"
dotnet user-secrets set "ConnectionStrings:Neo4jUser" "neo4j"
dotnet user-secrets set "ConnectionStrings:Neo4jPassword" "password"
```
3. To access our endpoint with external API for weather (Visual Crossing) either add your own api key or find ours in the report. Change `API_KEY` before running the code below:
```bash
dotnet user-secrets set "VisualCrossing:ApiKey" "API_KEY"
```

-----

### 2. Run the databases and migrator

1.  **Navigate** to the migrator project folder:
    ```bash
    cd src/TravelBuddy.Migrator
    ```
2.  **Execute** the code below to set up the databases and run the migrator to populate the mongodb and neo4j databases.
    ```bash
    docker-compose rm -sf test_mysql test_mongodb test_neo4j migrator && \
    docker-compose up -d test_mysql test_mongodb test_neo4j && \
    docker-compose up --build migrator
    ```
    Wait for the process to complete.

### 3. Run the API

1.  **Navigate** to the API project folder:
    ```bash
    cd src/TravelBuddy.Api
    ```
2.  **Execute** the API service:
    ```bash
    dotnet run
    ```
    The API should start up, connecting to your local database instances.

-----

### 5. Final Verification

  * Check the console output for `TravelBuddy.Api` to confirm it is running.
  * Verify the data copied by the migrator is present in the target collections on the **local MongoDB** instance.
  * Test the API endpoints to ensure full operational capabilities using your isolated environment.

