using DnsCheck.Client.Infrastructure.Http;

namespace DnsCheck.Client.Infrastructure;

/// <summary>
/// Guards for API operations that require an authenticated account.
/// </summary>
internal static class AccountScopedRequests
{
    internal static void RequireApiKey(RestClient rest)
    {
        if (string.IsNullOrWhiteSpace(rest.ApiKey))
        {
            throw new DnsCheckRequestException(
                "This operation requires an API key. Configure DnsCheckClientOptions.ApiKey in AddDnsCheckClient or pass a key to the HttpClient constructor.");
        }
    }
}
