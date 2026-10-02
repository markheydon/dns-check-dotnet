# API coverage

Mapped to [DNS Check API v1](https://www.dnscheck.co/api).

| Operation | SDK | Status |
|-----------|-----|--------|
| Get DNS record group | `Groups.GetAsync` | Implemented |
| List all DNS record groups | `Groups.ListAllAsync` | Implemented |
| Get DNS record | `DnsRecords.GetAsync` | Implemented |
| List DNS records in group | `DnsRecords.ListInGroupAsync` | Implemented |
| List all DNS records (account) | `DnsRecords.ListAllAsync` | Implemented (sequential `groups/all` + one `groups/{uuid}/all` per group; no paging) |
