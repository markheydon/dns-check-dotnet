---
title: "ADR-0002: Typed HTTP client and dependency injection"
status: accepted
date: 2026-10-02
deciders: Mark Heydon
tags: ["architecture", "http", "dependency-injection"]
---

# ADR-0002: Typed HTTP client and dependency injection

## Status

**Accepted**

## Context

CTX-001: Modern .NET libraries that call HTTP APIs should register outbound clients through `IHttpClientFactory` and typed clients (`AddHttpClient<T>()`), not allocate `new HttpClient()` inside public constructors.

CTX-002: The initial alpha SDK exposed `DnsCheckClient()`, `DnsCheckClient(string apiKey)`, and `DnsCheckClient(string apiKey, Uri baseAddress)` constructors that created and owned an internal `HttpClient`, which conflicts with socket handling and DI lifetime guidance.

CTX-003: Consumers include hosted applications (DI), console samples, and unit tests (injected `HttpClient` with a test handler).

## Decision

DEC-001: **`DnsCheckClient` is a typed HTTP client** registered via `AddDnsCheckClient(IServiceCollection, Action<DnsCheckClientOptions>?)`, which calls `AddHttpClient<DnsCheckClient>()` and configures base address from `DnsCheckClientOptions`.

DEC-002: **`DnsCheckClientOptions`** is a plain options type (API key, optional base address) configured in the extension callback — not `IOptions<T>` on the library public surface.

DEC-003: **Public construction** is `DnsCheckClient(HttpClient httpClient, string? apiKey = null)` for tests and advanced scenarios (caller owns `HttpClient` lifetime). Dependency injection activates the same constructor via `AddTypedClient`, supplying `DnsCheckClientOptions.ApiKey` from the registered options instance.

DEC-004: **Remove** parameterless and API-key-only constructors that allocated an internal `HttpClient`. **Remove `IDisposable`** from `DnsCheckClient`; the factory manages `HttpClient` lifetime for DI registrations.

DEC-005: **Resilience** (retries, timeouts, circuit breaking) is configured by the host on the `IHttpClientBuilder` returned from `AddDnsCheckClient`, not inside the SDK.

## Consequences

### Positive

- **POS-001**: Aligns with `IHttpClientFactory` and typed-client patterns for production apps.
- **POS-002**: Clear lifetime semantics — no double-dispose of shared `HttpClient` instances.
- **POS-003**: Extension point for consumers to add Polly / `Microsoft.Extensions.Http.Resilience` handlers.

### Negative

- **NEG-001**: Breaking change for alpha consumers using `new DnsCheckClient(apiKey)` without DI.
- **NEG-002**: Console scripts require a small `ServiceCollection` or manual `HttpClient` construction.
- **NEG-003**: Additional package dependencies (`Microsoft.Extensions.Http`, abstractions).

## Alternatives Considered

### Keep owned-client constructors

- **ALT-001**: **Description**: Retain `new DnsCheckClient(apiKey)` using internal `new HttpClient()`.
- **ALT-002**: **Rejection Reason**: Violates project C# HTTP standards and Microsoft guidance on `HttpClient` lifetime.

### Obsolete owned constructors first

- **ALT-003**: **Description**: Mark owned constructors `[Obsolete]` and remove in a later release.
- **ALT-004**: **Rejection Reason**: Alpha surface area is small; a single clean break is simpler for early adopters.

## References

- [CONVENTIONS.md](../CONVENTIONS.md) — C# patterns and testing standards
- [plan/IMPLEMENT_V1_API.md](../plan/IMPLEMENT_V1_API.md) — HTTP invariants (`RestClient`, no header mutation)
