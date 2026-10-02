using DnsCheck.Client.Infrastructure.Configuration;
using DnsCheck.Client.Infrastructure.Http;
using DnsCheck.Client.Services.DnsRecords;
using DnsCheck.Client.Services.Groups;

namespace DnsCheck.Client;

/// <summary>
/// Client for the DNS Check monitoring API.
/// </summary>
public sealed class DnsCheckClient : IDisposable
{
    /// <summary>
    /// Default DNS Check API v1 base URL.
    /// </summary>
    public const string DefaultBaseUrl = "https://www.dnscheck.co/api/v1/";

    private readonly HttpClient _httpClient;
    private readonly bool _ownsHttpClient;

    /// <summary>
    /// Creates a client with the default API base URL and no API key.
    /// </summary>
    /// <remarks>
    /// The public example DNS record group does not require a valid API key.
    /// Account-wide calls require an API key; use the constructor overload that accepts a key.
    /// </remarks>
    public DnsCheckClient()
        : this(CreateOwnedHttpClient(baseAddress: null), ownsHttpClient: true, apiKey: null, baseAddress: null)
    {
    }

    /// <summary>
    /// Creates a client with the default API base URL.
    /// </summary>
    /// <param name="apiKey">DNS Check API key. Treat as a secret; the SDK never logs it.</param>
    public DnsCheckClient(string apiKey)
        : this(CreateOwnedHttpClient(baseAddress: null), ownsHttpClient: true, apiKey, baseAddress: null)
    {
    }

    /// <summary>
    /// Creates a client with a custom API base URL.
    /// </summary>
    /// <param name="apiKey">DNS Check API key. Treat as a secret; the SDK never logs it.</param>
    /// <param name="baseAddress">API base URL. A trailing slash is applied when missing.</param>
    public DnsCheckClient(string apiKey, Uri baseAddress)
        : this(CreateOwnedHttpClient(baseAddress), ownsHttpClient: true, apiKey, baseAddress)
    {
    }

    /// <summary>
    /// Creates a client that uses the supplied <see cref="HttpClient"/>.
    /// </summary>
    /// <param name="httpClient">HTTP client instance. The SDK does not mutate <see cref="HttpClient.DefaultRequestHeaders"/>.</param>
    /// <param name="apiKey">Optional DNS Check API key.</param>
    public DnsCheckClient(HttpClient httpClient, string? apiKey = null)
        : this(httpClient, ownsHttpClient: false, apiKey, baseAddress: null)
    {
    }

    private DnsCheckClient(HttpClient httpClient, bool ownsHttpClient, string? apiKey, Uri? baseAddress)
    {
        ArgumentNullException.ThrowIfNull(httpClient);

        _httpClient = httpClient;
        _ownsHttpClient = ownsHttpClient;

        Uri resolvedBase = ResolveBaseAddress(httpClient, baseAddress);

        if (_ownsHttpClient && httpClient.BaseAddress is null)
        {
            httpClient.BaseAddress = resolvedBase;
        }

        RestClient rest = new(httpClient, resolvedBase, apiKey);
        Groups = new GroupService(rest);
        DnsRecords = new DnsRecordService(rest);
    }

    /// <summary>
    /// DNS record group monitoring.
    /// </summary>
    public IGroupService Groups { get; }

    /// <summary>
    /// DNS record monitoring within groups.
    /// </summary>
    public IDnsRecordService DnsRecords { get; }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_ownsHttpClient)
        {
            _httpClient.Dispose();
        }
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

    private static HttpClient CreateOwnedHttpClient(Uri? baseAddress)
    {
        HttpClient client = new();
        if (baseAddress is not null)
        {
            client.BaseAddress = HttpClientConfiguration.NormalizeBaseAddress(baseAddress);
        }

        return client;
    }
}
