---
name: code-review
description: >-
  Review DnsCheck.Client changes for correctness, security, API contract,
  architecture, and maintainability. Use for code reviews of a branch, PR, or
  local diff. Read-only unless the user explicitly asks for fixes.
---

# Code review

Review the change set as a pull request. Read [GOALS.md](../../GOALS.md), [SCOPE.md](../../SCOPE.md), [CONVENTIONS.md](../../CONVENTIONS.md), relevant [ADRs](../../adr/), and the [monitoring API reference](../../plan/MONITORING_API_REFERENCE.md) as needed.

Focus on actionable issues that could block approval. Order findings by severity. Do not make changes unless the user explicitly asks.

## Scope

Use the scope requested by the user. Otherwise review the current branch changes against the repository default branch, including staged and unstaged edits. If there are no changes in scope, say so and stop. Do not check out branches or change repository state as part of a review.

## Focus on finding

1. Correctness of HTTP paths, query parameters, JSON envelopes, and error mapping.
2. Safe API-key handling; keys must not be logged or exposed.
3. Public API and architecture consistency with the documented GET-only v1 surface, typed models, and `IHttpClientFactory` registration.
4. Appropriate request validation and cancellation propagation, without inventing undocumented API constraints.
5. Tests for changed behaviour and consistency of `samples/MonitorConsole` with implemented SDK operations.
6. Avoidable complexity or maintainability problems that materially affect the change.

## Do not comment on

- Formatting, personal style preferences, or minor optimisations.
- Planned future work or unrelated pre-existing issues.
- Missing functionality unless the change creates a defect or breaks an explicit requirement.

## How to review

1. Establish the requested diff scope.
2. Read changed files and enough surrounding code and project guidance to verify behaviour.
3. When models or HTTP code change, check JSON property mappings, API-key query handling, path validation, response parsing, and typed exception behaviour.
4. When SDK operations change, compare them with the monitoring API reference and ensure the console sample reflects implemented operations only.
5. Run targeted tests when useful to distinguish a real issue from a suspected one.
6. Report only actionable findings supported by evidence.

For code changes, the Release validation commands are listed in [AGENTS.md](../../AGENTS.md).

## Output

Lead with findings, sorted by severity:

| Severity | Location | Why it matters | Suggested improvement |
| --- | --- | --- | --- |
| … | … | … | … |

Use UK English. Include the relevant file and line for each finding.

## Closing

End with a brief merge-readiness verdict. If there are no significant findings, state that explicitly. Do not fix, commit, or push changes unless the user asks.
