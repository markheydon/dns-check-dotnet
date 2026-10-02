using System.Net;

namespace DnsCheck.Client.Tests.TestSupport;

internal sealed class QueuedHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<Func<HttpRequestMessage, HttpResponseMessage>> _responses = new();
    private readonly List<HttpRequestMessage> _sentRequests = [];

    internal IReadOnlyList<HttpRequestMessage> SentRequests => _sentRequests;

    /// <summary>Invoked after each request is recorded, before the queued response is returned.</summary>
    internal Action<HttpRequestMessage>? AfterRequestSent { get; set; }

    internal void Enqueue(HttpStatusCode statusCode, string content, string mediaType = "application/json")
    {
        Enqueue(statusCode, content, configureResponse: null, mediaType);
    }

    internal void Enqueue(
        HttpStatusCode statusCode,
        string content,
        Action<HttpResponseMessage>? configureResponse,
        string mediaType = "application/json")
    {
        _responses.Enqueue(_ =>
        {
            HttpResponseMessage response = new(statusCode)
            {
                Content = new StringContent(content, System.Text.Encoding.UTF8, mediaType),
            };

            configureResponse?.Invoke(response);
            return response;
        });
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<HttpResponseMessage>(cancellationToken);
        }

        if (_responses.Count == 0)
        {
            throw new InvalidOperationException($"No queued response for {request.Method} {request.RequestUri}.");
        }

        _sentRequests.Add(request);
        AfterRequestSent?.Invoke(request);

        Func<HttpRequestMessage, HttpResponseMessage> responseFactory = _responses.Dequeue();
        return Task.FromResult(responseFactory(request));
    }
}
