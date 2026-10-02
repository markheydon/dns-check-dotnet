# DnsCheck.Client

Unofficial .NET client for the [DNS Check monitoring API](https://www.dnscheck.co/api).

## Installation

```bash
dotnet add package DnsCheck.Client
```

## Usage

```csharp
using DnsCheck.Client;
using DnsCheck.Client.Models.DnsRecords;
using DnsCheck.Client.Models.Groups;

using var client = new DnsCheckClient("your-api-key");

DnsRecordGroup group = await client.Groups.GetAsync("group-uuid");
IReadOnlyList<DnsRecord> records = await client.DnsRecords.ListInGroupAsync("group-uuid");
```

See the [repository documentation](https://github.com/markheydon/dns-check-dotnet/tree/main/docs) for authentication and API coverage.
