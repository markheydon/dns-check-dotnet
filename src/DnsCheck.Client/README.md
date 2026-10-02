# DnsCheck.Client

Unofficial .NET client for the [DNS Check monitoring API](https://www.dnscheck.co/api). Not affiliated with DNS Check or Wind Serve, LLC.

> **Prerelease (`0.1.0-alpha.1`).** Monitoring GET operations are not implemented in this package version; service methods return a faulted task until v1 HTTP work lands. See [IMPLEMENT_V1_API.md](https://github.com/markheydon/dns-check-dotnet/blob/main/plan/IMPLEMENT_V1_API.md) in the repository.

## Installation

```bash
dotnet add package DnsCheck.Client
```

## Usage

```csharp
using DnsCheck.Client;

using var client = new DnsCheckClient("your-api-key");
// Groups.GetAsync / DnsRecords.GetAsync — see repository docs when implemented.
```

Further documentation: [repository README](https://github.com/markheydon/dns-check-dotnet) and [docs/](https://github.com/markheydon/dns-check-dotnet/tree/main/docs).
