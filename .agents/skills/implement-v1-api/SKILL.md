---
name: implement-v1-api
description: Implement the DNS Check v1 monitoring API end-to-end in DnsCheck.Client — models, RestClient, services, tests, MonitorConsole sample, and docs. Use when completing plan/IMPLEMENT_V1_API.md or adding monitoring GET operations.
---

# Implement DNS Check v1 API

Implement all four documented GET operations per [plan/IMPLEMENT_V1_API.md](../../plan/IMPLEMENT_V1_API.md).

## Steps

1. Read official docs pages linked from the plan.
2. Implement `RestClient.GetAsync` (auth query, errors, parse exceptions).
3. Add models, enums, and response wrappers with `JsonPropertyName`.
4. Replace `ServiceAvailability.MonitoringNotImplemented` in `GroupService` and `DnsRecordService` with live HTTP calls.
5. Add unit tests per `CONVENTIONS.md` (xUnit v3, built-in asserts; `QueuedHttpMessageHandler` and JSON fixtures for HTTP; NSubstitute only when mocks are needed).
6. Implement `samples/MonitorConsole` live smoke (env: `DNSCHECK_API_KEY`, optional `DNSCHECK_GROUP_UUID`).
7. Update `docs/` and `README.md`; tick `docs/api-coverage.md`.
8. Run Release build, format, and test gates from [AGENTS.md](../../AGENTS.md).

## Guardrails

- UK English in docs and XML comments.
- Never log or commit API keys.
- No write APIs unless DNS Check documents them.

## Pull request

Follow [plan/PULL_REQUEST_POLICY.md](../../plan/PULL_REQUEST_POLICY.md). Title example: `[Story] Implement DNS Check v1 monitoring API (#N)`.
