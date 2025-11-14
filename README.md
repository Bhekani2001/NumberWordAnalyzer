# Number Word Analyzer API

## 1. Project Overview

This API solves an interesting text analysis problem: it finds English number words (like "one", "two", etc.) hidden within scrambled, randomized text. For example, in a string like "eeehffetstriuueuefxxeexeseetoioneight", it can detect that there are occurrences of "one", "three", and "eight" even though the characters are all mixed up.

## 2. Development Approach

### 2.1 Understanding the Problem
When I first read the assessment, I realized the core challenge was detecting words when characters are scrambled. Traditional string searching wouldn't work, so I needed a smarter approach using character frequency analysis.

### 2.2 Solution Architecture
I chose Clean Architecture because it separates concerns and makes the code maintainable:

- Domain Layer: Pure business logic (the number word detection algorithm)
- Application Layer: Services and data transfer objects
- API Layer: Controllers and web-specific code
- Tests Layer: Comprehensive testing

### 2.3 Project Structure
I started by creating a clean solution structure in Visual Studio:

NumberWordAnalyzer/
- NumberWordAnalyzer.API/          # Controllers, Middleware
- NumberWordAnalyzer.Application/   # Services, DTOs, Interfaces
- NumberWordAnalyzer.Domain/        # Core logic, models
- NumberWordAnalyzer.Tests/         # Unit & Integration tests

### 2.4 Project References
I carefully set up the dependency chain:

- API references Application (but not Domain directly)
- Application references Domain (contains business logic)
- Tests references all projects (to test everything)

This ensures clean separation and prevents circular dependencies.

### 2.5 NuGet Packages
I selected these key packages for professional development:

- Swashbuckle.AspNetCore - For interactive Swagger UI documentation
- Moq - For creating mock objects in unit tests
- xUnit & xunit.runner.visualstudio - For robust testing framework
- Microsoft.AspNetCore.Mvc.Testing - For API integration tests

Each package serves a specific purpose in making the API production-ready.

## 3. How to Run This Project

### 3.1 Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code (or any code editor)

### 3.2 Quick Start (3 Easy Steps)

1. Get the Code Ready
   ```bash
   # Clone or download the project
   cd NumberWordAnalyzer

dotnet build
dotnet run --project NumberWordAnalyzer.API

{
  "inputText": "eeehffetstriuueuefxxeexeseetoioneight"
}

# Run all tests
dotnet test

# Run specific test project
dotnet test NumberWordAnalyzer.Tests
