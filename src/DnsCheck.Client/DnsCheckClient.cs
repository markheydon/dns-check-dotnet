using DnsCheck.Client.Infrastructure.Configuration;
using DnsCheck.Client.Infrastructure.Http;
using DnsCheck.Client.Services.DnsRecords;
using DnsCheck.Client.Services.Groups;

namespace DnsCheck.Client;

/// <summary>
/// Client for the DNS Check monitoring API.
/// </summary>
/// <remarks>
/// Register with <see cref="DependencyInjection.DnsCheckClientServiceCollectionExtensions.AddDnsCheckClient"/>.
/// For tests, use <see cref="DnsCheckClient(HttpClient, string?)"/> with a dedicated <see cref="HttpClient"/>.
/// When you supply an <see cref="HttpClient"/> via the constructor overload, this instance does not take ownership:
/// the caller must keep the <see cref="HttpClient"/> alive for the lifetime of this <see cref="DnsCheckClient"/>.
/// </remarks>
public sealed class DnsCheckClient
{
    /// <summary>
    /// Default DNS Check API v1 base URL.
    /// </summary>
    public const string DefaultBaseUrl = "https://www.dnscheck.co/api/v1/";

    private readonly HttpClient _httpClient;
    private readonly RestClient _rest;

    /// <summary>
    /// Creates a client that uses the supplied <see cref="HttpClient"/>.
    /// </summary>
    /// <param name="httpClient">HTTP client instance. The SDK does not mutate <see cref="HttpClient.DefaultRequestHeaders"/>.</param>
    /// <param name="apiKey">Optional DNS Check API key. When provided, must not be empty or whitespace.</param>
    public DnsCheckClient(HttpClient httpClient, string? apiKey = null)
    {
        ArgumentNullException.ThrowIfNull(httpClient);

        _httpClient = httpClient;
        Uri resolvedBase = ResolveBaseAddress(httpClient, baseAddress: null);
        _rest = new RestClient(httpClient, resolvedBase, NormalizeOptionalApiKey(apiKey));
        Groups = new GroupService(_rest);
        DnsRecords = new DnsRecordService(_rest);
    }

    /// <summary>
    /// DNS record group monitoring.
    /// </summary>
    public IGroupService Groups { get; }

    /// <summary>
    /// DNS record monitoring within groups.
    /// </summary>
    public IDnsRecordService DnsRecords { get; }

    internal HttpClient TestHttpClient => _httpClient;

    internal RestClient TestRestClient => _rest;

    internal string? TestApiKey => _rest.ApiKey;

    private static string? NormalizeOptionalApiKey(string? apiKey)
    {
        if (apiKey is null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new ArgumentException("API key cannot be empty or whitespace.", nameof(apiKey));
        }

        return apiKey;
    }

    private static Uri ResolveBaseAddress(HttpClient httpClient, Uri? baseAddress)
    {
        if (baseAddress is not null)
        {
            return HttpClientConfiguration.NormalizeBaseAddress(baseAddress);
        }

        if (httpClient.BaseAddress is not null)
        {
            return HttpClientConfiguration.NormalizeBaseAddress(httpClient.BaseAddress);
        }

        return HttpClientConfiguration.NormalizeBaseAddress(new Uri(DefaultBaseUrl));
    }
}
