namespace DnsCheck.Client.Infrastructure.Http;

/// <summary>
/// Internal HTTP transport for DNS Check API v1 GET requests.
/// </summary>
/// <remarks>
/// <para>Request URIs are built from <see cref="BaseAddress"/>, not from <see cref="HttpClient.BaseAddress"/>,
/// so injected <see cref="HttpClient"/> instances without a base address still target the resolved API URL.</para>
/// <para>Response handling is implemented in a follow-up change. See <c>plan/IMPLEMENT_V1_API.md</c>.</para>
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

        if (apiKey is not null && string.IsNullOrWhiteSpace(apiKey))
        {
            throw new ArgumentException("API key cannot be empty or whitespace.", nameof(apiKey));
        }

        _apiKey = apiKey;
    }

    internal HttpClient HttpClient => _httpClient;

    internal Uri BaseAddress => _baseAddress;

    internal string? ApiKey => _apiKey;

    internal Uri BuildRequestUri(string relativePath, IReadOnlyList<RestQuery.QueryParameter>? queryParameters = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);

        List<RestQuery.QueryParameter> parameters = new((queryParameters?.Count ?? 0) + 1);
        if (!string.IsNullOrWhiteSpace(_apiKey))
        {
            parameters.Add(new RestQuery.QueryParameter("api_key", _apiKey));
        }

        if (queryParameters is not null)
        {
            parameters.AddRange(queryParameters);
        }

        string pathAndQuery = RestQuery.Append(relativePath, parameters);

        if (!Uri.TryCreate(_baseAddress, pathAndQuery, out Uri? requestUri))
        {
            throw new ArgumentException(
                $"Could not combine base address '{_baseAddress}' with path '{pathAndQuery}'.",
                nameof(relativePath));
        }

        return requestUri;
    }
}
