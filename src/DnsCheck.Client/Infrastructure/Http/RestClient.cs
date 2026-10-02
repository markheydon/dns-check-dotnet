namespace DnsCheck.Client.Infrastructure.Http;

/// <summary>
/// Internal HTTP transport for DNS Check API v1 GET requests.
/// </summary>
/// <remarks>
/// Request and response handling is implemented in a follow-up change. See <c>plan/IMPLEMENT_V1_API.md</c>.
/// </remarks>
internal sealed class RestClient
{
    private readonly HttpClient _httpClient;
    private readonly Uri _baseAddress;
    private readonly string? _apiKey;

    internal RestClient(HttpClient httpClient, Uri baseAddress, string? apiKey)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(baseAddress);

        _httpClient = httpClient;
        _baseAddress = baseAddress;
        _apiKey = apiKey;
    }

    internal HttpClient HttpClient => _httpClient;

    internal Uri BaseAddress => _baseAddress;

    internal string? ApiKey => _apiKey;
}
