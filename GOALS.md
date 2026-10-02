# Goals

**Project:** DnsCheck.Client
**Owner:** Mark Heydon
**Last updated:** 2 October 2026

---

## Why This Exists

[DNS Check](https://www.dnscheck.co/) exposes a small, read-only monitoring API for DNS record groups and individual records. .NET developers integrating with Nagios-style checks or custom automation should not hand-build URLs and parse JSON envelopes by hand.

This project is a strongly typed, unofficial client in the same spirit as [freeagent-dotnet](https://github.com/markheydon/freeagent-dotnet) and [octopus-energy-dotnet](https://github.com/markheydon/octopus-energy-dotnet).

---

## Goals for v1.0

- **G1:** Provide a clean .NET SDK for the documented DNS Check **v1 monitoring** API (groups and records).
- **G2:** Callers discover operations from IntelliSense (`Groups`, `DnsRecords`) without reconstructing paths or query strings.
- **G3:** Typed models, status enums, and a small exception hierarchy for HTTP and parse failures.
- **G4:** API keys stay with the caller; the SDK never logs secrets.
- **G5:** Enable contributions without destabilising the public API.

---

## Success Looks Like

- Used in at least one automation or monitoring workflow owned by the author.
- Package published to NuGet with concise consumer documentation.
- CI green on `main`; release workflow publishes on version tags.

---

## Kill Criteria

- DNS Check ships an official .NET SDK that supersedes this work.
- Maintenance cost outweighs personal value.
- The documented API is withdrawn or materially changed without a version bump.

---

## What This Is NOT For

(See also: SCOPE.md)

- Creating or updating monitors (not in the public API).
- Enterprise webhooks.
- UI, CLI products, or site-sync automation (consumers may live elsewhere).

---

## Revision History

| Date | Change | Reason |
|---|---|---|
| 2 October 2026 | Stable 1.0.0 shipped | Monitoring GET API complete |
| 2 October 2026 | Initial draft | Repository scaffold |
