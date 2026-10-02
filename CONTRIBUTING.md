# Contributing to DnsCheck.Client

Thanks for your interest.

This project provides an unofficial .NET SDK for the [DNS Check monitoring API](https://www.dnscheck.co/api).

## Before You Start

- Read [GOALS.md](GOALS.md), [SCOPE.md](SCOPE.md), [CONVENTIONS.md](CONVENTIONS.md), and [VERSIONING.md](VERSIONING.md).
- Read [plan/IMPLEMENT_V1_API.md](plan/IMPLEMENT_V1_API.md) when adding API operations.

## Development Setup

Requirements:

- .NET 8.0 SDK (`net8.0` target).
- .NET 10.0 SDK (primary for samples). `global.json` pins `10.0.300` with `rollForward: latestPatch` within the 10.0.300 feature band.

```bash
dotnet clean DnsCheck.slnx && \
dotnet restore DnsCheck.slnx && \
dotnet build DnsCheck.slnx --no-restore --configuration Release -warnaserror && \
dotnet test DnsCheck.slnx --no-build --configuration Release
```

Verify formatting:

```bash
dotnet format DnsCheck.slnx --verify-no-changes
```

Live API smoke (optional locally; GitHub Actions runs it on `main` pushes, schedule, or manual dispatch when `DNSCHECK_API_KEY` is configured):

```bash
dotnet run --project samples/MonitorConsole -- --run-all
```

To enable authenticated smoke in GitHub Actions, add the `DNSCHECK_API_KEY` repository secret. See [docs/contributing/ci-live-smoke.md](docs/contributing/ci-live-smoke.md).

## Pull Requests

Follow [plan/PULL_REQUEST_POLICY.md](plan/PULL_REQUEST_POLICY.md) and [plan/LABEL_STRATEGY.md](plan/LABEL_STRATEGY.md).

## Code of Conduct

By participating, you agree to [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md).
