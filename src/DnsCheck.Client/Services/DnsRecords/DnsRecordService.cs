using DnsCheck.Client.Infrastructure;
using DnsCheck.Client.Infrastructure.Http;
using DnsCheck.Client.Models.DnsRecords;
using DnsCheck.Client.Models.Groups;

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
    public async Task<DnsRecord> GetAsync(string groupUuid, int recordId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ApiPathValidation.ValidateGroupUuid(groupUuid);

        if (recordId <= 0)
        {
            throw new DnsCheckRequestException("DNS record ID must be a positive integer.");
        }

        DnsRecordResponse response = await _rest.GetAsync<DnsRecordResponse>(
            $"groups/{groupUuid}/{recordId}",
            cancellationToken);

        return ApiResponseEnvelope.Require(response.DnsRecord, "dns_record");
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<DnsRecord>> ListInGroupAsync(
        string groupUuid,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ApiPathValidation.ValidateGroupUuid(groupUuid);

        DnsRecordsListResponse response = await _rest.GetAsync<DnsRecordsListResponse>(
            $"groups/{groupUuid}/{DnsCheckRecords.All}",
            cancellationToken);

        return ApiResponseEnvelope.RequireList(response.DnsRecords, "dns_records");
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<DnsRecord>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        AccountScopedRequests.RequireApiKey(_rest);

        GroupsListResponse groupsResponse = await _rest.GetAsync<GroupsListResponse>(
            $"groups/{DnsCheckGroups.All}",
            cancellationToken);

        IReadOnlyList<DnsRecordGroup> groups = ApiResponseEnvelope.RequireList(groupsResponse.Groups, "groups");
        if (groups.Count == 0)
        {
            return [];
        }

        List<DnsRecord> records = new();
        foreach (DnsRecordGroup group in groups)
        {
            cancellationToken.ThrowIfCancellationRequested();
            IReadOnlyList<DnsRecord> groupRecords = await ListInGroupAsync(group.Uuid, cancellationToken);
            records.AddRange(groupRecords);
        }

        return records;
    }
}
