# Implement DNS Check API v1

Runbook for completing the monitoring API in `DnsCheck.Client`. Use with `.agents/skills/implement-v1-api/SKILL.md`.

Official docs:

- [DNS Check API overview](https://www.dnscheck.co/api)
- [DNS record group monitoring](https://www.dnscheck.co/api/dns-record-group-monitoring)
- [DNS record monitoring](https://www.dnscheck.co/api/dns-record-monitoring)

---

## Operations

| Docs operation | HTTP | SDK method |
|----------------|------|------------|
| Get one group | `GET groups/{uuid}?api_key=` | `Groups.GetAsync(string groupUuid, CancellationToken)` |
| List all groups | `GET groups/all?api_key=` | `Groups.ListAllAsync(CancellationToken)` |
| Get one record | `GET groups/{uuid}/{id}?api_key=` | `DnsRecords.GetAsync(string groupUuid, int recordId, CancellationToken)` |
| List records in group | `GET groups/{uuid}/all?api_key=` | `DnsRecords.ListInGroupAsync(string groupUuid, CancellationToken)` |
| List all records (account) | `GET groups/all` + `GET groups/{uuid}/all` per group | `DnsRecords.ListAllAsync(CancellationToken)` — `GET groups/all/all` returns 401 on typical keys |

Path constants: `DnsCheckGroups.All`, `DnsCheckRecords.All` (list paths; single record get uses `int` record id).

Example public group (no valid key required): `ea883d67-d9f6-45e3-b3a1-844dd1857824`.

---

## JSON envelopes

| Response | Root property | Notes |
|----------|---------------|--------|
| Single group | `group` | Per docs |
| List groups | `groups` | Fixture in `groups-all.json` |
| Single record | `dns_record` | Per docs |
| List records | `dns_records` | Confirmed on example group `/all` |

Missing required branch after deserialise → `DnsCheckApiException`.

Errors: non-2xx body is often a JSON **string** (`"Unauthorized"`, `"Not found"`).

---

## Models

Populate `DnsRecordGroup` and `DnsRecord` with documented fields. Use `[JsonPropertyName]` on every property.

- Timestamps: `DateTimeOffset` for `created_at`, `updated_at`
- `DnsCheckStatus`: `pass`, `fail`, `unknown` (`JsonStringEnumMemberName`)
- `DnsRecordType`: `A`, `AAAA`, `ALIAS`, `CAA`, `CNAME`, `HTTPS`, `MX`, `NS`, `PTR`, `SOA`, `SPF`, `SRV`, `SVCB`, `TXT`

Wrapper types: `GroupResponse`, `GroupsListResponse`, `DnsRecordResponse`, `DnsRecordsListResponse`.

---

## HTTP (`RestClient`)

- Base: `https://www.dnscheck.co/api/v1/`
- Build absolute request URIs with `RestClient.BuildRequestUri` (uses resolved base, not `HttpClient.BaseAddress`)
- Append `api_key` via `RestQuery` when key is non-null
- Validate `groupUuid` (and similar path segments) before building paths: GUID format, no `?`, `#`, or `/` — use `ApiPathValidation.ValidateGroupUuid` (throws `DnsCheckRequestException`)
- Do not pass `api_key` in caller-supplied query parameters; `RestClient` rejects duplicates
- Fail-fast positive integer `recordId` on `DnsRecords.GetAsync` (see `DnsRecordService`)
- Relative paths passed to `RestClient.BuildRequestUri` must pass `ApiPathValidation.ValidateRelativePath` (no `..`, `%`, `?`, `#`, leading `/`, or absolute URI schemes)
- `GetAsync<T>(relativePath, cancellationToken)` — deserialize with `DnsCheckJsonSerializerOptions`
- Map 401/404 (and other errors) to `DnsCheckApiException` with body text
- Do not mutate injected `HttpClient.DefaultRequestHeaders`
- No pagination or retry in v1

Relative paths: `groups/{uuid}`, `groups/{uuid}/{recordIdOrAll}`.

---

## Tests

- Fixtures under `tests/DnsCheck.Client.Tests/TestSupport/Fixtures/` from public example responses
- `QueuedHttpMessageHandler`: assert path and `api_key` query parameter
- No live API key in CI

---

## Sample (`samples/MonitorConsole`)

- Interactive menu by default; `--run-all` for CI-style checks and dotnet test-style summary
- Without key: example group get + list records (account-wide checks skipped)
- With `DNSCHECK_API_KEY`: `Groups.ListAllAsync` and composite `DnsRecords.ListAllAsync` (`groups/all`, then `groups/{uuid}/all` per group)
- Optional `DNSCHECK_GROUP_UUID`
- Non-zero exit on failure (`--run-all` when any check fails or none pass)

---

## Docs

Update:

- `README.md` quick start
- `docs/getting-started.md`, `docs/authentication.md`, `docs/api-coverage.md` (check all five operations)

---

## PR2 checklist

- [ ] `dotnet format DnsCheck.slnx --verify-no-changes`
- [ ] `dotnet build DnsCheck.slnx -c Release -warnaserror`
- [ ] `dotnet test DnsCheck.slnx -c Release --no-build`
- [ ] Sample runs against example group
- [ ] Bump package version per `VERSIONING.md`
- [ ] No secrets in repo
