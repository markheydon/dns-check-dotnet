using DnsCheck.Client.Models.Groups;

namespace DnsCheck.Client.Services.Groups;

/// <summary>
/// DNS record group monitoring operations.
/// </summary>
public interface IGroupService
{
    /// <summary>
    /// Retrieves a single DNS record group by UUID.
    /// </summary>
    /// <param name="groupUuid">The group UUID, or <see cref="DnsCheckGroups.All"/> to list all groups.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<DnsRecordGroup> GetAsync(string groupUuid, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all DNS record groups for the authenticated account.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IReadOnlyList<DnsRecordGroup>> ListAllAsync(CancellationToken cancellationToken = default);
}
