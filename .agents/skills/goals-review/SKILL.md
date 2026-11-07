---
name: goals-review
description: Audit the repository or a requested change against DnsCheck.Client goals, scope, conventions, and sample-sync rules.
---

# Goals Review

Audit the repository or requested diff for alignment with the DNS Check client goals and scope. This is a **read-only review** unless the user explicitly asks for fixes.

## Required reading

Read these before assessing:

1. [GOALS.md](../../GOALS.md) - G1-G5 and kill criteria
2. [SCOPE.md](../../SCOPE.md) - in/out of scope for v1.0
3. [CONVENTIONS.md](../../CONVENTIONS.md) - SDK and testing patterns
4. [AGENTS.md](../../AGENTS.md) - sample sync and repository workflow

## Scope of review

Determine scope from user context:

| Trigger | Scope |
|---------|--------|
| No extra context | Whole-repository alignment check, with focused reads of relevant code and docs |
| PR, branch, or local changes | Review the requested diff and verify affected docs and sample behaviour |
| Mentioned paths | Review those paths against relevant goals, scope, and conventions |

When reviewing code changes, run the Release build and tests from [AGENTS.md](../../AGENTS.md) if the environment supports them. Report commands and results.

## Checklist

### G1 - Clean .NET monitoring SDK

- [ ] Public services and models cover documented v1 monitoring operations with typed APIs.
- [ ] New serialised model properties have explicit `JsonPropertyName` mappings.
- [ ] Consumer-facing operations do not require callers to construct HTTP paths or parse response envelopes.

### G2 - Discoverable operations

- [ ] Operations are discoverable from `DnsCheckClient.Groups` and `DnsCheckClient.DnsRecords`.
- [ ] Changes do not force callers to reconstruct documented paths or query strings.

### G3 - Typed models and errors

- [ ] API status and record types retain their documented typed representations.
- [ ] HTTP and parse failures use the SDK exception hierarchy rather than being swallowed.

### G4 - API-key safety

- [ ] The caller supplies the API key; it is not logged, printed, or persisted by the SDK.
- [ ] Samples and tests do not expose real API keys.

### G5 - Contribution-friendly maintenance

- [ ] Public API and architecture changes are intentional and consistent with accepted ADRs.
- [ ] Behaviour changes have appropriate tests and, when API operations change, the console sample and API coverage documentation are kept in sync.

### Scope boundaries

- [ ] SDK changes stay within the documented, GET-only DNS Check v1 monitoring API.
- [ ] No write APIs, enterprise webhooks, UI, or unrelated CLI product are introduced without an explicit scope change.
- [ ] Documentation uses UK English and matches implemented SDK behaviour.

## Output format

```markdown
# Goals review - <scope>

**Verdict:** Aligned | Minor drift | Significant drift

## Summary
<2-4 sentences>

## Findings

| ID | Severity | Goal | Finding | Evidence |
|----|----------|------|---------|----------|
| F1 | high/medium/low | G2 | ... | file:line or diff |

## Passing checks
- Bullet list of goals/areas with no issues found

## Recommended actions
1. Ordered list - fixes first, then preventive measures
```

Severity guide:

- **high** - violates scope, a goal, or the public API contract; should block merge
- **medium** - meaningful inconsistency or documentation/sample drift
- **low** - minor documentation or maintainability improvement

Do not file GitHub issues. Do not edit files unless the user asks you to remediate findings.
