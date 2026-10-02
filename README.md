# DNS Check .NET Client

Unofficial .NET client for the [DNS Check monitoring API](https://www.dnscheck.co/api). Not affiliated with DNS Check or Wind Serve, LLC.

[![CI](https://github.com/markheydon/dns-check-dotnet/actions/workflows/ci.yml/badge.svg)](https://github.com/markheydon/dns-check-dotnet/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/DnsCheck.Client.svg)](https://www.nuget.org/packages/DnsCheck.Client/)

> **Stable `1.0.0`.** All documented v1 monitoring GET operations are implemented. Public API changes follow SemVer — see [VERSIONING.md](VERSIONING.md).

**Documentation:** [markheydon.me.uk/dns-check-dotnet](https://markheydon.me.uk/dns-check-dotnet/) — source in [`docs/`](docs/README.md).

## Features

- DNS record group and DNS record monitoring (read-only GET API)
- Typed models and exceptions
- Targets .NET 8.0 and .NET 10.0
- Fully async with XML documentation
- `AddDnsCheckClient` for `IHttpClientFactory` integration

## Installation

```bash
dotnet add package DnsCheck.Client
```

## Quick start

```csharp
using DnsCheck.Client;
using DnsCheck.Client.DependencyInjection;
using DnsCheck.Client.Models.Groups;
using DnsCheck.Client.Models.DnsRecords;
using Microsoft.Extensions.DependencyInjection;

const string exampleGroup = "ea883d67-d9f6-45e3-b3a1-844dd1857824";

ServiceCollection services = new();
services.AddDnsCheckClient(options => options.ApiKey = "your-api-key");

DnsCheckClient client = services.BuildServiceProvider().GetRequiredService<DnsCheckClient>();

DnsRecordGroup group = await client.Groups.GetAsync(exampleGroup);
IReadOnlyList<DnsRecord> records = await client.DnsRecords.ListInGroupAsync(exampleGroup);
```

Omit `ApiKey` in `AddDnsCheckClient` for the [public example group](https://www.dnscheck.co/api/dns-record-group-monitoring). See [Getting started](docs/getting-started.md) for registration and test `HttpClient` patterns.

## Building from source

```bash
git clone https://github.com/markheydon/dns-check-dotnet.git
cd dns-check-dotnet
dotnet clean DnsCheck.slnx && \
dotnet restore DnsCheck.slnx && \
dotnet build DnsCheck.slnx --no-restore --configuration Release -warnaserror && \
dotnet test DnsCheck.slnx --no-build --configuration Release
```

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md). By participating, you agree to [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md).

## Support and security

- [SUPPORT.md](SUPPORT.md)
- [SECURITY.md](SECURITY.md)

## Licence

MIT — see [LICENSE](LICENSE).

## Resources

- [DNS Check API documentation](https://www.dnscheck.co/api)
- [Console sample](samples/README.md)
