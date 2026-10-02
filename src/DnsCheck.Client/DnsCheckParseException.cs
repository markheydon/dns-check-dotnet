namespace DnsCheck.Client;

/// <summary>
/// Thrown when a successful HTTP response body cannot be deserialised into the expected model.
/// </summary>
public sealed class DnsCheckParseException : DnsCheckException
{
    /// <summary>
    /// Creates an exception with the specified message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public DnsCheckParseException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Creates an exception with the specified message and inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public DnsCheckParseException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
