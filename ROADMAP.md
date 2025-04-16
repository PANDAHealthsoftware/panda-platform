
# PANDA Roadmap

This roadmap outlines the planned development for the PANDA (Patient Appointment Network Data Application) project. It reflects current capabilities and prioritizes upcoming enhancements in functionality, infrastructure, testing, and deployment.

---

## ✅ Completed

### Core Functionality
- Patient and Appointment CRUD operations
- DTO validation using FluentValidation
- AutoMapper integration for model/DTO mapping
- JWT Authentication and Role-based Authorization
- SQLite with Entity Framework Core
- RESTful API with Swagger UI documentation

### Architecture & Tooling
- Layered solution structure
- Shared DTOs and enums across API and frontend
- Logging via Serilog
- Unit and Integration tests using xUnit, NSubstitute, FluentAssertions
- Frontend built in Blazor WebAssembly
- Frontend unit tests using bUnit

---

## 📌 Short-Term Goals

### API & Business Logic
- Appointment conflict detection (time overlap handling)
- Practitioner availability scheduling
- Role management endpoint (create/update/delete roles)

### Frontend
- UI for scheduling with conflict warnings
- Role/user management interface

### Testing & Quality
- Add end-to-end (E2E) tests (e.g., Playwright, Selenium)
- Test cases for JWT-protected flows

---

## 🚀 Long-Term Goals

### Infrastructure & DevOps
- Docker support with Docker Compose for full-stack dev
- Cloud deployment (Azure App Service / AWS ECS)
- CI/CD pipeline (GitHub Actions)

### Feature Enhancements
- Calendar sync support (iCal or external integration)
- Notification system for reminders (email/SMS)
- Audit logs and patient appointment history

### UX & Internationalization
- Timezone-aware scheduling
- Localization (language support)

---

## 🧪 Maintenance
- Update packages for .NET 8 compatibility
- Regular security audits and dependency checks
