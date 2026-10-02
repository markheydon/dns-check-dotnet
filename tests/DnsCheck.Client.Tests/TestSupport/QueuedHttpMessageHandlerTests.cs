using System.Net;
using DnsCheck.Client.Tests.TestSupport;

namespace DnsCheck.Client.Tests.TestSupport;

public sealed class QueuedHttpMessageHandlerTests
{
    [Fact]
    public async Task SendAsync_DequeuesConfiguredResponse()
    {
        QueuedHttpMessageHandler handler = new();
        handler.Enqueue(HttpStatusCode.OK, "{\"ok\":true}");

        using HttpClient httpClient = new(handler);
        HttpResponseMessage response = await httpClient.GetAsync(
            "https://example.test/v1/groups/all",
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Single(handler.SentRequests);
    }
}
