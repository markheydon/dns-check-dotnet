namespace DnsCheck.Client;

/// <summary>
/// Path tokens for DNS record group API calls.
/// </summary>
public static class DnsCheckGroups
{
    /// <summary>
    /// Path token for listing all DNS record groups for the authenticated account
    /// (<c>GET groups/all</c>). Use <see cref="Services.Groups.IGroupService.ListAllAsync"/> — do not pass this value to
    /// <see cref="Services.Groups.IGroupService.GetAsync"/>.
    /// </summary>
    public const string All = "all";
}
