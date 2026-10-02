namespace DnsCheck.Client.Tests.TestSupport;

internal static class DnsCheckClientTestAccess
{
    internal static HttpClient GetHttpClient(DnsCheck.Client.DnsCheckClient client) => client.TestHttpClient;

    internal static string? GetApiKey(DnsCheck.Client.DnsCheckClient client) => client.TestApiKey;
}
