# Job Application Assistant

An AI-powered job application agent built with **ASP.NET Core**, **Claude API (Anthropic)**, and **PostgreSQL**. Designed as a portfolio project to explore AI agent architecture while solving a real-world problem: tailoring job applications at scale.

---

## What It Does

Given a job description and your resume, the assistant runs a 4-step AI pipeline to produce a fully tailored application package:

| Step | Service | Output |
|------|---------|--------|
| 1 | `SkillExtractionService` | Job title, seniority, required/nice-to-have skills, keywords |
| 2 | `ResumeMatchService` | Match score + matched/missing/partial skills |
| 3 | `ResumeRewriteService` | Resume bullets reworded to mirror JD language |
| 4 | `CoverLetterService` | Tailored cover letter with strategy notes |

All 4 steps are orchestrated by a single `PipelineOrchestrator` — one request in, one complete application package out. Every run is persisted to PostgreSQL with full status tracking and error logging.

---

## Architecture

The project follows a **Clean 3-Layer Architecture**:

```
JobApplicationAssistant/
│
├── JobApplicationAssistant.API/             # Entry point — Controllers, DI wiring, Swagger
│   ├── Controllers/
│   │   └── JobApplicationController.cs
│   └── Program.cs
│
├── JobApplicationAssistant.Core/            # Business logic — Interfaces, Models, Orchestrator
│   ├── Interfaces/
│   │   ├── IClaudeService.cs
│   │   ├── ISkillExtractionService.cs
│   │   ├── IResumeMatchService.cs
│   │   ├── IResumeRewriteService.cs
│   │   ├── ICoverLetterService.cs
│   │   └── IJobApplicationRepository.cs
│   ├── Models/
│   │   ├── Pipeline/                        # PipelineRequest, PipelineResult, step result models
│   │   └── Responses/                       # JobApplicationSummary, JobApplicationDetail
│   └── Pipeline/
│       └── PipelineOrchestrator.cs
│
└── JobApplicationAssistant.Infrastructure/  # External integrations — Claude, DB, Helpers
    ├── Claude/
    │   └── ClaudeService.cs
    ├── Pipeline/
    │   ├── SkillExtractionService.cs
    │   ├── ResumeMatchService.cs
    │   ├── ResumeRewriteService.cs
    │   └── CoverLetterService.cs
    ├── Common/
    │   └── JsonHelper.cs                    # Strips markdown fences from Claude responses
    └── Persistence/
        ├── AppDbContext.cs
        ├── JobApplicationRepository.cs
        ├── Entities/                        # JobApplicationEntity, PipelineResultEntity, PipelineErrorEntity
        └── Configurations/                  # IEntityTypeConfiguration<T> per entity
```

**Why 3 layers?** Each layer has one job:

- **API** — speaks HTTP. Knows nothing about Claude or PostgreSQL.
- **Core** — owns business rules and interfaces. Has zero external dependencies.
- **Infrastructure** — talks to the outside world. Implements Core's interfaces.

---

## Tech Stack

| Concern | Technology |
|---------|------------|
| Backend framework | ASP.NET Core Web API (.NET 9) |
| AI brain | Anthropic Claude API (official .NET SDK) |
| ORM | Entity Framework Core 9.0.4 + Npgsql |
| Database | PostgreSQL 18 |
| Logging | Serilog (file + console) |
| API exploration | Swagger UI (Swashbuckle) |

---

## Database Schema

Three tables persist every pipeline run:

```
job_applications          — one row per run (id, job_description, resume_text, status, created_at)
pipeline_results          — one row per completed run, JSONB columns for each step result
pipeline_errors           — one row per failure (failed_step, error_message, created_at)
```

