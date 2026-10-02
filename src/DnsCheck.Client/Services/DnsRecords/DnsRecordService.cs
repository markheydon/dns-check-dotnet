using DnsCheck.Client.Infrastructure.Http;
using DnsCheck.Client.Models.DnsRecords;

namespace DnsCheck.Client.Services.DnsRecords;

/// <summary>
/// DNS record monitoring operations within a group.
/// </summary>
public sealed class DnsRecordService : IDnsRecordService
{
    private readonly RestClient _rest;

    internal DnsRecordService(RestClient rest)
    {
        ArgumentNullException.ThrowIfNull(rest);
        _rest = rest;
    }

    /// <inheritdoc />
    public Task<DnsRecord> GetAsync(string groupUuid, int recordId, CancellationToken cancellationToken = default)
    {
        _ = _rest;
        throw new NotImplementedException("DNS record monitoring API is not implemented yet. See plan/IMPLEMENT_V1_API.md.");
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<DnsRecord>> ListInGroupAsync(string groupUuid, CancellationToken cancellationToken = default)
    {
        _ = _rest;
        throw new NotImplementedException("DNS record monitoring API is not implemented yet. See plan/IMPLEMENT_V1_API.md.");
    }
}
