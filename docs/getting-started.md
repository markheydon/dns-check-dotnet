# Getting started

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

## Example calls

```csharp
const string exampleGroup = "ea883d67-d9f6-45e3-b3a1-844dd1857824";

DnsRecordGroup group = await client.Groups.GetAsync(exampleGroup);
IReadOnlyList<DnsRecord> records = await client.DnsRecords.ListInGroupAsync(exampleGroup);

// Account-wide (API key required)
IReadOnlyList<DnsRecordGroup> groups = await client.Groups.ListAllAsync();
IReadOnlyList<DnsRecord> allRecords = await client.DnsRecords.ListAllAsync();
```

See [api-coverage.md](api-coverage.md) for the full operation list.
