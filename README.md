# Project Overview

This is a standard .NET 8 Web API project utilizing a clean, layered architecture. It separates the core application logic (API, Features) from data access and infrastructure concerns (Database). This structure is designed to be scalable, maintainable, and easy to test, serving as a solid foundation for future development.

## Architecture

The solution follows a multi-project structure to enforce separation of concerns:

### 1. Web API Project (Entry Point)
* **Purpose**: Acts as the main entry point for the application. It handles HTTP requests, routing, middleware configuration, and dependency injection.
* **Key Components**:
  * `Program.cs`: Configures services (DI) and the HTTP request pipeline.
  * `appsettings.json`: Stores application configuration and connection strings.
  * `Features/` or `Controllers/`: Contains the API endpoints. We recommend a vertical slice architecture grouping by feature (e.g., Auth, Product, User), but standard MVC controllers can also be used.
* **Responsibilities**:
  * Request validation.
  * Returning appropriate HTTP status codes and responses.
  * Wiring up services.

### 2. Database Project (Class Library)
* **Purpose**: Encapsulates all data access logic and database schemas.
* **Key Components**:
  * `AppDbContext.cs`: The Entity Framework Core database context.
  * `Models/` or `Entities/`: The C# classes that map to database tables.
  * `Migrations/`: Contains EF Core database migrations (if using a relational database).
* **Responsibilities**:
  * Defining the database schema.
  * Handling data querying and persistence.
  * Keeping EF Core dependencies isolated from the main Web API logic where possible.

## Getting Started

### Prerequisites
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* A suitable database provider (e.g., SQL Server, PostgreSQL, or In-Memory for testing)

### Setup & Run
1. **Restore dependencies**:
   ```bash
   dotnet restore
   ```
2. **Database Configuration**:
   Update the connection string in the API project's `appsettings.json` to point to your local database instance.
3. **Apply Migrations** (If applicable):
   ```bash
   dotnet ef database update --project <YourDatabaseProjectName> --startup-project <YourApiProjectName>
   ```
4. **Run the Application**:
   Navigate to the API project directory and run:
   ```bash
   dotnet watch run
   ```
   Alternatively, use your IDE's run/debug capabilities.

### API Documentation
When running in Development mode, the API documentation is typically available via Swagger UI. Navigate to `/swagger` in your browser (e.g., `https://localhost:<port>/swagger`).
