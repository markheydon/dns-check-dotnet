using System.Net;
using DnsCheck.Client;
using DnsCheck.Client.DependencyInjection;
using DnsCheck.Client.Models.Groups;
using DnsCheck.Client.Tests.TestSupport;
using Microsoft.Extensions.DependencyInjection;

namespace DnsCheck.Client.Tests.DependencyInjection;

public sealed class DnsCheckClientDependencyInjectionTests
{
    [Fact]
    public void AddDnsCheckClient_ResolvesClientWithDefaultBaseAddress()
    {
        ServiceCollection services = new();
        services.AddDnsCheckClient();

        using ServiceProvider provider = services.BuildServiceProvider();
        DnsCheckClient client = provider.GetRequiredService<DnsCheckClient>();

        Assert.NotNull(client.Groups);
        Uri? baseAddress = DnsCheckClientTestAccess.GetHttpClient(client).BaseAddress;
        Assert.NotNull(baseAddress);
        Assert.Equal(DnsCheckClient.DefaultBaseUrl, baseAddress!.AbsoluteUri);
    }

    [Fact]
    public void AddDnsCheckClient_WithApiKey_PassesKeyToRestClient()
    {
        ServiceCollection services = new();
        services.AddDnsCheckClient(options => options.ApiKey = "test-api-key");

        using ServiceProvider provider = services.BuildServiceProvider();
        DnsCheckClient client = provider.GetRequiredService<DnsCheckClient>();

        Assert.Equal("test-api-key", DnsCheckClientTestAccess.GetApiKey(client));
    }

    [Fact]
    public void AddDnsCheckClient_WithCustomBaseAddress_NormalizesTrailingSlash()
    {
        ServiceCollection services = new();
        services.AddDnsCheckClient(options =>
        {
            options.ApiKey = "test-api-key";
            options.BaseAddress = new Uri("https://api.example.test/v1");
        });

        using ServiceProvider provider = services.BuildServiceProvider();
        DnsCheckClient client = provider.GetRequiredService<DnsCheckClient>();

        Uri uri = client.TestRestClient.BuildRequestUri("groups/all");

        Assert.Equal("https://api.example.test/v1/groups/all?api_key=test-api-key", uri.AbsoluteUri);
    }

    [Fact]
    public void AddDnsCheckClient_WhenApiKeyWhitespace_ThrowsArgumentException()
    {
        ServiceCollection services = new();

        ArgumentException exception = Assert.Throws<ArgumentException>(
            () => services.AddDnsCheckClient(options => options.ApiKey = "   "));

        Assert.Contains("API key", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task AddDnsCheckClient_WithConfiguredHandler_SendsHttpThroughFactoryPipeline()
    {
        const string exampleGroupUuid = "ea883d67-d9f6-45e3-b3a1-844dd1857824";
        QueuedHttpMessageHandler handler = new();
        handler.Enqueue(HttpStatusCode.OK, FixtureFiles.Read("group-get.json"));

        ServiceCollection services = new();
        services.AddDnsCheckClient()
            .ConfigurePrimaryHttpMessageHandler(() => handler);

        await using ServiceProvider provider = services.BuildServiceProvider();
        DnsCheckClient client = provider.GetRequiredService<DnsCheckClient>();

        DnsRecordGroup group = await client.Groups.GetAsync(exampleGroupUuid, TestContext.Current.CancellationToken);

        Assert.Equal("Example DNS Check", group.Name);
        HttpRequestMessage request = Assert.Single(handler.SentRequests);
        Assert.Contains(exampleGroupUuid, request.RequestUri!.AbsolutePath, StringComparison.Ordinal);
    }
}
