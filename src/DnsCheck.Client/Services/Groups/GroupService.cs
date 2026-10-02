using DnsCheck.Client.Infrastructure;
using DnsCheck.Client.Infrastructure.Http;
using DnsCheck.Client.Models.Groups;

namespace DnsCheck.Client.Services.Groups;

/// <summary>
/// DNS record group monitoring operations.
/// </summary>
public sealed class GroupService : IGroupService
{
    private readonly RestClient _rest;

    internal GroupService(RestClient rest)
    {
        ArgumentNullException.ThrowIfNull(rest);
        _rest = rest;
    }

    /// <inheritdoc />
    public Task<DnsRecordGroup> GetAsync(string groupUuid, CancellationToken cancellationToken = default)
    {
        ApiPathValidation.ValidateGroupUuid(groupUuid);
        return ServiceAvailability.MonitoringNotImplemented<DnsRecordGroup>(cancellationToken);
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<DnsRecordGroup>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_rest.ApiKey))
        {
            throw new DnsCheckRequestException(
                "Listing all DNS record groups requires an API key. Use DnsCheckClient(string apiKey) or pass a key to the HttpClient constructor.");
        }

        return ServiceAvailability.MonitoringNotImplemented<IReadOnlyList<DnsRecordGroup>>(cancellationToken);
    }

    internal RestClient RestClient => _rest;
}
