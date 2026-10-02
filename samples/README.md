# Samples

## MonitorConsole

Opt-in live smoke test for the DNS Check monitoring API. **Not run in CI.**

```bash
export DNSCHECK_API_KEY='your-key'   # optional for public example group only
dotnet run --project samples/MonitorConsole
```

| Variable | Required | Description |
|----------|----------|-------------|
| `DNSCHECK_API_KEY` | No for example group | Account API key |
| `DNSCHECK_GROUP_UUID` | No | Defaults to the documented public example group |

Until the API is implemented, the sample prints a placeholder message and exits 0.
