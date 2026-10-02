namespace DnsCheck.Client.Infrastructure;

internal static class ServiceAvailability
{
    internal const string MonitoringNotImplementedMessage =
        "DNS Check monitoring API calls are not implemented in this package version. "
        + "See https://github.com/markheydon/dns-check-dotnet/blob/main/plan/IMPLEMENT_V1_API.md.";

    internal static Task<T> MonitoringNotImplemented<T>(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromException<T>(new InvalidOperationException(MonitoringNotImplementedMessage));
    }
}
