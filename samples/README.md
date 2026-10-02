# Samples

## MonitorConsole

Opt-in live smoke test for the DNS Check monitoring API. **Not run in CI.**

```bash
export DNSCHECK_API_KEY='your-key'   # optional for public example group only
dotnet run --project samples/MonitorConsole
```

| Variable | Required | Description |
|----------|----------|-------------|
| `DNSCHECK_API_KEY` | No for example group | Account API key (enables account-wide list calls) |
| `DNSCHECK_GROUP_UUID` | No | Defaults to the documented public example group |

Exits `0` on success and `1` when a `DnsCheckException` is thrown. The API key is never printed.
