namespace DnsCheck.Client;

/// <summary>
/// Thrown when a monitoring API operation is not yet implemented in this package version.
/// </summary>
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
