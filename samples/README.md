# Samples

## MonitorConsole

Live smoke checks for the DNS Check monitoring API. Uses the **public example group** without an API key; account-wide checks are **skipped** unless `DNSCHECK_API_KEY` is set.

### Quick run (interactive menu)

```bash
dotnet run --project samples/MonitorConsole
```

### CI-style run (dotnet test-style summary)

```bash
dotnet run --project samples/MonitorConsole -- --run-all
```

This mode is used in CI. It prints green/red/yellow per check and a final **Passed!** / **Failed!** summary.

- **Build & Test** job: runs `--run-all` without a key (public example group; account checks skipped).
- **Console Sample Smoke** job: runs `--run-all` with the `DNSCHECK_API_KEY` repository secret when configured ([setup](../../docs/contributing/ci-live-smoke.md)).

### Environment

| Variable | Required | Description |
|----------|----------|-------------|
| `DNSCHECK_API_KEY` | No for public group | Enables `Groups.ListAllAsync` and `DnsRecords.ListAllAsync` checks |
| `DNSCHECK_GROUP_UUID` | No | Defaults to the documented public example group |

The API key is never printed.

### Understanding output

- **Passed** (green) means the SDK call succeeded (HTTP + JSON), not that every DNS record is healthy.
- **Fail** in monitoring status lines is **live data from DNS Check**. The public example group is documented to include failing checks on purpose.
- **Skipped** (yellow) means a check was not run (for example account-wide lists without an API key).

### Options

| Flag | Description |
|------|-------------|
| `--run-all`, `-a` | Run all checks non-interactively |
| `--help`, `-h` | Show usage |
