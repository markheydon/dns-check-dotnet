namespace DnsCheck.Client;

/// <summary>
/// Thrown when a request violates a local contract constraint before any HTTP call is made.
/// </summary>
public sealed class DnsCheckRequestException : DnsCheckException
{
    /// <summary>
    /// Creates an exception with the specified message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public DnsCheckRequestException(string message)
        : base(message)
    {
    }
}
