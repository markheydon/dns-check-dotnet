# Getting started

## Install

```bash
dotnet add package DnsCheck.Client
```

## Register the client

In applications using dependency injection:

```csharp
using DnsCheck.Client;
using DnsCheck.Client.DependencyInjection;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDnsCheckClient(options =>
{
    options.ApiKey = builder.Configuration["DnsCheck:ApiKey"];
});

// Resolve DnsCheckClient from DI (for example in a hosted service or minimal API endpoint).
```

For console tools, build a small `ServiceCollection`, call `AddDnsCheckClient`, and resolve `DnsCheckClient` from the provider. See [`samples/MonitorConsole`](../samples/README.md).

### Tests and advanced scenarios

Inject a test `HttpClient` (for example with `QueuedHttpMessageHandler`):

```csharp
using HttpClient httpClient = new(handler);
DnsCheckClient client = new(httpClient, apiKey: "test-api-key");
```

## Example calls

```csharp
using DnsCheck.Client.Models.DnsRecords;
using DnsCheck.Client.Models.Groups;

const string exampleGroup = "ea883d67-d9f6-45e3-b3a1-844dd1857824";

DnsRecordGroup group = await client.Groups.GetAsync(exampleGroup);
IReadOnlyList<DnsRecord> records = await client.DnsRecords.ListInGroupAsync(exampleGroup);

// Account-wide (API key required)
IReadOnlyList<DnsRecordGroup> groups = await client.Groups.ListAllAsync();
IReadOnlyList<DnsRecord> allRecords = await client.DnsRecords.ListAllAsync();
```

The public example group can be used without an API key when `DnsCheckClientOptions.ApiKey` is unset. See [authentication.md](authentication.md).

See [api-coverage.md](api-coverage.md) for the full operation list.
