using System.Net;

namespace DnsCheck.Client.Infrastructure;

/// <summary>
/// Validates required JSON envelope branches after deserialisation.
/// </summary>
internal static class ApiResponseEnvelope
{
    internal static T Require<T>(T? value, string envelopePropertyName)
        where T : class
    {
        if (value is null)
        {
            throw new DnsCheckApiException(
                HttpStatusCode.OK,
                $"The API response did not contain a required '{envelopePropertyName}' property.");
        }

        return value;
    }

    internal static IReadOnlyList<T> RequireList<T>(IReadOnlyList<T>? value, string envelopePropertyName)
    {
        if (value is null)
        {
            throw new DnsCheckApiException(
                HttpStatusCode.OK,
                $"The API response did not contain a required '{envelopePropertyName}' property.");
        }

        return value;
    }
}
