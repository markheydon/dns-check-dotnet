using System.Net;
using System.Text.Json;
using DnsCheck.Client.Infrastructure.Serialization;

namespace DnsCheck.Client.Infrastructure.Http;

/// <summary>
/// Internal HTTP transport for DNS Check API v1 GET requests.
/// </summary>
/// <remarks>
/// <para>Request URIs are built from <see cref="BaseAddress"/>, not from <see cref="HttpClient.BaseAddress"/>,
/// so injected <see cref="HttpClient"/> instances without a base address still target the resolved API URL.</para>
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

    internal async Task<T> GetAsync<T>(string relativePath, CancellationToken cancellationToken = default)
    {
        Uri requestUri = BuildRequestUri(relativePath);
        using HttpResponseMessage response = await _httpClient.GetAsync(requestUri, cancellationToken);
        string body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            string detail = ParseErrorDetail(body) ?? response.ReasonPhrase ?? "The DNS Check API returned an error.";
            throw new DnsCheckApiException(response.StatusCode, detail);
        }

        try
        {
            return JsonSerializer.Deserialize<T>(body, DnsCheckJsonSerializerOptions.Default)
                ?? throw new DnsCheckParseException("The API response body was empty or could not be deserialised.");
        }
        catch (JsonException ex)
        {
            throw new DnsCheckParseException("The API response body could not be deserialised.", ex);
        }
    }

    internal Uri BuildRequestUri(string relativePath, IReadOnlyList<RestQuery.QueryParameter>? queryParameters = null)
    {
        ApiPathValidation.ValidateRelativePath(relativePath);

        List<RestQuery.QueryParameter> parameters = new((queryParameters?.Count ?? 0) + 1);
        if (!string.IsNullOrWhiteSpace(_apiKey))
        {
            parameters.Add(new RestQuery.QueryParameter("api_key", _apiKey));
        }

        if (queryParameters is not null)
        {
            foreach (RestQuery.QueryParameter parameter in queryParameters)
            {
                if (string.Equals(parameter.Name, "api_key", StringComparison.OrdinalIgnoreCase))
                {
                    throw new DnsCheckRequestException(
                        "The api_key query parameter is added by the SDK and must not be supplied in queryParameters.");
                }
            }

            parameters.AddRange(queryParameters);
        }

        string pathAndQuery = RestQuery.Append(relativePath, parameters);

        if (!Uri.TryCreate(_baseAddress, pathAndQuery, out Uri? requestUri))
        {
            throw new DnsCheckRequestException(
                $"Could not combine base address '{_baseAddress}' with relative path '{relativePath}'.");
        }

        return requestUri;
    }

    private static string? ParseErrorDetail(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<string>(body);
        }
        catch (JsonException)
        {
            return body.Trim();
        }
    }
}
