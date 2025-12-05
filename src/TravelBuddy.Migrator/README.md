# TravelBuddy Migrator

This migrator transfers data from MySQL to MongoDB and Neo4j for the TravelBuddy project. It runs as a .NET console application and can be executed standalone or as a Docker container.

## What It Does

The migrator reads from MySQL and populates both MongoDB and Neo4j with:

**MongoDB (Document Store):**
- Users
- Destinations
- Trips (with embedded trip destinations and buddies)
- Conversations (with embedded participants)
- Messages
- Audit tables (UserAudit, TripAudit, BuddyAudit, ConversationAudit)
- System event logs

**Neo4j (Graph Database):**
- User nodes
- Destination nodes
- Trip nodes with relationships (USER_CREATED_TRIP, TRIP_VISITS_DESTINATION)
- Buddy relationships (USER_IS_BUDDY_WITH)
- Conversation nodes with participant relationships
- Message nodes with creator relationships
- Audit nodes with entity audit chains (HAS_AUDIT, CHANGED)
- System event log nodes

## Prerequisites
- Docker and Docker Compose installed
- The TravelBuddy project cloned

## Files
- `Dockerfile`: Builds the migrator as a .NET container
- `Program.cs`: Main migration logic
- `docker-compose.yml`: Defines all services (MySQL, MongoDB, Neo4j, Migrator)
- `Models/`: Document models for MongoDB collections
- `appsettings.json`: Database connection configuration

## Usage

### Build and Start All Services
From the project root (where `docker-compose.yml` is located):

```bash
# Stop and remove the database containers if they already exist
docker-compose rm -sf test_mysql test_mongodb test_neo4j migrator
# Start the database containers again (data will be wiped)
docker-compose up -d test_mysql test_mongodb test_neo4j
# Run the migrator
docker-compose up --build migrator
```

### Reset testing environments

For MySQL:
- Restart the container and the data will be reset

For MongoDB and Neo4j:
- Restart mysql container (to be sure it only migrates seed data)
- Wait for mysql to be ready (~10 sec)
- Run the migrator while all three db containers are running

### Manual Run
To run the migrator standalone (outside Docker):

```bash
docker-compose up --build migrator
```

## Configuration

The migrator reads connection details from `appsettings.json` which specifies environment variable names to load from `.env`:

```json
{
  "MySql": {
    "Host": "MYSQL_HOST",
    "Port": "MYSQL_PORT",
    "User": "MYSQL_USER",
    "Password": "MYSQL_PASSWORD",
    "Database": "MYSQL_DATABASE"
  },
  "Mongo": {
    "Connection": "MONGO_CONNECTION_STRING",
    "Database": "MONGO_DATABASE"
  },
  "Neo4j": {
    "Uri": "NEO4J_URI",
    "User": "NEO4J_USER",
    "Password": "NEO4J_PASSWORD"
  }
}
```

Ensure `.env` file in the project root contains the actual values for these variables for it to run locally. This is not necessary for the docker containers.

