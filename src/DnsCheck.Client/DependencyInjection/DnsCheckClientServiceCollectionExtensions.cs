using DnsCheck.Client.Infrastructure.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DnsCheck.Client.DependencyInjection;

/// <summary>
/// Registers <see cref="DnsCheckClient"/> with the HTTP client factory (<c>AddHttpClient</c>).
/// </summary>
public static class DnsCheckClientServiceCollectionExtensions
{
    /// <summary>
    /// Adds a typed <see cref="DnsCheckClient"/> and configures its <see cref="HttpClient"/> base address from options.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Optional callback to configure <see cref="DnsCheckClientOptions"/>.</param>
    /// <returns>The <see cref="IHttpClientBuilder"/> for further HTTP configuration (for example resilience handlers).</returns>
    public static IHttpClientBuilder AddDnsCheckClient(
        this IServiceCollection services,
        Action<DnsCheckClientOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        DnsCheckClientOptions options = new();
        configure?.Invoke(options);
        ValidateOptionalApiKey(options.ApiKey);

        services.TryAddSingleton(options);

        return services
            .AddHttpClient<DnsCheckClient>()
            .ConfigureHttpClient((_, httpClient) =>
            {
                Uri baseAddress = options.BaseAddress is not null
                    ? HttpClientConfiguration.NormalizeBaseAddress(options.BaseAddress)
                    : HttpClientConfiguration.NormalizeBaseAddress(new Uri(DnsCheckClient.DefaultBaseUrl));

                httpClient.BaseAddress = baseAddress;
            })
            .AddTypedClient<DnsCheckClient>((httpClient, serviceProvider) =>
            {
                DnsCheckClientOptions resolvedOptions = serviceProvider.GetRequiredService<DnsCheckClientOptions>();
                return new DnsCheckClient(httpClient, resolvedOptions.ApiKey);
            });
    }

    private static void ValidateOptionalApiKey(string? apiKey)
    {
        if (apiKey is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new ArgumentException("API key cannot be empty or whitespace.");
        }
    }
}
