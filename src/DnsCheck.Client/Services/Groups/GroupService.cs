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
        _ = _rest;
        return ServiceAvailability.MonitoringNotImplemented<DnsRecordGroup>();
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<DnsRecordGroup>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        _ = _rest;
        return ServiceAvailability.MonitoringNotImplemented<IReadOnlyList<DnsRecordGroup>>();
    }
}
