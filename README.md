# Teczter

**Teczter** is a lightweight manual regression testing platform designed to help teams create, assign, and execute repeatable test scenarios across product releases without relying on expensive commercial tooling.

The system is built around reusable test definitions that can be executed multiple times through isolated executions, allowing teams to:

- Re-run the same regression tests across releases
- Track pass/fail outcomes over time
- Assign testing responsibilities to users
- Monitor release readiness through execution groups

This enables a clear separation between test definition and execution history, ensuring that updates to a test are centrally managed while historical execution results remain unaffected.

---

## Overview

Below are the core concepts of the system:

### 🔹 Test
A **test** consists of multiple steps that describe a specific process to follow. Each test includes:
- A title
- A description
- A department (indicating ownership)
- Optional URLs (e.g., a starting point)

Tests themselves are not executed directly. Instead, they are executed through **executions** (see below), allowing for test reuse and central updates. Only **admin users** can modify tests.

### 🔹 Test Step
A **test step** is an individual instruction within a test. Each has:
- A step number (ordering)
- Instruction text
- Optional URLs

Each step can be marked as **passed** or **failed**. Only admins can edit test steps.

### 🔹 Execution
An **execution** represents a run-through of a test. It:
- References a specific test
- Records test step results
- Tracks the outcome of the execution

### 🔹 Execution Group
An **execution group** is a collection of executions—often tied to a product or a specific release/version. Execution groups:
- Help track testing progress across releases
- Execution groups become immutable once closed, preserving the integrity of release testing outcomes.

Only admins can manage execution groups.

### 🔹 User
Users can be assigned executions to complete. User roles determine access rights, such as:
- Editing test or step details
- Assigning executions

---

## Demo Mode

You can run the application in demo mode directly from the command line without requiring any external database setup. From the root of the repository (the same directory that contains the solution file), run:

dotnet run --project TeczterApi/Teczter.WebApi -- --demo=true

When demo mode is enabled, the application will automatically create a local SQLite database file with pre-seeded data, allowing API endpoints to be called immediately. On each application startup in demo mode, the SQLite database will be deleted and re-created to ensure a clean, consistent dataset for testing and demonstration purposes.

### 🔹 Running via IDE (Visual Studio / JetBrains Rider)

Demo mode can also be enabled within an IDE such as Visual Studio or JetBrains Rider by selecting the DemoHttp or DemoHttps run/debug profiles. These profiles are preconfigured in the project's launchSettings.json file to pass the required --demo=true argument at startup.

### 🔹 Accessing Swagger

Once the application is running in demo mode, Swagger UI can be accessed at:

http://localhost:5000/swagger (DemoHttp)

https://localhost:5001/swagger (DemoHttps)

---

## Technology Stack

- **Backend**: [.NET 10](https://dotnet.microsoft.com/)
- **Frontend**: `Yet to be decided.`

### Libraries & Tools

- `Entity Framework Core`
- `ASP.NET Core Identity`
- `FluentValidation`
- `Moq`
- `NUnit`
- `Serilog`
- `Shouldly`

### Database

- [PostgreSQL](https://www.postgresql.org/) (Primary Production Database)
- Currently developed and managed via **Entity Framework Core Migrations**
- [SQLite](https://sqlite.org/) (Ephemeral Demo Mode)



---

## Planned Work

This project is actively being developed. Upcoming tasks include:

- Complete role-based authorisation using ASP.NET Core Identity and JWT.
- Build an MVP user interface.
- Add notifications.

---

## Contributing

Teczter is a personal portfolio project, but I welcome collaboration. If you wish to contribute (especially to the frontend) please reach out!

