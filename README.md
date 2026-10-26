# AgroSubsidies

## Overview

# AgroSubsidies

## Overview

AgroSubsidies is an ASP.NET Core Web API project built with .NET 10. The application simulates an agricultural subsidy management system and supports three user roles: **Farmer, Employee, and Admin**.

Farmers can register through the corresponding API endpoint and manage their agricultural parcels. Each parcel contains information such as its area, crop type, ownership type, and regional unit.

Farmers can also submit subsidy applications by selecting a subsidy program and the parcels they want to include. The system supports several subsidy types, including **Basic Support, Eco Scheme, Organic Farming, and Young Farmers** **Livestock** support is planned but has not yet been implemented.

When a farmer submits an application, its initial status is set to UnderReview

An authorized employee or administrator can trigger the evaluation process through a dedicated API endpoint. The system then automatically evaluates the application against predefined business rules and updates its status to either **Approved** or **Rejected**.

For this project, Employee and Admin accounts are created directly in the database rather than through public registration endpoints. This is an intentional design decision for the current implementation.

## Technologies Used

AgroSubsidies was developed using the following technologies and architectural patterns:

### Backend
- **C# / .NET 10** — The primary programming language and development platform used to implement the application's business logic.
- **ASP.NET Core Web API** —  RESTful API endpoints for managing users, agricultural parcels, subsidy programs, and subsidy applications.
- **Entity Framework Core** — An Object-Relational Mapper (ORM) to interact with the database through C# entities and LINQ queries.
- **Microsoft SQL Server** — The relational database management system used to store application data.
- **LINQ** — Used for querying, filtering, and manipulating data.

### Security
- **JWT (JSON Web Tokens)** — Authentication and securing API endpoints.
- **Role-Based Authorization** — Restricts access to specific endpoints based on user roles (Farmer, Employee, Admin).
- **BCrypt** — Hash user passwords before storing them in the database.
- **.NET User Secrets** — Store sensitive development configuration, including database credentials and the JWT signing key.

### Architecture and Design Patterns
- **Repository Pattern** — Data access  and separates database operations from business logic.
- **Unit of Work Pattern** — Coordinates repository operations and database changes.
- **Service Layer** — Validation, business logic and subsidy eligibility evaluation.
- **Dependency Injection** — Used to manage dependencies between controllers, services, and repositories.
- **Data Transfer Objects (DTOs)** — Separate API request and response models from database entities.
- **Global Exception Handling** — Centralized handling of application exceptions.

### Development Tools
- **Visual Studio** — Main development environment.
- **Swagger / OpenAPI** — Used to document and test REST API endpoints.
- **SQL Server Management Studio (SSMS)** — Used for database management and SQL queries.
- **Git & GitHub** — Used for version control and source code hosting.

  
## Key Features

### Authentication & Authorization
- User authentication using JWT tokens.
- Role-based access control for Farmers, Employees, and Admins.
- Password hashing using BCrypt.

### Farmer Features
- Farmer registration and account management.
- Agricultural parcel management, including area, crop type, ownership type, and regional unit.
- Submission of subsidy applications for selected parcels.
- Viewing personal subsidy applications and their evaluation status.

### Employee & Admin Features
- Access to protected subsidy evaluation endpoints.
- Automatic approval or rejection of applications based on predefined eligibility criteria.
- Application evaluation based on subsidy type, parcel area, crop type, and farmer eligibility.

### Subsidy Programs
- **Basic Support**
- **Eco Scheme**
- **Organic Farming**
- **Young Farmers**
- **Livestock** — Planned for future implementation.

## Getting Started

1. Clone the repository and open `AgroSubsidies.sln` in Visual Studio.
2. Configure a SQL Server database and set the connection string and JWT secret key using .NET User Secrets.
3. Apply the Entity Framework Core migrations to create the database schema.
4. Run `WebAppAgroAPI` and use Swagger UI (https://localhost:7282/swagger/index.html) to check and test the available API endpoints.
