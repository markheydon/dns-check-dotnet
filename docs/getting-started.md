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

## File-based apps (.NET 10+)

You can run a minimal quick start from a single `.cs` file with `#:package` (no `.csproj`). The client uses source-generated JSON metadata for monitoring API responses, so you do not need `JsonSerializerIsReflectionEnabledByDefault=true`.

```csharp
#:package DnsCheck.Client@1.0.2

using DnsCheck.Client;
using DnsCheck.Client.Models.Groups;

using var httpClient = new HttpClient();
DnsCheckClient client = new(httpClient, apiKey: null);

const string exampleGroup = "ea883d67-d9f6-45e3-b3a1-844dd1857824";

DnsRecordGroup group = await client.Groups.GetAsync(exampleGroup);
```

See [`samples/FileBasedProducts`](../samples/README.md) for the repository CI smoke script.

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

## Errors and exceptions

The SDK maps DNS Check API and client-contract failures to `DnsCheckException` derivatives:

| Exception | When |
|-----------|------|
| `DnsCheckRequestException` | Invalid path segments, missing API key for account-scoped calls, or other client-side request rules |
| `DnsCheckApiException` | Non-success HTTP status from DNS Check (for example 401, 404) |
| `DnsCheckParseException` | Success status but JSON that cannot be deserialised or is missing required envelope properties |

`DnsCheckApiException` inherits from `DnsCheckHttpException`, which exposes `StatusCode`.

Network connectivity problems, TLS failures, and DNS resolution errors surface as standard .NET types (for example `HttpRequestException`) from `HttpClient`. Cancelled operations propagate `OperationCanceledException` (including `TaskCanceledException` when the token or client timeout fires). The SDK does not wrap those transport-layer failures.

`DnsCheckMonitoringNotImplementedException` remains for binary compatibility only; it is obsolete, is never thrown in current releases.
