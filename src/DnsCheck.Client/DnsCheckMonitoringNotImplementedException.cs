namespace DnsCheck.Client;

/// <summary>
/// Legacy exception type from early package versions. v1 monitoring operations are implemented; this type is never thrown.
/// Catch <see cref="DnsCheckApiException"/>, <see cref="DnsCheckRequestException"/>, or <see cref="DnsCheckParseException"/> instead.
/// </summary>
[Obsolete("DnsCheckMonitoringNotImplementedException is legacy-only and will be removed in 2.0.0. Catch DnsCheckApiException, DnsCheckRequestException, or DnsCheckParseException instead.")]
public sealed class DnsCheckMonitoringNotImplementedException : DnsCheckException
{
    /// <summary>
    /// Creates an exception with the specified message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public DnsCheckMonitoringNotImplementedException(string message)
        : base(message)
    {
    }
}
