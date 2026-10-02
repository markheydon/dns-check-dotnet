namespace DnsCheck.Client;

/// <summary>
/// Configuration for <see cref="DnsCheckClient"/> when registered via <see cref="DependencyInjection.DnsCheckClientServiceCollectionExtensions.AddDnsCheckClient"/>.
/// </summary>
public sealed class DnsCheckClientOptions
{
    /// <summary>
    /// DNS Check API key. When omitted, only operations that do not require authentication can succeed.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// API base URL. When omitted, <see cref="DnsCheckClient.DefaultBaseUrl"/> is used.
    /// </summary>
    public Uri? BaseAddress { get; set; }
}
