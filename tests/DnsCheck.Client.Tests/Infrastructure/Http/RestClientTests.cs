using System.Net;
using DnsCheck.Client;
using DnsCheck.Client.Infrastructure.Http;
using DnsCheck.Client.Models.Groups;
using DnsCheck.Client.Tests.TestSupport;

namespace DnsCheck.Client.Tests.Infrastructure.Http;

public sealed class RestClientTests
{
    [Fact]
    public void BuildRequestUri_WithApiKey_AppendsQueryParameter()
    {
        using HttpClient httpClient = new();
        RestClient rest = new(httpClient, new Uri(DnsCheckClient.DefaultBaseUrl), "test-key");

        Uri uri = rest.BuildRequestUri("groups/all");

        Assert.Equal(
            "https://www.dnscheck.co/api/v1/groups/all?api_key=test-key",
            uri.AbsoluteUri);
    }

    [Fact]
    public void BuildRequestUri_WithoutApiKey_OmitsQueryParameter()
    {
        using HttpClient httpClient = new();
        RestClient rest = new(httpClient, new Uri(DnsCheckClient.DefaultBaseUrl), apiKey: null);

        Uri uri = rest.BuildRequestUri("groups/ea883d67-d9f6-45e3-b3a1-844dd1857824");

        Assert.Equal(
            "https://www.dnscheck.co/api/v1/groups/ea883d67-d9f6-45e3-b3a1-844dd1857824",
            uri.AbsoluteUri);
    }

    [Theory]
    [InlineData("../groups/all")]
    [InlineData("/groups/all")]
    [InlineData("https://evil.example/groups/all")]
    [InlineData("groups/foo?extra=1")]
    [InlineData("groups/%2e%2e/admin")]
    public void BuildRequestUri_WhenRelativePathUnsafe_ThrowsDnsCheckRequestException(string relativePath)
    {
        using HttpClient httpClient = new();
        RestClient rest = new(httpClient, new Uri(DnsCheckClient.DefaultBaseUrl), apiKey: null);

        Assert.Throws<DnsCheckRequestException>(() => rest.BuildRequestUri(relativePath));
    }

    [Fact]
    public void BuildRequestUri_WhenQueryIncludesApiKey_ThrowsDnsCheckRequestException()
    {
        using HttpClient httpClient = new();
        RestClient rest = new(httpClient, new Uri(DnsCheckClient.DefaultBaseUrl), "test-key");

        DnsCheckRequestException exception = Assert.Throws<DnsCheckRequestException>(
            () => rest.BuildRequestUri(
                "groups/all",
                [new RestQuery.QueryParameter("api_key", "other")]));

        Assert.Contains("api_key", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Constructor_WhitespaceApiKey_ThrowsArgumentException()
    {
        using HttpClient httpClient = new();

        ArgumentException exception = Assert.Throws<ArgumentException>(
            () => new RestClient(httpClient, new Uri(DnsCheckClient.DefaultBaseUrl), "   "));

        Assert.Equal("apiKey", exception.ParamName);
    }

    [Fact]
    public async Task BuildRequestUri_WithInjectedHttpClientWithoutBaseAddress_SendsAbsoluteRequestUri()
    {
        QueuedHttpMessageHandler handler = new();
        handler.Enqueue(HttpStatusCode.OK, "\"OK\"");

        using HttpClient httpClient = new(handler);
        RestClient rest = new(httpClient, new Uri(DnsCheckClient.DefaultBaseUrl), apiKey: null);

        Uri requestUri = rest.BuildRequestUri("groups/all");
        await httpClient.GetAsync(requestUri, TestContext.Current.CancellationToken);

        HttpRequestMessage sent = Assert.Single(handler.SentRequests);
        Assert.Equal(requestUri, sent.RequestUri);
        Assert.Equal("https://www.dnscheck.co/api/v1/groups/all", sent.RequestUri!.AbsoluteUri);
    }

    [Fact]
    public async Task GetAsync_WhenSuccess_DeserialisesResponse()
    {
        QueuedHttpMessageHandler handler = new();
        handler.Enqueue(HttpStatusCode.OK, FixtureFiles.Read("group-get.json"));

        using HttpClient httpClient = new(handler);
        RestClient rest = new(httpClient, new Uri(DnsCheckClient.DefaultBaseUrl), apiKey: null);

        GroupResponse response = await rest.GetAsync<GroupResponse>(
            $"groups/ea883d67-d9f6-45e3-b3a1-844dd1857824",
            TestContext.Current.CancellationToken);

        Assert.NotNull(response.Group);
        Assert.Equal("Example DNS Check", response.Group!.Name);
    }

    [Fact]
    public async Task GetAsync_WhenUnauthorized_ThrowsDnsCheckApiException()
    {
        QueuedHttpMessageHandler handler = new();
        handler.Enqueue(HttpStatusCode.Unauthorized, "\"Unauthorized\"");

        using HttpClient httpClient = new(handler);
        RestClient rest = new(httpClient, new Uri(DnsCheckClient.DefaultBaseUrl), apiKey: null);

        DnsCheckApiException exception = await Assert.ThrowsAsync<DnsCheckApiException>(
            () => rest.GetAsync<GroupResponse>("groups/all", TestContext.Current.CancellationToken));

        Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
        Assert.Equal("Unauthorized", exception.Detail);
    }

    [Fact]
    public async Task GetAsync_WhenBodyInvalidJson_ThrowsDnsCheckParseException()
    {
        QueuedHttpMessageHandler handler = new();
        handler.Enqueue(HttpStatusCode.OK, "not-json");

        using HttpClient httpClient = new(handler);
        RestClient rest = new(httpClient, new Uri(DnsCheckClient.DefaultBaseUrl), apiKey: null);

        await Assert.ThrowsAsync<DnsCheckParseException>(
            () => rest.GetAsync<GroupResponse>(
                "groups/ea883d67-d9f6-45e3-b3a1-844dd1857824",
                TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task GetAsync_WithApiKey_AppendsKeyToRequest()
    {
        QueuedHttpMessageHandler handler = new();
        handler.Enqueue(HttpStatusCode.OK, FixtureFiles.Read("groups-all.json"));

        using HttpClient httpClient = new(handler);
        RestClient rest = new(httpClient, new Uri(DnsCheckClient.DefaultBaseUrl), "secret-key");

        await rest.GetAsync<GroupsListResponse>("groups/all", TestContext.Current.CancellationToken);

        HttpRequestMessage sent = Assert.Single(handler.SentRequests);
        Assert.Contains("api_key=secret-key", sent.RequestUri!.Query, StringComparison.Ordinal);
    }
}
