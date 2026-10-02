# Agent Instructions

Repository-specific operating rules for AI coding agents.

## Core Context

Read before non-trivial changes:

- `GOALS.md`
- `SCOPE.md`
- `CONVENTIONS.md`
- `plan/IMPLEMENT_V1_API.md` — API implementation runbook
- `adr/` — accepted decisions

## Language

UK English in documentation, comments, and user-facing text.

## Tech Stack

- .NET 8.0 and .NET 10.0 (primary focus on 10 for samples).
- xUnit v3 with Microsoft.Testing.Platform.
- API client only (no database).

## Architecture

- SDK: `DnsCheckClient`, `Groups` and `DnsRecords` services, typed models, internal HTTP/JSON.
- GET-only v1 monitoring API per [ADR-0001](adr/adr-0001-read-only-monitoring-sdk.md).

## Sample Sync

[`samples/MonitorConsole`](samples/README.md) must reflect **implemented** SDK behaviour only. Update it when adding API operations in the same PR.

## Skills

Project skills live in `.agents/skills/`.

| Skill | Use when |
|---|---|
| `implement-v1-api` | Implementing DNS Check monitoring endpoints end-to-end |
| `create-architectural-decision-record` | Creating or major-updating an ADR |
| `documentation-writer` | Diátaxis-aligned documentation |
| `project-documentation` | Project-aware docs placement |
| `pr-address-review` | Addressing open PR review threads |

## Task Routing

- **SDK work** (`src/`, `tests/`): `CONVENTIONS.md` and `implement-v1-api` skill.
- **Documentation** (`**/*.md` except `adr/`): documentation skills.
- **ADRs** (`adr/*.md`): ADR skill only.

## Not Allowed Without Explicit Instruction

- Add or remove NuGet packages (except central versions in `Directory.Packages.props` when maintaining deps).
- Modify CI/CD behaviour.
- Commit secrets or API keys.

## Pull Request Workflow

Follow [plan/PULL_REQUEST_POLICY.md](plan/PULL_REQUEST_POLICY.md) and [`.github/PULL_REQUEST_TEMPLATE.md`](.github/PULL_REQUEST_TEMPLATE.md).

Before opening a PR (Release):

```bash
dotnet format DnsCheck.slnx --verify-no-changes
dotnet build DnsCheck.slnx -c Release -warnaserror
dotnet test DnsCheck.slnx -c Release --no-build
```

Title shape: `[Type] Imperative summary (#NNN)` with labels from [plan/LABEL_STRATEGY.md](plan/LABEL_STRATEGY.md).

All agent-authored pull requests require human review before merge.
