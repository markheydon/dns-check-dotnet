# Versioning Policy

**Project:** DnsCheck.Client
**Last updated:** 2 October 2026

---

## Overview

This project uses [Semantic Versioning 2.0.0](https://semver.org/). Public API changes follow SemVer.

---

## Current Stage: Alpha

Tag format: `0.1.0-alpha.n` until the v1 monitoring API is feature-complete and tested.

- Suitable for early adopters who pin exact versions.
- Breaking changes may occur between alpha releases.

---

## Criteria for First Stable 1.0.0

1. [In Scope — v1.0](SCOPE.md) implemented with unit test coverage (recorded fixtures).
2. Goals G1–G5 in [GOALS.md](GOALS.md) met for monitoring GET operations.
3. SDK used in at least one real workflow owned by the author.
4. No known breaking changes planned immediately.

---

## Publishing

See [plan/RELEASE.md](plan/RELEASE.md). Tag `vX.Y.Z` must match `<Version>` in `src/DnsCheck.Client/DnsCheck.Client.csproj`.

---

## Revision History

| Date | Change | Reason |
|---|---|---|
| 2 October 2026 | Initial draft | Repository scaffold |
