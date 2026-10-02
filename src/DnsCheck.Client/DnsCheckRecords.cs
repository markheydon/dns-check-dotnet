namespace DnsCheck.Client;

/// <summary>
/// Path tokens for DNS record API calls within a group.
/// </summary>
public static class DnsCheckRecords
{
    /// <summary>
    /// Path token for listing all DNS records in a group (<c>GET groups/{uuid}/all</c>).
    /// Use <see cref="Services.DnsRecords.IDnsRecordService.ListInGroupAsync"/> with a group UUID — this constant is not a
    /// <c>groupUuid</c> argument.
    /// </summary>
    public const string All = "all";
}
