# Versioning Policy

**Project:** DnsCheck.Client
**Last updated:** 2 October 2026

---

## Overview

This project uses [Semantic Versioning 2.0.0](https://semver.org/). Public API changes follow SemVer.

---

## Current stage: stable 1.x

**1.0.0** is the first stable release for the documented DNS Check v1 **monitoring** GET API (groups and records).

- Patch releases (`1.0.x`): bug fixes, documentation, non-breaking dependency updates.
- Minor releases (`1.x.0`): backward-compatible SDK additions (for example new upstream read operations if DNS Check documents them).
- Major releases (`2.0.0`): breaking public API changes.

Pre-1.0 alpha packages used the `0.1.0-alpha.n` tag format. Do not reuse those versions after 1.0.0.

---

## 1.0.0 criteria (met)

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
| 2 October 2026 | Stable 1.0.0 | First stable monitoring SDK release |
| 2 October 2026 | Initial draft | Repository scaffold |
