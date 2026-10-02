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
    /// <returns>The requested DNS record.</returns>
    /// <exception cref="DnsCheckRequestException">Thrown when <paramref name="groupUuid"/> is not a valid path segment.</exception>
    /// <exception cref="DnsCheckRequestException">Thrown when <paramref name="recordId"/> is not a positive integer.</exception>
    /// <exception cref="DnsCheckApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="DnsCheckParseException">Thrown when a successful response cannot be deserialised.</exception>
    Task<DnsRecord> GetAsync(string groupUuid, int recordId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all DNS records in a group.
    /// </summary>
    /// <param name="groupUuid">The DNS record group UUID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All DNS records in the group.</returns>
    /// <exception cref="DnsCheckRequestException">Thrown when <paramref name="groupUuid"/> is not a valid path segment.</exception>
    /// <exception cref="DnsCheckApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="DnsCheckParseException">Thrown when a successful response cannot be deserialised.</exception>
    Task<IReadOnlyList<DnsRecord>> ListInGroupAsync(string groupUuid, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all DNS records across every group for the authenticated account.
    /// </summary>
    /// <remarks>
    /// Implemented by calling <c>GET groups/all</c> and then <c>GET groups/{uuid}/all</c> for each group, sequentially.
    /// Large accounts may require many round trips; failures on any group abort the whole operation.
    /// DNS Check returns <c>401 Unauthorized</c> for <c>GET groups/all/all</c> on typical accounts.
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All DNS records visible to the API key.</returns>
    /// <exception cref="DnsCheckRequestException">Thrown when the client was constructed without an API key.</exception>
    /// <exception cref="DnsCheckApiException">Thrown when the API returns an error response.</exception>
    /// <exception cref="DnsCheckParseException">Thrown when a successful response cannot be deserialised.</exception>
    Task<IReadOnlyList<DnsRecord>> ListAllAsync(CancellationToken cancellationToken = default);
}
