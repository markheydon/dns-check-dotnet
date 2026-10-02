using DnsCheck.Client;
using DnsCheck.Client.Tests.TestSupport;

namespace DnsCheck.Client.Tests;

public sealed class DnsCheckClientTests
{
    [Fact]
    public void Constructor_WithHttpClient_CreatesInstance()
    {
        using HttpClient httpClient = new()
        {
            BaseAddress = new Uri("https://api.example.test/v1/"),
        };

        DnsCheckClient client = new(httpClient);

        Assert.NotNull(client.Groups);
        Assert.NotNull(client.DnsRecords);
    }

    [Fact]
    public void Constructor_WithNullHttpClient_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new DnsCheckClient(null!));
    }

    [Fact]
    public void Constructor_WithHttpClientWithoutBaseAddress_DoesNotMutateSuppliedClient()
    {
        using HttpClient httpClient = new();
        DnsCheckClient client = new(httpClient);

        Assert.Null(httpClient.BaseAddress);
    }

    [Fact]
    public void Constructor_WithHttpClientAndApiKey_ResolvesDefaultBaseForRestClient()
    {
        using HttpClient httpClient = new();
        DnsCheckClient client = new(httpClient, "test-api-key");

        Uri uri = client.TestRestClient.BuildRequestUri("groups/all");

        Assert.Equal(
            "https://www.dnscheck.co/api/v1/groups/all?api_key=test-api-key",
            uri.AbsoluteUri);
    }

    [Fact]
    public void Constructor_WithHttpClientBaseAddress_BuildRequestUriUsesCustomHost()
    {
        using HttpClient httpClient = new()
        {
            BaseAddress = new Uri("https://api.example.test/v1/"),
        };

        DnsCheckClient client = new(httpClient, "test-api-key");

        Uri uri = client.TestRestClient.BuildRequestUri("groups/all");

        Assert.Equal("https://api.example.test/v1/groups/all?api_key=test-api-key", uri.AbsoluteUri);
    }

    [Fact]
    public void Constructor_WithHttpClientAndApiKey_PassesKeyToRestClient()
    {
        using HttpClient httpClient = new();
        DnsCheckClient client = new(httpClient, "test-api-key");

        Assert.Equal("test-api-key", DnsCheckClientTestAccess.GetApiKey(client));
    }

    [Fact]
    public void Constructor_WithHttpClient_WhenApiKeyWhitespace_ThrowsArgumentException()
    {
        using HttpClient httpClient = new();

        ArgumentException exception = Assert.Throws<ArgumentException>(() => new DnsCheckClient(httpClient, "   "));

        Assert.Equal("apiKey", exception.ParamName);
    }

}
