
# PANDA - Patient Appointment Network Data Application (Tech Test)

## Overview
PANDA is a production-quality API designed for managing patient demographics and appointments. This application implements a CRUD backend with the ability to store patient data and appointments, while ensuring high-quality code, effective validation, and integration with external systems.

This solution is developed as part of the Aire Logic tech test, showcasing technical knowledge, best practices, and design decisions.

## Features
- **Patient CRUD Operations**: Create, read, update, and delete patient records.
- **Appointment CRUD Operations**: Schedule, update, view, and cancel patient appointments.
- **Validation**: FluentValidation for both `PatientDto` and `CreateAppointmentDto`.
- **Automated Testing**: Unit tests for all CRUD operations, including validation and edge case scenarios.
- **AutoMapper Integration**: Simplified data mapping between entities and DTOs.

## Tech Stack
- **.NET 8**: The application is built using .NET 8 for the backend API.
- **Dapper**: A lightweight ORM for SQL data access.
- **SQLite**: Persistent SQLite database for data storage.
- **FluentValidation**: Ensures data validity for DTOs.
- **NSubstitute**: Used for mocking dependencies in unit tests.
- **EF Core InMemory**: For in-memory testing of CRUD operations.
- **AutoMapper**: For efficient mapping between entities and DTOs.

## Getting Started

### Prerequisites
To run the project locally, ensure you have the following installed:
- .NET 8 SDK
- SQLite (if not using the default SQLite file)
- Visual Studio or JetBrains Rider for development

### Clone the Repository
```bash
git clone https://github.com/your-repository/panda.git
cd panda
```

### Set up the Database
The SQLite database is automatically seeded with sample data for patients and appointments. No additional setup is needed.

### Running the Application
You can run the application using the following command:
```bash
dotnet run
```

The API will start on `http://localhost:5000`. You can use any API client (e.g., Postman or cURL) to test the endpoints.

### Available Endpoints
- **POST /patients**: Create a new patient.
- **GET /patients/{id}**: Get details of a specific patient.
- **PUT /patients/{id}**: Update an existing patient.
- **DELETE /patients/{id}**: Delete a patient record.
- **POST /appointments**: Create a new appointment.
- **GET /appointments/{id}**: View an appointment.
- **PUT /appointments/{id}**: Update an appointment.
- **DELETE /appointments/{id}**: Cancel an appointment.

### Testing with API Client (Postman, cURL, etc.)
To interact with the API using your preferred client (Postman, cURL, or similar), follow these steps:

1. **Postman Setup**:
    - Open Postman and create a new **GET** request for `http://localhost:5000/api/patients` to retrieve all patients.
    - For creating a new patient, use **POST** and set the body type to `application/json`, then send a JSON payload like this:
      ```json
      {
        "firstName": "John",
        "lastName": "Doe",
        "dateOfBirth": "1985-10-10",
        "nhsNumber": "1234567890",
        "postcode": "AB1 2CD",
        "gender": "Male"
      }
      ```
    - For updating a patient, send a **PUT** request to `http://localhost:5000/api/patients/{id}` with the patient's updated details in the body.

2. **cURL Setup**:
    - Use cURL to send requests from the command line. For example, to retrieve all patients:
      ```bash
      curl -X GET http://localhost:5000/api/patients
      ```
    - To create a new patient, use:
      ```bash
      curl -X POST http://localhost:5000/api/patients         -H "Content-Type: application/json"         -d '{"firstName": "John", "lastName": "Doe", "dateOfBirth": "1985-10-10", "nhsNumber": "1234567890", "postcode": "AB1 2CD", "gender": "Male"}'
      ```

3. **Expected Responses**:
    - For **GET /patients**: Returns a list of all patients, with the `Gender` property showing the enum value (e.g., "Male", "Female").
    - For **GET /patients/{id}**: Returns a single patient with the `Gender` as an enum value.
    - For **POST /patients**: Returns a success message and the ID of the newly created patient.
    - For **PUT /patients/{id}**: Updates the patient data and returns a `204 No Content` status upon success.
    - For **DELETE /patients/{id}**: Deletes the patient and returns a `204 No Content` status upon success.

### Validation and Error Handling
The application uses FluentValidation for all DTOs, ensuring the input data is valid. API responses provide clear messages for validation errors.

### Seeding Data
The SQLite database is seeded with realistic patient and appointment data on startup. You can review or modify this data in the `seed.sql` file.

## Running Tests
Navigate to the tests folder: Use the terminal to go to the PANDA.Api.Tests folder where the test project is located.:
```bash
cd PANDA.Api.Tests
```
Run the tests: Once you're in the PANDA.Api.Tests directory, run the tests using the following command:
The tests cover the CRUD operations, validation logic, and edge case scenarios.
```bash
dotnet test
```
Ensure Dependencies: Make sure you have all the necessary dependencies installed. If you're missing any, you can restore them with:
```bash
dotnet restore
```
## Contributing
This project was created for the Aire Logic tech test. However, contributions to improve the application are welcome. Please fork the repository, make changes, and submit a pull request.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
