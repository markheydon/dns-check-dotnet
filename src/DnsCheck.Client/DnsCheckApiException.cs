using System.Net;

namespace DnsCheck.Client;

/// <summary>
/// Thrown when the DNS Check API returns a documented error response.
/// </summary>
public sealed class DnsCheckApiException : DnsCheckHttpException
{
    /// <summary>
    /// Creates an exception from an API error response.
    /// </summary>
    /// <param name="statusCode">The HTTP status code returned by the API.</param>
    /// <param name="detail">The API error detail message.</param>
    public DnsCheckApiException(HttpStatusCode statusCode, string detail)
        : base(statusCode, detail)
    {
        Detail = detail;
    }

    /// <summary>
    /// Gets the API error detail message.
    /// </summary>
    public string Detail { get; }
}
