namespace DnsCheck.Client.Infrastructure;

/// <summary>
/// Fail-fast checks for values that become URL path segments.
/// </summary>
internal static class ApiPathValidation
{
    internal static void ValidateGroupUuid(string groupUuid)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(groupUuid);

        if (groupUuid.Contains('?', StringComparison.Ordinal)
            || groupUuid.Contains('#', StringComparison.Ordinal)
            || groupUuid.Contains('/', StringComparison.Ordinal)
            || groupUuid.Contains('\\', StringComparison.Ordinal))
        {
            throw new ArgumentException("Group UUID must not contain URL or path delimiter characters.", nameof(groupUuid));
        }

        if (!Guid.TryParse(groupUuid, out _))
        {
            throw new ArgumentException("Group UUID must be a valid GUID.", nameof(groupUuid));
        }
    }
}
