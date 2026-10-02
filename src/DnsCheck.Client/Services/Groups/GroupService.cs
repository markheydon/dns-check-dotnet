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
    public async Task<DnsRecordGroup> GetAsync(string groupUuid, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ApiPathValidation.ValidateGroupUuid(groupUuid);
        GroupResponse response = await _rest.GetAsync<GroupResponse>(
            $"groups/{groupUuid}",
            cancellationToken);

        return ApiResponseEnvelope.Require(response.Group, "group");
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<DnsRecordGroup>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        AccountScopedRequests.RequireApiKey(_rest);

        GroupsListResponse response = await _rest.GetAsync<GroupsListResponse>(
            $"groups/{DnsCheckGroups.All}",
            cancellationToken);

        return ApiResponseEnvelope.RequireList(response.Groups, "groups");
    }
}
