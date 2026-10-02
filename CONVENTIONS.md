> This file defines coding and design conventions for the DnsCheck.Client SDK.

# Conventions

**Project:** DnsCheck.Client
**Last updated:** 2 October 2026

When in doubt, follow this file. To change a convention, update it and add an ADR for significant architecture changes.

---

## Project Structure

```
src/
└── DnsCheck.Client/
    ├── DnsCheckClient.cs
    ├── DnsCheckException.cs (and related public exceptions)
    ├── DnsCheckGroups.cs / DnsCheckRecords.cs
    ├── DependencyInjection/
    ├── Infrastructure/
    │   ├── Configuration/
    │   ├── Http/
    │   └── Serialization/
    ├── Models/
    │   ├── Groups/
    │   └── DnsRecords/
    └── Services/
        ├── Groups/
        └── DnsRecords/

tests/
└── DnsCheck.Client.Tests/
    └── TestSupport/

samples/
└── MonitorConsole/
```

Place every **public** type in the `DnsCheck.Client` namespace at the project root (one type per file), except `DependencyInjection/` extension types.

---

## Patterns in Use

- **Client + Services** — one `DnsCheckClient` with `Groups` and `DnsRecords`.
- **Strongly typed contracts** — explicit models; `JsonPropertyName` on every serialised property.
- **Exception hierarchy** — SDK-specific types as the public contract.
- **Async-first** — cancellation-aware methods.
- **GET-only v1** — no write helpers until DNS Check documents them.
- **HTTP via `IHttpClientFactory`** — register with `AddDnsCheckClient`; the library does not construct `HttpClient` in public API (see [ADR-0002](adr/adr-0002-typed-http-client-and-di.md)).

---

## C# patterns

- **Dependency injection** — apps call `AddDnsCheckClient` on `IServiceCollection`; tests may use `DnsCheckClient(HttpClient, string?)` with a test `HttpClient`.
- **Async/await** — all I/O is async; do not use `.Result`, `.Wait()`, or sync-over-async.
- **`CancellationToken`** — propagate on public async methods (HTTP and service APIs).
- **Configuration** — `DnsCheckClientOptions` configured in the `AddDnsCheckClient` callback; do not expose `IOptions<T>` on the library’s public surface.
- **Outbound HTTP** — typed client (`DnsCheckClient` + `AddHttpClient<DnsCheckClient>()`); do not `new HttpClient()` in library code or register a long-lived singleton `HttpClient` yourself.
- **Nullable reference types** — enabled for all projects (`Directory.Build.props`).
- **DTOs** — use property-based `record` types for immutable API models and JSON envelope types; use `class` for exceptions and services.
- **Resilience** (retries, timeouts, circuit breaking) — configure on the `IHttpClientBuilder` returned from `AddDnsCheckClient` in hosted apps, not hand-rolled per call in the SDK.

---

## JSON and Types

- `System.Text.Json` with case-insensitive property names for deserialisation.
- `DateTimeOffset` for API timestamps (`created_at`, `updated_at`).
- Enums with explicit wire values for `status` and `record_type` via internal `JsonConverter` types (see `plan/IMPLEMENT_V1_API.md`).

---

## Testing

### Unit testing

- **xUnit v3** with Microsoft.Testing.Platform for all automated tests.
- **NSubstitute** for mocks, stubs, and test doubles when unit isolation requires them.
- **Built-in xUnit `Assert` methods only** — keep test dependencies minimal.

Do not introduce:

- FluentAssertions, AwesomeAssertions, or Shouldly
- Moq, NUnit, or MSTest

### HTTP SDK tests

- Prefer **`QueuedHttpMessageHandler`** and JSON fixtures under `TestSupport/Fixtures/` for service and `RestClient` tests (no live API keys in CI).
- Use NSubstitute when mocking non-HTTP collaborators; do not replace HTTP fakes with mocks unless there is a clear benefit.

### AppHost and end-to-end testing

- **.NET Aspire AppHost** modelling and orchestration are not tested in this repository.
- **Playwright (C#)** — add only if the product gains UI or hosted-app journeys that need end-to-end coverage; do not use Playwright as a substitute for unit tests.

Optional local live smoke: `samples/MonitorConsole` and `DNSCHECK_API_KEY`.

---

## Documentation

- UK English in comments and docs.
- Update `docs/api-coverage.md` when adding SDK operations.
