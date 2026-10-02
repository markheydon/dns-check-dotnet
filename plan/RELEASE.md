# Release runbook

Maintainer guide for publishing `DnsCheck.Client` to NuGet.org. Policy: [VERSIONING.md](../VERSIONING.md).

## Prerequisites

- [ ] NuGet.org **Trusted Publishing** for `markheydon/dns-check-dotnet`, workflow `release.yml`, package `DnsCheck.Client`
- [ ] `NUGET_USER` repository secret set
- [ ] `<Version>` in `src/DnsCheck.Client/DnsCheck.Client.csproj` matches the intended tag
- [ ] `main` is green on CI

## Publish steps

1. Bump `<Version>` if needed.
2. Merge to `main`.
3. Tag and push: `git tag v0.1.0-alpha.1 && git push origin v0.1.0-alpha.1`
4. Monitor the Release workflow.
5. Verify [nuget.org](https://www.nuget.org/packages/DnsCheck.Client) and GitHub Releases.

## First public package

Prefer the first NuGet listing after the v1 monitoring API is implemented (four GET operations, tests, working console sample). Scaffold-only tags are optional for pipeline validation only.
