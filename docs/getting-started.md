# Getting started

> **Prerelease.** The monitoring API is not fully implemented yet. See [api-coverage.md](api-coverage.md).

## Install

```bash
dotnet add package DnsCheck.Client
```

## Create a client

```csharp
using DnsCheck.Client;

using var client = new DnsCheckClient("your-api-key");
```

The public example DNS record group can be accessed without a valid API key. See [authentication.md](authentication.md).

## Next steps

When v1 is complete, use `client.Groups` and `client.DnsRecords` for monitoring calls. See [plan/IMPLEMENT_V1_API.md](../plan/IMPLEMENT_V1_API.md).