Pipeline status flow: `processing` → `completed` or `failed`

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL 18](https://www.postgresql.org/download/windows/)
- An [Anthropic API key](https://console.anthropic.com/)

### 1. Clone the repo

```powershell
git clone https://github.com/devmanidhiman/JobApplicationAssistant.git
cd JobApplicationAssistant
```

### 2. Set up PostgreSQL

Create a dedicated database user and database:

```sql
CREATE USER jobapp_user WITH PASSWORD 'yourpassword';
CREATE DATABASE job_application_assistant OWNER jobapp_user;
GRANT ALL PRIVILEGES ON DATABASE job_application_assistant TO jobapp_user;
```

### 3. Configure the connection string

In `src/JobApplicationAssistant.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=job_application_assistant;Username=jobapp_user;Password=yourpassword"
  }
}
```

### 4. Set your Anthropic API key

**Windows (PowerShell):**

```powershell
$env:ANTHROPIC_API_KEY = "sk-ant-your-key-here"
```

> If set in System Environment Variables, fully restart Visual Studio or VS Code for the value to be picked up.

### 5. Run migrations

```powershell
cd src/JobApplicationAssistant.Infrastructure
dotnet ef database update --startup-project ../JobApplicationAssistant.API
```

### 6. Run the API

```powershell
cd src/JobApplicationAssistant.API
dotnet run
```

Navigate to `http://localhost:{port}/swagger` to explore the API.

---

## API Endpoints

### `POST /api/jobapplication/analyze`

Runs the full 4-step pipeline and persists the result.

**Request:**
```json
{
  "jobDescription": "We are looking for a Senior .NET Developer...",
  "resumeText": "Senior Software Developer with 5 years of experience..."
}
```

**Response:**
```json
{
  "skillExtraction": {
    "jobTitle": "Senior .NET Developer",
    "seniorityLevel": "Senior",
    "requiredSkills": ["C#", "ASP.NET Core", "PostgreSQL"],
    "niceToHaveSkills": ["Azure", "Docker"],
    "keywords": [".NET", "backend", "API"]
  },
  "resumeMatch": {
    "matchScore": 82,
    "matchedSkills": ["C#", "ASP.NET Core"],
    "missingSkills": ["CI/CD pipelines"],
    "partialMatches": ["Docker"],
    "summary": "Strong match..."
  },
  "resumeRewrite": {
    "rewrittenBullets": [
      {
        "original": "Built REST APIs for e-commerce platforms.",
        "rewritten": "Designed and maintained production-grade RESTful APIs...",
        "reasoning": "Mirrored JD language..."
      }
    ],
    "summary": "Rewrite strategy..."
  },
  "coverLetter": {
    "coverLetter": "Dear Hiring Manager...",
    "strategy": "Led with strongest matched skills..."
  }
}
```

### `GET /api/jobapplication`

Returns a summary list of all past pipeline runs, ordered by most recent.

### `GET /api/jobapplication/{id}`

Returns the full pipeline result for a specific run, with all four step outputs deserialized.

### `POST /api/jobapplication/ping`

Simple Claude connectivity check.

---

## Roadmap

```
v1 — Automated 4-step pipeline              ✅ Complete
     PostgreSQL persistence                  ✅ Complete
     Retrieval endpoints                     ✅ Complete
     Error persistence & status tracking     ✅ Complete

v2 — Simple frontend UI                     🔲 In progress
     Conversational pre-pipeline clarification  🔲 Planned

v3 — Full chat-based refinement loop        🔲 Planned
     (user can iteratively refine resume & cover letter)
```

---

## Key Design Decisions

**Claude response parsing** — Claude sometimes wraps JSON responses in markdown fences (` ```json `). `JsonHelper.StripMarkdownFences()` in `Infrastructure.Common` handles this transparently before deserialization.

**JSONB for step results** — Pipeline step outputs (`SkillExtractionResult`, `ResumeMatchResult`, etc.) are stored as JSONB in PostgreSQL rather than normalized tables. The data is never queried by internal fields — only stored and retrieved whole — so JSONB gives full fidelity with zero mapping overhead.

**`IEntityTypeConfiguration<T>`** — Each entity's EF Core configuration lives in its own class rather than one large `OnModelCreating` method. Adding new entities doesn't touch existing configuration code.

**Repository interface in Core** — `IJobApplicationRepository` is defined in Core, not Infrastructure. Core never takes a dependency on EF Core — it only depends on the abstraction it defines.

**DI lifetimes** — `AnthropicClient` is `Singleton` (one instance for the app lifetime). All services and the repository are `Scoped` (one instance per request, sharing the same `DbContext`).

**Environment variables on Windows** — `builder.Configuration["ANTHROPIC_API_KEY"]` is used instead of `Environment.GetEnvironmentVariable()` because .NET's `IConfiguration` correctly picks up the process environment across platforms and IDE restarts.

---

## License

MIT — feel free to fork, adapt, and build on this.

---

> Built by a backend developer learning AI agent patterns — one pipeline step at a time.