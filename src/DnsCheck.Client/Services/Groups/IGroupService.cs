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
    /// <param name="groupUuid">The DNS record group UUID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The requested group.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown in prerelease package versions before monitoring API implementation is complete.
    /// </exception>
    Task<DnsRecordGroup> GetAsync(string groupUuid, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all DNS record groups for the authenticated account.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All groups visible to the API key.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown in prerelease package versions before monitoring API implementation is complete.
    /// </exception>
    Task<IReadOnlyList<DnsRecordGroup>> ListAllAsync(CancellationToken cancellationToken = default);
}
