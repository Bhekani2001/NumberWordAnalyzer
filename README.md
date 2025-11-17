Number Word Analyzer API

Project Overview

This API solves an interesting text analysis problem: it finds English number words (like "one", "two", etc.) hidden within scrambled, randomized text. For example, in a string like `"eeehffetstriuueuefxxeexeseetoioneight"`, it can detect occurrences of "one", "three", and "eight" even though the characters are all mixed up.

Live Demo

The API is deployed and available at:  
**https://numberwordanalyzer.onrender.com**


Development Approach
Understanding the Problem
When I first read the assessment, I realized the core challenge was detecting words when characters are scrambled. Traditional string searching wouldn't work, so I needed a smarter approach using character frequency analysis.

Solution Architecture
I chose Clean Architecture because it separates concerns and makes the code maintainable:

Domain Layer: Pure business logic (the number word detection algorithm)

Application Layer: Services and data transfer objects

API Layer: Controllers and web-specific code

Tests Layer: Comprehensive testing

Project Structure
NumberWordAnalyzer/
├── NumberWordAnalyzer.API/          # Controllers, Middleware
├── NumberWordAnalyzer.Application/   # Services, DTOs, Interfaces
├── NumberWordAnalyzer.Domain/        # Core logic, models
└── NumberWordAnalyzer.Tests/         # Unit & Integration tests

Project References
I carefully set up the dependency chain:

API references Application (but not Domain directly)

Application references Domain (contains business logic)

Tests references all projects (to test everything)

This ensures clean separation and prevents circular dependencies.


NuGet Packages
I selected these key packages for professional development:

Swashbuckle.AspNetCore - For interactive Swagger UI documentation

Moq - For creating mock objects in unit tests

xUnit & xunit.runner.visualstudio - For robust testing framework

Microsoft.AspNetCore.Mvc.Testing - For API integration tests

Each package serves a specific purpose in making the API production-ready.


How to Run This Project
Prerequisites
.NET 8.0 SDK

Visual Studio 2022 or VS Code (or any code editor)


Test the API

Open your browser to: https://localhost:7274/swagger

Try the POST /api/NumberWordAnalyzer endpoint

Use this test input: eeehffetstriuueuefxxeexeseetoioneight


API Endpoints
Live Endpoints (Deployed):
Base URL: https://numberwordanalyzer.onrender.com

Swagger UI: https://numberwordanalyzer.onrender.com/swagger

Health Check: https://numberwordanalyzer.onrender.com/api/NumberWordAnalyzer/health


Available Endpoints:
GET / - API information and endpoints

POST /api/NumberWordAnalyzer - Main text analysis endpoint

GET /api/NumberWordAnalyzer/health - Health check

GET /api/NumberWordAnalyzer/words - Supported number words

GET /api/NumberWordAnalyzer/algorithm - Algorithm explanation

POST /api/NumberWordAnalyzer/validate - Input validation


Algorithm
Character Frequency Counting
The solution uses a character frequency counting algorithm:

Count Characters: Scan the input and count how many times each letter appears

Analyze Each Number Word: For each number word ("one", "two", etc.), check required characters

Calculate Maximum Occurrences: Using the limiting character, determine how many times the word can be formed

Return Results: Provide counts for all number words

Example:
Input: "ooonnneee" (3 o's, 3 n's, 3 e's)
"one" requires: 1 o, 1 n, 1 e
Maximum "one" occurrences = min(3 o's, 3 n's, 3 e's) = 3

Complexity: O(n) - Linear time efficiency


Testing
The project includes comprehensive testing:

Unit Tests: Core algorithm, services, validation, controllers

Integration Tests: End-to-end API functionality

Edge Cases: Empty inputs, null values, special characters

Run tests with: dotnet test


Deployment
The application is deployed using Docker to Render.com:

Dockerfile: Multi-stage build for optimal image size

CI/CD: Automatic deployments on Git push

Environment: Production-ready with proper configuration
