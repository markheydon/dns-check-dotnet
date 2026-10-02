using DnsCheck.Client;
using DnsCheck.Client.Services.Groups;
using DnsCheck.Client.Tests.TestSupport;

namespace DnsCheck.Client.Tests;

public sealed class DnsCheckClientTests
{
    [Fact]
    public void Constructor_Default_CreatesInstance()
    {
        using DnsCheckClient client = new();

        Assert.NotNull(client.Groups);
        Assert.NotNull(client.DnsRecords);
    }

    [Fact]
    public void Constructor_WithApiKey_CreatesInstance()
    {
        using DnsCheckClient client = new("test-api-key");

        Assert.NotNull(client.Groups);
        Assert.NotNull(client.DnsRecords);
    }

    [Fact]
    public void Constructor_WithNullHttpClient_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new DnsCheckClient(null!));
    }

    [Fact]
    public void Constructor_WithHttpClient_UsesSuppliedClient()
    {
        using HttpClient httpClient = new()
        {
            BaseAddress = new Uri("https://api.example.test/v1/"),
        };

        using DnsCheckClient client = new(httpClient);

        Assert.NotNull(client);
    }

    [Fact]
    public void Constructor_WithHttpClientWithoutBaseAddress_DoesNotMutateSuppliedClient()
    {
        using HttpClient httpClient = new();
        using DnsCheckClient client = new(httpClient);

        Assert.Null(httpClient.BaseAddress);
    }

    [Fact]
    public void Constructor_Default_SetsOwnedHttpClientBaseAddress()
    {
        using DnsCheckClient client = new();

        Uri? baseAddress = DnsCheckClientTestAccess.GetHttpClient(client).BaseAddress;

        Assert.NotNull(baseAddress);
        Assert.Equal(DnsCheckClient.DefaultBaseUrl, baseAddress!.AbsoluteUri);
    }

    [Fact]
    public void Constructor_WithCustomBaseAddress_NormalizesTrailingSlashOnOwnedClient()
    {
        using DnsCheckClient client = new("test-api-key", new Uri("https://api.example.test/v1"));

        Uri? baseAddress = DnsCheckClientTestAccess.GetHttpClient(client).BaseAddress;

        Assert.NotNull(baseAddress);
        Assert.Equal("https://api.example.test/v1/", baseAddress!.AbsoluteUri);
    }

    [Fact]
    public void Constructor_WithCustomBaseAddress_BuildRequestUriUsesCustomHost()
    {
        using DnsCheckClient client = new("test-api-key", new Uri("https://api.example.test/v1/"));

        GroupService groups = (GroupService)client.Groups;
        Uri uri = groups.RestClient.BuildRequestUri("groups/all");

        Assert.Equal("https://api.example.test/v1/groups/all?api_key=test-api-key", uri.AbsoluteUri);
    }

    [Fact]
    public void Constructor_WithApiKey_PassesKeyToRestClient()
    {
        using DnsCheckClient client = new("test-api-key");

        Assert.Equal("test-api-key", DnsCheckClientTestAccess.GetApiKey(client));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithRequiredApiKey_WhenEmptyOrWhitespace_ThrowsArgumentException(string apiKey)
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(() => new DnsCheckClient(apiKey));

        Assert.Equal("apiKey", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithHttpClient_WhenApiKeyWhitespace_ThrowsArgumentException()
    {
        using HttpClient httpClient = new();

        ArgumentException exception = Assert.Throws<ArgumentException>(() => new DnsCheckClient(httpClient, "   "));

        Assert.Equal("apiKey", exception.ParamName);
    }
}
