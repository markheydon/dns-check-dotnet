namespace DnsCheck.Client.Infrastructure;

/// <summary>
/// Helpers for consistent async return shapes on monitoring service stubs.
/// </summary>
internal static class RequestTasks
{
    internal static Task<T> FromValidationThenStub<T>(Action validation, CancellationToken cancellationToken)
    {
        try
        {
            validation();
        }
        catch (DnsCheckRequestException ex)
        {
            return Task.FromException<T>(ex);
        }

        return ServiceAvailability.MonitoringNotImplemented<T>(cancellationToken);
    }
}
