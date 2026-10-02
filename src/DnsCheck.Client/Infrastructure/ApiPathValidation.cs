namespace DnsCheck.Client.Infrastructure;

/// <summary>
/// Fail-fast checks for values that become URL path segments.
/// </summary>
internal static class ApiPathValidation
{
    internal static void ValidateRelativePath(string relativePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);

        if (relativePath[0] is '/' or '\\')
        {
            throw new DnsCheckRequestException("Relative path must not start with a path separator.");
        }

        if (relativePath.Contains('?', StringComparison.Ordinal)
            || relativePath.Contains('#', StringComparison.Ordinal))
        {
            throw new DnsCheckRequestException("Relative path must not contain query or fragment delimiters.");
        }

        if (relativePath.Contains('%', StringComparison.Ordinal))
        {
            throw new DnsCheckRequestException("Relative path must not contain percent-encoded segments.");
        }

        if (relativePath.Contains("..", StringComparison.Ordinal))
        {
            throw new DnsCheckRequestException("Relative path must not contain parent directory segments ('..').");
        }

        if (relativePath.Contains("://", StringComparison.Ordinal))
        {
            throw new DnsCheckRequestException("Relative path must not contain an absolute URI scheme.");
        }
    }

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

        if (string.Equals(groupUuid, DnsCheckGroups.All, StringComparison.OrdinalIgnoreCase))
        {
            throw new DnsCheckRequestException(
                "The path token 'all' is not a group UUID. Use IGroupService.ListAllAsync() to list all groups.");
        }

        if (!Guid.TryParse(groupUuid, out _))
        {
            throw new DnsCheckRequestException("Group UUID must be a valid GUID.");
        }
    }
}
