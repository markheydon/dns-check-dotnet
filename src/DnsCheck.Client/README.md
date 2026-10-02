# DnsCheck.Client

Unofficial .NET client for the [DNS Check monitoring API](https://www.dnscheck.co/api) (DNS record groups and individual records). Not affiliated with DNS Check or Wind Serve, LLC.

Stable `1.x` with SemVer guarantees for the documented v1 monitoring GET API.

## Install

```bash
dotnet add package DnsCheck.Client
```

## Quick example

```csharp
using DnsCheck.Client;
using DnsCheck.Client.DependencyInjection;
using DnsCheck.Client.Models.DnsRecords;
using DnsCheck.Client.Models.Groups;

const string exampleGroup = "ea883d67-d9f6-45e3-b3a1-844dd1857824";

builder.Services.AddDnsCheckClient(options => options.ApiKey = "your-api-key");

DnsCheckClient client = /* resolve from DI */;

DnsRecordGroup group = await client.Groups.GetAsync(exampleGroup);
IReadOnlyList<DnsRecord> records = await client.DnsRecords.ListInGroupAsync(exampleGroup);
```

Omit `ApiKey` when using DNS Check's [public example group](https://www.dnscheck.co/api/dns-record-group-monitoring). Account-wide listing requires a key from [Generating an API Key](https://www.dnscheck.co/api/generate-key).

## Documentation

Getting started, authentication, and API coverage: [markheydon.me.uk/dns-check-dotnet](https://markheydon.me.uk/dns-check-dotnet/).

Source and issue tracker: [github.com/markheydon/dns-check-dotnet](https://github.com/markheydon/dns-check-dotnet)
