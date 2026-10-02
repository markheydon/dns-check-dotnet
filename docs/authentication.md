# Authentication

DNS Check uses a 32-character API key passed as the `api_key` query parameter on each GET request.

Generate a key: [DNS Check — Generating an API Key](https://www.dnscheck.co/api/generate-key).

## SDK usage

```csharp
using var client = new DnsCheckClient(apiKey);
```

Treat the key as a secret. Do not commit it to source control.

## Environment variables (samples)

| Variable | Purpose |
|----------|---------|
| `DNSCHECK_API_KEY` | API key for live smoke tests |
| `DNSCHECK_GROUP_UUID` | Optional specific group UUID |

CI uses mocked HTTP only; it does not read these variables.

## Public example group

Group UUID `ea883d67-d9f6-45e3-b3a1-844dd1857824` is documented as not requiring a valid API key for read access.
