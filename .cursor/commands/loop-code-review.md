# Looped code review

## Your job

Repeat this cycle until the review is clear of actionable findings, or after **6** complete cycles:

1. **Review** — Follow [.agents/skills/code-review/SKILL.md](../../.agents/skills/code-review/SKILL.md). Use the requested scope, or review branch changes against the repository default branch when no scope is given. Do not implement fixes during review.
2. **Fix** — Implement actionable findings using minimal, convention-matching diffs. Skip non-blocking nits.
3. **Verify** — From the repository root, run:

   ```shell
   dotnet format DnsCheck.slnx --verify-no-changes
   dotnet build DnsCheck.slnx -c Release -warnaserror
   dotnet test DnsCheck.slnx -c Release --no-build
   ```

   Resolve failures before the next loop. If the build/test environment is unavailable, state that clearly.

## Stop condition

- **All clear** — The code-review skill reports no significant issues. Summarise review results and validation.
- **Cap** — After **6** complete review cycles, stop and summarise remaining findings, fixes, and validation.

Do not commit, push, or perform GitHub pull-request actions unless the user explicitly asks.
