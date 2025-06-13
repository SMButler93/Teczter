# Teczter

**Teczter** is a free, lightweight manual regression testing tool. It allows users to create and assign tests, execute them, and track test statuses. The goal of this project is to provide an accessible alternative to expensive commercial testing platforms.

---

## 📖 Overview

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
- Become immutable once closed (no further changes allowed)

Only admins can manage execution groups.

### 🔹 User
Users can be assigned executions to complete. User roles determine access rights, such as:
- Editing test or step details
- Assigning executions

---

## 🛠️ Technology Stack

- **Backend**: [.NET 9](https://dotnet.microsoft.com/)
- **Frontend**: [Angular](https://angular.io/)
- **UI Library**: [PrimeNG](https://www.primefaces.org/primeng/)

### 📦 Libraries & Tools

- `FluentValidation`
- `Moq`
- `NUnit`
- `Serilog`
- `Shouldly`

### 📄 Database

- Managed via **SSDT** (SQL Server Data Tools)
- Originally developed using **EF Core Code-First**, but migrated in **May 2025**
- ORM: **Entity Framework Core**

---

## 🚧 Planned Work

This project is actively being developed. Upcoming tasks include:

- Implement authentication and authorization
- Build an MVP user interface
- Add email notifications
- Introduce domain events and CQRS where appropriate

---

## 🤝 Contributing

Teczter is a personal project, but contributions or feedback are welcome. If you're interested in helping out, feel free to raise an issue or submit a pull request!

