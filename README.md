# PANDA - Patient Appointment Network Data Application

PANDA is a lightweight .NET API built to manage patients and their appointments with high reliability and test coverage. Designed for rapid deployment and extensibility.

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- SQLite (optional: used by default for persistence)

### Run the API

```bash
cd PANDA.Api

dotnet run
```

The API will be available at:

```
https://localhost:5001
http://localhost:5000
```

---

## 🔮 Run the Tests

```bash
cd PANDA.Api.Tests

dotnet test
```

---

## 📖 API Overview

### Patient Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET    | `/api/patient/{id}` | Get a patient by ID |
| POST   | `/api/patient` | Create a new patient |
| PUT    | `/api/patient/{id}` | Update patient info |
| DELETE | `/api/patient/{id}` | Delete a patient |

### Appointment Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET    | `/api/appointment/{id}` | Get an appointment |
| POST   | `/api/appointment` | Create an appointment |
| PUT    | `/api/appointment/{id}` | Update an appointment |
| DELETE | `/api/appointment/{id}` | Cancel an appointment |
| POST   | `/api/appointment/track-missed-appointment?appointmentId=123` | Mark missed appointments |

---

## ✅ Features

- Full CRUD support for Patients and Appointments
- Entity validation using FluentValidation
- NHS number checksum validation
- Postcode format validation
- Timezone-aware timestamps (`DateTimeOffset`)
- Appointments cannot be reinstated once cancelled
- Missed appointment tracking logic
- Detailed logging using Serilog
- Fully tested with EF Core InMemory + FluentValidation TestHelper

---

## 🧩 Tech Stack

- ASP.NET Core Web API
- Entity Framework Core (SQLite/InMemory)
- FluentValidation
- Serilog
- xUnit + FluentAssertions + NSubstitute

---

## 🔒 Assumptions

- The API is used in a trusted internal environment (no authentication).
- Frontend developers rely on server-side validation and JSON error responses.
- Future localisation and clinician tracking are planned but not implemented in MVP.

---

## 📫 Contact

For questions or feedback, feel free to open an issue or contact the maintainer.
