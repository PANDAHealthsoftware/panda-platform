# PANDA - Patient Appointment Network Data Application

## Overview

PANDA is a modular, production-quality application designed for managing patient demographics and appointments. It is structured across multiple projects to separate responsibilities and ensure testability, maintainability, and scalability.

This solution was developed as part of the Aire Logic technical assessment and follows modern .NET practices, including layered architecture, authentication, authorization, automated testing, and structured logging.

## Solution Structure

- `PANDA.Api`: ASP.NET Core Web API for appointments and patient management.
- `PANDAView`: Blazor WebAssembly frontend client for interacting with the API.
- `PANDA.Shared`: Shared DTOs, enums, and converters used by both API and frontend.
- `PANDA.Api.Tests`: Test project containing unit and integration tests for the API.

## Key Features

- RESTful API for patient and appointment management
- JWT bearer authentication with role-based access control
- FluentValidation for input DTOs
- AutoMapper for mapping between domain models and DTOs
- SQLite with Entity Framework Core
- Structured logging using Serilog
- Swagger UI for API documentation and testing
- Frontend built with Blazor WebAssembly
- Unit and integration tests (xUnit, NSubstitute, FluentAssertions, bUnit)

## Security and Authentication

### JWT Authentication

The API uses JWT Bearer tokens to authenticate and authorize users. Roles such as `Admin` and `Clinician` are enforced via policies and `[Authorize(Roles = "...")]` attributes.

Include the JWT token in the request header:

```
Authorization: Bearer <your-token>
```

Test tokens can be generated for integration testing using the `JwtTestHelper` in the `PANDA.Api.Tests.Helpers` namespace.

## Setup Instructions

### Prerequisites

- .NET 8 SDK
- Visual Studio / JetBrains Rider / VS Code
- SQLite (optional — file-based DB is used)

### Cloning and Running the API

```bash
git clone https://github.com/your-org/panda-api.git
cd panda-api
dotnet restore
dotnet run --project PANDA.Api
```

The API will start on `https://localhost:5001`. Swagger UI is available at `/swagger`.

## Using the Blazor Frontend (PANDAView)

PANDAView is a Blazor WebAssembly app that interacts with the PANDA API via HTTP using JWT tokens.

### Running the Frontend

```bash
cd PANDAView
dotnet run
```

The app will start at `https://localhost:5002` (or whichever port is configured). Ensure CORS is enabled in the API to allow requests from this origin.

### Authentication from Frontend

1. Obtain a JWT from your auth system or test helper.
2. Store it in browser storage (local/session).
3. Configure `HttpClient` to use the token:
   ```csharp
   httpClient.DefaultRequestHeaders.Authorization =
       new AuthenticationHeaderValue("Bearer", token);
   ```

### Role-Based UI in Blazor

```razor
<AuthorizeView Roles="Admin">
    <button class="btn btn-danger">Delete Appointment</button>
</AuthorizeView>
```

## Testing

### Running Unit & Integration Tests

```bash
cd PANDA.Api.Tests
dotnet test
```

Includes tests for:
- Controllers (mocked + integration)
- Services and validators
- JWT-protected endpoints
- Helper utilities (e.g., JWT token generator)

### Running UI Tests (bUnit)

UI tests for Blazor components can be found in `PANDAView.Tests` (if created). Run with:

```bash
cd PANDAView.Tests
dotnet test
```

## Branching and Contribution Guidelines

### Branch Strategy

- `master`: Production/stable releases only.
- `development`: All feature branches should branch from here.
- `feature/*`: Feature branches (e.g., `feature/api-auth`, `feature/blazor-ui`).

### GitHub Branch Rules

- `master` is protected: no direct commits, no branching from master.
- All pull requests target `development`.

## DTO Validation

The following DTOs are validated using FluentValidation:

- `CreatePatientDto`
- `UpdatePatientDto`
- `CreateAppointmentDto`

Validation errors return `400 Bad Request` with detailed messages.

## Enums Used

### Gender
| Value | Name     |
|-------|----------|
| 0     | Male     |
| 1     | Female   |
| 2     | Other    |
| 3     | Unknown  |

### AppointmentStatus
| Value | Name      |
|-------|-----------|
| 0     | Scheduled |
| 1     | Completed |
| 2     | Missed    |
| 3     | Cancelled |

### Department
| Value | Name        |
|-------|-------------|
| 0     | General     |
| 1     | Cardiology  |
| 2     | Neurology   |
| 3     | Oncology    |
| 4     | Paediatrics |
| 5     | Dermatology |

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
