# EduPulse – Academic Course & Laboratory Management Portal

A full-stack web application developed for **Project III – Tools for Program Development** to centralize university coursework, laboratory assignments, student submissions, and professor grading.

## Features (Milestone 2 Prototype)
- **Role-Based Access Control:** Student, Professor, and Administrator roles with JWT authentication and BCrypt password hashing.
- **Course & Lab Management:** Professors publish course modules, lab guides, and deadlines; students view enrolled courses and download materials.
- **Assignment Submissions:** Students upload `.zip` / `.rar` solution archives with automatic timestamping and late-submission detection.
- **Grading & Feedback:** Professors filter submissions by study group (e.g., `CEN 4.S2 B`), download archives, assign grades (`1.0 – 10.0`), and write feedback.

## Tech Stack
- **Frontend:** React 18, TypeScript, Vite, Tailwind CSS
- **Backend:** ASP.NET Core 8 Web API (C#), Entity Framework Core 8
- **Database:** PostgreSQL 16
- **DevOps & Tools:** Docker, Docker Compose, GitHub Actions CI, Swagger/OpenAPI

## Project Structure
```text
EduPulse-AcademicPortal/
├── .github/workflows/ci.yml         # Automated build, lint & test CI pipeline
├── docker-compose.yml               # Multi-container stack (PostgreSQL, API, React)
├── docs/                            # Report 1, Report 2 & architecture/ERD/UI diagrams
├── backend/
│   ├── AcademicPortal.sln
│   ├── Dockerfile
│   ├── src/AcademicPortal.API/      # Controllers, Models, Data (EF Core), Program.cs
│   └── tests/AcademicPortal.Tests/  # xUnit unit tests
└── frontend/
    ├── Dockerfile
    ├── package.json
    └── src/                         # React TypeScript components & dashboards
```

## Quick Start (Docker Compose)
```bash
docker-compose up --build
```
- **Frontend:** `http://localhost:3000`
- **Backend API & Swagger:** `http://localhost:5000/swagger`
- **PostgreSQL:** `localhost:5432`
