namespace DnsCheck.Client.Infrastructure;

/// <summary>
/// Fail-fast checks for values that become URL path segments.
/// </summary>
internal static class ApiPathValidation
{
    internal static void ValidateGroupUuid(string groupUuid)
    {
        if (string.IsNullOrWhiteSpace(groupUuid))
        {
            throw new DnsCheckRequestException("Group UUID is required.");
        }

        if (groupUuid.Contains('?', StringComparison.Ordinal)
            || groupUuid.Contains('#', StringComparison.Ordinal)
            || groupUuid.Contains('/', StringComparison.Ordinal)
            || groupUuid.Contains('\\', StringComparison.Ordinal))
        {
            throw new DnsCheckRequestException("Group UUID must not contain URL or path delimiter characters.");
        }

        if (!Guid.TryParse(groupUuid, out _))
        {
            throw new DnsCheckRequestException("Group UUID must be a valid GUID.");
        }
    }
}
