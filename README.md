# HR System

A web application for time tracking, absence management and selected HR processes in small and
medium-sized service companies and software houses. It was built as a university project
(_Projektowanie i programowanie aplikacji i systemów biznesowych_, Uniwersytet Dolnośląski DSW).

| Area     | Technology                                                       |
|----------|------------------------------------------------------------------|
| Backend  | C# / ASP.NET Core 10 (REST API), ASP.NET Core Identity           |
| Data     | PostgreSQL 17, Entity Framework Core 10 (code-first migrations)  |
| Frontend | TypeScript, React 19, Vite, React Router, TanStack Query, Mantine |
| Runtime  | Docker / Docker Compose (PostgreSQL + API + nginx serving the SPA) |

See [docs/architecture.md](docs/architecture.md) for a description of the architecture.

## Repository layout

```
backend/                     ASP.NET Core solution (HrSystem.slnx)
  src/HrSystem.Domain/         entities and business rules (no dependencies)
  src/HrSystem.Application/    use cases (application services), DTOs, abstractions
  src/HrSystem.Infrastructure/ EF Core + PostgreSQL, ASP.NET Core Identity, migrations, seeding
  src/HrSystem.Api/            REST controllers, authentication/authorization, error handling
  tests/HrSystem.Api.Tests/    integration tests of the HTTP API
frontend/                    React single-page application
docs/                        technical documentation
docker-compose.yml           full local environment
```

## Running the application

### Option A – everything in Docker

```bash
cp .env.example .env        # optional: change passwords, disable demo accounts
docker compose up --build
```

The application is then available at <http://localhost:8080>. PostgreSQL is also exposed on
`localhost:5432`, so it can be inspected with any database client.

### Option B – local development

Requirements: [.NET SDK 10](https://dotnet.microsoft.com/download), Node.js 24 (≥ 22.22) and Docker
(for PostgreSQL only).

```bash
# 1. Database
docker compose up -d db

# 2. Backend – http://localhost:5000, API reference: http://localhost:5000/scalar
cd backend
dotnet run --project src/HrSystem.Api

# 3. Frontend – http://localhost:5173 (proxies /api to the backend)
cd frontend
npm install
npm run dev
```

On startup the backend applies pending migrations and seeds roles and user accounts.

### Demo accounts

Created when `Seed:DemoUsers` is enabled (Development and the default Docker setup):

| E-mail              | Password    | Roles                   |
|---------------------|-------------|-------------------------|
| `admin@hr.local`    | `Admin123!` | Administrator, Employee |
| `hr@hr.local`       | `Demo123!`  | HR, Employee            |
| `manager@hr.local`  | `Demo123!`  | Manager, Employee       |
| `employee@hr.local` | `Demo123!`  | Employee                |

Change these passwords (`.env`) and set `SEED_DEMO_USERS=false` for any non-local deployment.

## Development

### Database migrations

```bash
cd backend
dotnet tool restore
dotnet ef migrations add <MigrationName> -p src/HrSystem.Infrastructure -s src/HrSystem.Api -o Persistence/Migrations
```

Migrations are applied automatically when the API starts. To get the SQL script instead:
`dotnet ef migrations script -p src/HrSystem.Infrastructure -s src/HrSystem.Api`.

### Database backup and restore

```bash
docker compose exec -T db pg_dump -U hr -Fc hr_system > backup.dump
docker compose exec -T db pg_restore -U hr -d hr_system --clean --if-exists < backup.dump
```

### Tests and checks

```bash
cd backend && dotnet test                         # API integration tests
cd frontend && npm test && npm run lint && npm run typecheck
```

## Schedule

- [x] **21.11.2026** – application skeleton, architecture
- [ ] **19.12.2026** – projects, time tracking / timesheets, absences, organizational structure
- [ ] **16.01.2027** – remaining features: employment history, performance reviews, reports
