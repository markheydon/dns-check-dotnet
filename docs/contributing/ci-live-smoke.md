# CI live API smoke

The **Console Sample Smoke** job in [`.github/workflows/ci.yml`](../../.github/workflows/ci.yml) runs `samples/MonitorConsole` with `--run-all` against the live DNS Check API using your account API key.

Default CI (`verify`, `build-and-test`) uses mocked HTTP in unit tests only. Authenticated smoke is an extra signal that account-wide list operations work with a real key. The public example group (no API key) is not exercised in CI; run `dotnet run --project samples/MonitorConsole -- --run-all` locally without `DNSCHECK_API_KEY` when you want a live check of unauthenticated reads.

## What you need

| Item | Purpose |
|------|---------|
| DNS Check API key | 32-character key from [Generating an API Key](https://www.dnscheck.co/api/generate-key) |
| GitHub repository secret | Store the key as `DNSCHECK_API_KEY` |

Use a **read-only monitoring** key if DNS Check offers restricted keys. The smoke job only performs GET requests (list groups, list records). Do not use a key with write access unless you accept the risk of future sample changes.

## Configure GitHub Actions

1. Open the repository **Settings** → **Secrets and variables** → **Actions**.
2. Add a repository secret:
   - **Name:** `DNSCHECK_API_KEY`
   - **Value:** your DNS Check API key (no quotes)
3. Re-run the **Console Sample Smoke** job on `main` (or wait for the next push).

Forks, pull requests, and `main` without this secret skip authenticated smoke with a log message. Configure the secret on your fork if you want weekly schedule or manual runs to exercise the live API.

## Local verification

```bash
export DNSCHECK_API_KEY='your-key-here'
dotnet run --project samples/MonitorConsole -- --run-all
```

Expect five checks **passed** (no skipped account-wide checks). The public example group may still show monitoring status **Fail** in output; that is expected API data.

## Schedule

CI runs authenticated smoke on a weekly schedule (Monday 06:00 UTC) when the secret is configured, matching the pattern used in [freeagent-dotnet](https://github.com/markheydon/freeagent-dotnet).
