using System.Net;
using DnsCheck.Client;
using DnsCheck.Client.Infrastructure.Http;
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

    [Fact]
    public void BuildRequestUri_WhitespaceApiKey_OmitsQueryParameter()
    {
        using HttpClient httpClient = new();
        RestClient rest = new(httpClient, new Uri(DnsCheckClient.DefaultBaseUrl), "   ");

        Uri uri = rest.BuildRequestUri("groups/all");

        Assert.Equal("https://www.dnscheck.co/api/v1/groups/all", uri.AbsoluteUri);
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
}
