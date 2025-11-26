## 1\. Local Development: The Secret Manager Tool 🔑 (Recommended)

For your local machine, the **Secret Manager** is the simplest and safest solution. It stores your secrets in a separate, hidden JSON file on your computer, outside of your project directory, which is never committed to Git.

Your `Program.cs` code will automatically load these secrets during local development.

### Steps to Implement Secret Manager:

1.  **Initialize Secrets** (Run this command in your terminal from the directory containing `TravelBuddy.Api.csproj`):

    ```bash
    dotnet user-secrets init
    ```

2.  **Set Your Connection String:** Use the tool to store your secret. This value is now hidden on your machine.

    ```bash
    dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=YOUR_SERVER;Database=travelbuddy_db;Uid=YOUR_USER;Pwd=YOUR_PASSWORD"
    dotnet user-secrets set "ConnectionStrings:MongoDbConnection" "mongodb+srv://<db_username>:<db_password>@travelbuddycluster.zfwb35g.mongodb.net/?appName=TravelBuddyCluster"
    dotnet user-secrets set "ConnectionStrings:Neo4jUri" "bolt://localhost:7687"
    dotnet user-secrets set "ConnectionStrings:Neo4jUser" "neo4j"
    dotnet user-secrets set "ConnectionStrings:Neo4jPassword" "your_secure_password"
    ```

    *Replace the example string with your actual MySQL connection string.*

### How Your `appsettings.json` Looks

After setting the secret, you should **remove the connection string value** from your `appsettings.json`. The framework will still look for the key but override the empty value with the secret from the manager.

```json
// File: appsettings.json
{
  "Logging": {
    // ...
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    // Leave the value empty or an empty string.
    "DefaultConnection": "" 
  }
}
```

-----

## 2\. Production: Environment Variables ☁️ (Standard Practice)

For production (e.g., Azure, AWS, Docker containers), the safest and most standard way to store secrets is as **System Environment Variables**.

ASP.NET Core automatically reads these variables. You just need to match the configuration key structure, replacing the colon (`:`) separator with a **double underscore (`__`)**.

### Example Environment Variable Setup:

| Configuration Key in Code | Environment Variable Name |
| :--- | :--- |
| `"ConnectionStrings:DefaultConnection"` | `ConnectionStrings__DefaultConnection` |

When you deploy your application to a hosting service (like Azure App Service), you simply enter the connection string into the hosting platform's configuration settings, and it manages the environment variable for you.

-----

By using the **Secret Manager** locally and **Environment Variables** in production, your connection string will never be exposed in your code or configuration files.