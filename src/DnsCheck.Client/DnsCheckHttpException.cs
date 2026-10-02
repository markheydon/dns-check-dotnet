using System.Net;

namespace DnsCheck.Client;

/// <summary>
/// Thrown when the DNS Check API returns a non-success HTTP status code.
/// </summary>
public class DnsCheckHttpException : DnsCheckException
{
    /// <summary>
    /// Creates an exception for the specified HTTP status code.
    /// </summary>
    /// <param name="statusCode">The HTTP status code returned by the API.</param>
    /// <param name="message">The error message.</param>
    public DnsCheckHttpException(HttpStatusCode statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode;
    }

    /// <summary>
    /// Gets the HTTP status code returned by the API.
    /// </summary>
    public HttpStatusCode StatusCode { get; }
}
