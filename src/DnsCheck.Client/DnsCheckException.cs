namespace DnsCheck.Client;

/// <summary>
/// Base exception for DNS Check SDK failures.
/// </summary>
public class DnsCheckException : Exception
{
    /// <summary>
    /// Creates an exception with the specified message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public DnsCheckException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Creates an exception with the specified message and inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public DnsCheckException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
