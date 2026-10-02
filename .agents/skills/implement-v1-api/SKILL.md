---
name: implement-v1-api
description: Extend the DNS Check monitoring API in DnsCheck.Client — models, RestClient, services, tests, MonitorConsole sample, and docs. Use when adding new documented monitoring GET operations or changing the v1 HTTP layer.
---

# Extend DNS Check monitoring API

Follow [plan/MONITORING_API_REFERENCE.md](../../plan/MONITORING_API_REFERENCE.md) for operations, JSON envelopes, and HTTP invariants.

## Steps

1. Read official docs pages linked from the reference.
2. Extend `RestClient` only if new cross-cutting HTTP behaviour is required (auth query, errors, parse exceptions).
3. Add or update models, enums, and response wrappers with `JsonPropertyName`.
4. Implement service methods on `GroupService` and/or `DnsRecordService`.
5. Add unit tests per `CONVENTIONS.md` (xUnit v3, built-in asserts; `QueuedHttpMessageHandler` and JSON fixtures for HTTP; NSubstitute only when mocks are needed).
6. Update `samples/MonitorConsole` when the new operation should appear in live smoke.
7. Update `docs/` and `README.md`; update `docs/api-coverage.md`.
8. Run Release build, format, and test gates from [AGENTS.md](../../AGENTS.md).

## Guardrails

- UK English in docs and XML comments.
- Never log or commit API keys.
- No write APIs unless DNS Check documents them.

## Pull request

Follow [plan/PULL_REQUEST_POLICY.md](../../plan/PULL_REQUEST_POLICY.md). Title example: `[Story] Add DNS Check monitoring operation for … (#N)`.
