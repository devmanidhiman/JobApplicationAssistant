# Job Application Assistant

An AI-powered job application agent built with **ASP.NET Core**, **Claude API (Anthropic)**, and **PostgreSQL**. Designed as a portfolio project to explore AI agent architecture while solving a real-world problem: tailoring job applications at scale.

---

## 🎯 What It Does

Given a job description and your resume, the assistant runs a 4-step AI pipeline to produce a fully tailored application package:

| Step | Service | Output |
|------|---------|--------|
| 1 | `SkillExtractionService` | Skills & keywords extracted from the JD |
| 2 | `ResumeMatchService` | Match score + skill gap analysis |
| 3 | `ResumeRewriteService` | Resume bullets reworded to mirror JD language |
| 4 | `CoverLetterService` | Tailored cover letter |

All 4 steps are orchestrated by a single `PipelineOrchestrator` — one request in, one complete application package out.

---

## Architecture

The project follows a **Clean 3-Layer Architecture**:

```
JobApplicationAssistant/
│
├── JobApplicationAssistant.API/          # Entry point — Controllers, DI wiring, Swagger
│   ├── Controllers/
│   │   └── JobApplicationController.cs
│   └── Program.cs
│
├── JobApplicationAssistant.Core/         # Business logic — Interfaces, Models, Orchestrator
│   ├── Interfaces/
│   │   ├── IClaudeService.cs
│   │   ├── ISkillExtractionService.cs
│   │   ├── IResumeMatchService.cs
│   │   ├── IResumeRewriteService.cs
│   │   └── ICoverLetterService.cs
│   ├── Models/
│   │   ├── PipelineRequest.cs
│   │   └── PipelineResult.cs
│   └── Orchestration/
│       └── PipelineOrchestrator.cs
│
└── JobApplicationAssistant.Infrastructure/  # External integrations — Claude, DB, Helpers
    ├── AI/
    │   ├── ClaudeService.cs
    │   ├── SkillExtractionService.cs
    │   ├── ResumeMatchService.cs
    │   ├── ResumeRewriteService.cs
    │   └── CoverLetterService.cs
    ├── Common/
    │   └── JsonHelper.cs                 # Strips markdown fences from Claude responses
    └── Persistence/                      # PostgreSQL (Phase 3)
```

**Why 3 layers?** Each layer has one job:
- **API** — speaks HTTP. Knows nothing about Claude or PostgreSQL.
- **Core** — owns business rules and interfaces. Has zero external dependencies.
- **Infrastructure** — talks to the outside world. Implements Core's interfaces.

---

## Tech Stack

| Concern | Technology |
|---------|-----------|
| Backend framework | ASP.NET Core Web API (.NET 8) |
| AI brain | Anthropic Claude API (official .NET SDK) |
| Logging | Serilog (file + console) |
| API exploration | Swagger UI (Swashbuckle) |
| Database | PostgreSQL *(Phase 3)* |
| Frontend | Simple UI *(Phase 3)* |

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- An [Anthropic API key](https://console.anthropic.com/)

### 1. Clone the repo

```bash
git clone https://github.com/your-username/JobApplicationAssistant.git
cd JobApplicationAssistant
```

### 2. Set your API key

Set the environment variable before running. The application reads it via `IConfiguration`.

**Windows (PowerShell):**
```powershell
$env:ANTHROPIC_API_KEY = "sk-ant-your-key-here"
```

> If you set this in System Environment Variables, you must **fully restart Visual Studio or VS Code** for the new value to be picked up by the running process.

### 3. Run the API

```bash
cd JobApplicationAssistant.API
dotnet run
```

Navigate to `https://localhost:{port}/swagger` to explore the API via Swagger UI.

---

## API Usage

### `POST /api/jobapplication/process`

Runs the full 4-step pipeline.

**Request body:**
```json
{
  "jobDescription": "We are looking for a Senior .NET Developer with experience in microservices, Azure, and CI/CD pipelines...",
  "resumeText": "Software Engineer with 5 years of experience in C#, ASP.NET Core, SQL Server..."
}
```

**Response:**
```json
{
  "extractedSkills": ["C#", ".NET", "microservices", "Azure", "CI/CD"],
  "matchScore": 78,
  "skillGaps": ["Kubernetes", "Terraform"],
  "rewrittenBullets": [
    "Architected microservices-based backend using ASP.NET Core, reducing deployment time by 40%",
    "..."
  ],
  "coverLetter": "Dear Hiring Manager, I am excited to apply for the Senior .NET Developer role..."
}
```

---

## Roadmap

```
v1 — Automated 4-step pipeline          ✅ Complete
v2 — Conversational pre-pipeline        🔲 Planned
       (clarify role, tone, priorities before running)
v3 — Full chat-based refinement loop    🔲 Planned
       (user can iteratively refine resume & cover letter)
```

**Near-term (Phase 3):**
- [ ] PostgreSQL persistence — store pipeline runs per user
- [ ] Token cost tracking per request
- [ ] Simple frontend UI

---

## Key Design Decisions & Lessons

**Claude response parsing** — Claude sometimes wraps JSON in markdown fences (` ```json `). `JsonHelper.StripMarkdownFences()` in `Infrastructure.Common` handles this before deserialization.

**DI lifetime** — `ClaudeService` is registered as `Singleton` (one HTTP client for the app lifetime). Pipeline step services are `Scoped` (one instance per request).

**Environment variables & Windows** — `builder.Configuration["ANTHROPIC_API_KEY"]` is used instead of `Environment.GetEnvironmentVariable()` because .NET's `IConfiguration` respects the process environment correctly across platforms and IDE restarts.

**Swashbuckle version** — Pinned to `7.3.1` to avoid OpenAPI version conflicts with .NET 8.

---

## 📄 License

MIT — feel free to fork, adapt, and build on this.

---

> Built by a backend developer learning AI agent patterns — one pipeline step at a time.