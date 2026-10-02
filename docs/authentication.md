# Authentication

DNS Check uses a 32-character API key passed as the `api_key` query parameter on each GET request.

Generate a key: [DNS Check — Generating an API Key](https://www.dnscheck.co/api/generate-key).

## SDK usage

```csharp
using var client = new DnsCheckClient(apiKey);
```

The `DnsCheckClient(string apiKey)` constructor requires a non-empty API key. Use `new DnsCheckClient()` when you do not have a key (for example the public example group). Optional keys on the `HttpClient` overload must be `null` or a non-whitespace value.

Treat the key as a secret. Do not commit it to source control.

## Environment variables (samples)

| Variable | Purpose |
|----------|---------|
| `DNSCHECK_API_KEY` | API key for live smoke tests |
| `DNSCHECK_GROUP_UUID` | Optional specific group UUID |

CI uses mocked HTTP only; it does not read these variables.

## Account-wide listing

`Groups.ListAllAsync()` requires a client constructed with an API key. The parameterless `DnsCheckClient()` constructor is intended for the public example group and other unauthenticated reads documented by DNS Check.

## Public example group

Group UUID `ea883d67-d9f6-45e3-b3a1-844dd1857824` is documented as not requiring a valid API key for read access.
