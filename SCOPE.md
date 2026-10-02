# Scope

**Project:** DnsCheck.Client
**Last updated:** 2 October 2026

---

## In Scope — v1.0

- Authenticated HTTP client using a DNS Check **API key** (`api_key` query parameter) against `https://www.dnscheck.co/api/v1/`.
- Configurable base URL (default production host).
- Strongly typed models and resource services for:
  - DNS record group monitoring (`GET /groups/{group_uuid}`)
  - DNS record monitoring (`GET /groups/{group_uuid}/{record_id}`)
  - Special path tokens `group_uuid=all` and `record_id=all`
- Typed exception hierarchy for HTTP and API errors.
- NuGet packaging and release workflow.
- Consumer docs: getting started, authentication, API coverage.
- Console sample for opt-in live smoke tests (not run in CI).

---

## Out of Scope

Do not implement without an explicit scope change.

- Write APIs (create/update/delete monitors) — not documented on [dnscheck.co/api](https://www.dnscheck.co/api).
- Enterprise webhooks.
- Blazor or other UI samples.
- Pagination (API returns full collections per call).
- Official affiliation or endorsement claims.

---

## Revision History

| Date | Change | Reason |
|---|---|---|
| 2 October 2026 | Initial draft | Repository scaffold |
