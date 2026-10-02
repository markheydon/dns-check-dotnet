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

Place every **public** type in the `DnsCheck.Client` namespace at the project root (one type per file).

---

## Patterns in Use

- **Client + Services** — one `DnsCheckClient` with `Groups` and `DnsRecords`.
- **Strongly typed contracts** — explicit models; `JsonPropertyName` on every serialised property.
- **Exception hierarchy** — SDK-specific types as the public contract.
- **Async-first** — cancellation-aware methods.
- **GET-only v1** — no write helpers until DNS Check documents them.

---

## JSON and Types

- `System.Text.Json` with case-insensitive property names for deserialisation.
- `DateTimeOffset` for API timestamps (`created_at`, `updated_at`).
- Enums with explicit wire values for `status` and `record_type` via internal `JsonConverter` types (see `plan/IMPLEMENT_V1_API.md`).

---

## Testing

- Unit tests use `QueuedHttpMessageHandler`; no live API keys in CI.
- Optional local live smoke via `samples/MonitorConsole` and `DNSCHECK_API_KEY`.

---

## Documentation

- UK English in comments and docs.
- Update `docs/api-coverage.md` when adding SDK operations.
