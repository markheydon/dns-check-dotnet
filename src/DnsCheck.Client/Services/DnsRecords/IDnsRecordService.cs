using DnsCheck.Client.Models.DnsRecords;

namespace DnsCheck.Client.Services.DnsRecords;

/// <summary>
/// DNS record monitoring operations within a group.
/// </summary>
public interface IDnsRecordService
{
    /// <summary>
    /// Retrieves a single DNS record by ID within a group.
    /// </summary>
    /// <param name="groupUuid">The DNS record group UUID.</param>
    /// <param name="recordId">The DNS record ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<DnsRecord> GetAsync(string groupUuid, int recordId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all DNS records in a group.
    /// </summary>
    /// <param name="groupUuid">The DNS record group UUID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IReadOnlyList<DnsRecord>> ListInGroupAsync(string groupUuid, CancellationToken cancellationToken = default);
}
