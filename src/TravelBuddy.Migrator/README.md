# TravelBuddy Migrator

This migrator sets up and populates MongoDB and Neo4j databases for the TravelBuddy project using the MySQL database. It is designed to run automatically as a Docker container alongside your database containers.

## Features
- Migrates data from MySQL to MongoDB and Neo4j
- Can be run manually or automatically via Docker Compose
- Resets MongoDB and Neo4j data on container restart (using tmpfs)

## Prerequisites
- Docker and Docker Compose installed
- The TravelBuddy project cloned

## Files
- `Dockerfile`: Builds the migrator as a .NET container
- `Program.cs`: Main migration logic
- `docker-compose.yml`: Defines all services (MySQL, MongoDB, Neo4j, Migrator)

## Usage

### 1. Build and Start All Containers
From the project root (where `docker-compose.yml` is located):

```bash
# Build and start all services (MySQL, MongoDB, Neo4j, Migrator)
docker-compose up --build
```

- The migrator will run automatically after the databases are ready.
- It will populate MySQL, MongoDB, and Neo4j with default data.

### 2. Resetting Data
To reset MongoDB and Neo4j to their default state:

```bash
# Stop and remove the database containers
docker-compose rm -sf test_mysql test_mongodb test_neo4j migrator
# Start them again (data will be wiped)
docker-compose up -d test_mysql test_mongodb test_neo4j
# Re-run the migrator
# (if not run automatically, you can run it manually)
docker-compose up --build migrator
```

### 3. Manual Migrator Run
You can run the migrator manually at any time:

```bash
docker-compose up --build migrator
```

## Environment Variables
Make sure your `appsettings.json` file use the correct hostnames and credentials for Docker Compose:

