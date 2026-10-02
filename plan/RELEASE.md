# Release runbook

Maintainer guide for publishing `DnsCheck.Client` to NuGet.org. Policy: [VERSIONING.md](../VERSIONING.md).

## Prerequisites

- [ ] NuGet.org **Trusted Publishing** for `markheydon/dns-check-dotnet`, workflow `release.yml`, package `DnsCheck.Client`
- [ ] `NUGET_USER` repository secret set
- [ ] `<Version>` in `src/DnsCheck.Client/DnsCheck.Client.csproj` matches the intended tag
- [ ] `main` is green on CI

## Publish steps

1. Bump `<Version>` and refresh `<PackageReleaseNotes>` in `DnsCheck.Client.csproj` if needed.
2. Merge to `main`.
3. Tag and push, for example: `git tag v1.0.0 && git push origin v1.0.0`
4. Monitor the Release workflow.
5. Verify [nuget.org](https://www.nuget.org/packages/DnsCheck.Client) and GitHub Releases.

## First stable release

**1.0.0** covers all five documented monitoring operations (including composite `DnsRecords.ListAllAsync`). Earlier alpha tags were for pipeline and early-adopter validation only.
