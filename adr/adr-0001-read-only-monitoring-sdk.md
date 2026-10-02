---
title: Read-only DNS Check monitoring SDK
status: accepted
date: 2026-10-02
deciders: Mark Heydon
---

# ADR-0001: Read-only DNS Check monitoring SDK

## Context

DNS Check documents a versioned JSON API (`/api/v1/`) accessed via GET requests and an `api_key` query parameter. It supports monitoring DNS record groups and individual records. There is no documented public API for creating or updating monitors.

## Decision

DEC-001: The public package covers **monitoring GET operations** only, as documented on [dnscheck.co/api](https://www.dnscheck.co/api).

DEC-002: Authentication is the caller-supplied API key appended as `api_key` on each request. The SDK does not persist or log keys.

DEC-003: The package is **unofficial** and not affiliated with DNS Check or Wind Serve, LLC.

## Consequences

Positive:

- Small, honest surface aligned with upstream docs.
- Suitable for Nagios-style polling and custom dashboards.

Negative:

- Site-sync automation that creates monitors must use other mechanisms until DNS Check documents write APIs.
