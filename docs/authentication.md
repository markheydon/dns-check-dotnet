# Authentication

DNS Check uses a 32-character API key passed as the `api_key` query parameter on each GET request.

Generate a key: [DNS Check — Generating an API Key](https://www.dnscheck.co/api/generate-key).

## SDK usage

```csharp
using DnsCheck.Client.DependencyInjection;

builder.Services.AddDnsCheckClient(options =>
{
    options.ApiKey = apiKey;
});
```

When registering via `AddDnsCheckClient`, `DnsCheckClientOptions.ApiKey` must be `null` or a non-whitespace value. For manual construction, pass `null` or a non-whitespace key to `DnsCheckClient(HttpClient, string?)`.

Treat the key as a secret. Do not commit it to source control.

## Environment variables (samples)

| Variable | Purpose |
|----------|---------|
| `DNSCHECK_API_KEY` | API key for live smoke tests |
| `DNSCHECK_GROUP_UUID` | Optional specific group UUID |

Default CI jobs use mocked HTTP only. The optional **Console Sample Smoke** workflow reads `DNSCHECK_API_KEY` when configured — see [contributing/ci-live-smoke.md](contributing/ci-live-smoke.md).

## Account-wide listing

`Groups.ListAllAsync()` and `DnsRecords.ListAllAsync()` require an API key configured on the client. Omit the key only when using the public example group and other unauthenticated reads documented by DNS Check.

## Public example group

Group UUID `ea883d67-d9f6-45e3-b3a1-844dd1857824` is documented as not requiring a valid API key for read access to that group's monitoring data.
