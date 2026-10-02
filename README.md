# DNS Check .NET Client

Unofficial .NET client for the [DNS Check monitoring API](https://www.dnscheck.co/api). Not affiliated with DNS Check or Wind Serve, LLC.

[![CI](https://github.com/markheydon/dns-check-dotnet/actions/workflows/ci.yml/badge.svg)](https://github.com/markheydon/dns-check-dotnet/actions/workflows/ci.yml)

> **Prerelease (`0.1.0-alpha.1`).** Repository scaffold is in place; the four documented GET operations are implemented in a follow-up change. See [plan/IMPLEMENT_V1_API.md](plan/IMPLEMENT_V1_API.md).

## Features (target v1)

- DNS record group and DNS record monitoring (read-only GET API)
- Typed models and exceptions
- Targets .NET 8.0 and .NET 10.0
- Fully async with XML documentation

## Installation

```bash
dotnet add package DnsCheck.Client
```

## Quick start

```csharp
using DnsCheck.Client;

using var client = new DnsCheckClient("your-api-key");
// Groups.GetAsync / DnsRecords.GetAsync — see plan/IMPLEMENT_V1_API.md
```

**Documentation:** [docs/](docs/README.md)

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
