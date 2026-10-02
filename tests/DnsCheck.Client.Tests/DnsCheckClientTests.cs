using DnsCheck.Client;

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
}
