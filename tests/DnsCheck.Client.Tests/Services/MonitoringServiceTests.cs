using System.Net;
using DnsCheck.Client;
using DnsCheck.Client.Models;
using DnsCheck.Client.Models.DnsRecords;
using DnsCheck.Client.Models.Groups;
using DnsCheck.Client.Tests.TestSupport;

namespace DnsCheck.Client.Tests.Services;

public sealed class MonitoringServiceTests
{
    private const string ExampleGroupUuid = "ea883d67-d9f6-45e3-b3a1-844dd1857824";

    [Fact]
    public async Task Groups_GetAsync_ReturnsGroup()
    {
        QueuedHttpMessageHandler handler = new();
        handler.Enqueue(HttpStatusCode.OK, FixtureFiles.Read("group-get.json"));

        using DnsCheckClient client = CreateClient(handler);

        DnsRecordGroup group = await client.Groups.GetAsync(ExampleGroupUuid, TestContext.Current.CancellationToken);

        Assert.Equal(ExampleGroupUuid, group.Uuid);
        Assert.Equal("Example DNS Check", group.Name);
        Assert.Equal(DnsCheckStatus.Fail, group.Status);
        Assert.True(group.IsPublic);

        HttpRequestMessage request = Assert.Single(handler.SentRequests);
        Assert.Contains($"/groups/{ExampleGroupUuid}", request.RequestUri!.AbsolutePath, StringComparison.Ordinal);
        Assert.True(string.IsNullOrEmpty(request.RequestUri!.Query));
    }

    [Fact]
    public async Task Groups_ListAllAsync_ReturnsGroups()
    {
        QueuedHttpMessageHandler handler = new();
        handler.Enqueue(HttpStatusCode.OK, FixtureFiles.Read("groups-all.json"));

        using DnsCheckClient client = CreateClient(handler, apiKey: "test-api-key");

        IReadOnlyList<DnsRecordGroup> groups = await client.Groups.ListAllAsync(TestContext.Current.CancellationToken);

        DnsRecordGroup group = Assert.Single(groups);
        Assert.Equal(ExampleGroupUuid, group.Uuid);

        HttpRequestMessage request = Assert.Single(handler.SentRequests);
        Assert.Contains("/groups/all", request.RequestUri!.AbsolutePath, StringComparison.Ordinal);
        Assert.Contains("api_key=test-api-key", request.RequestUri!.Query, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Groups_ListAllAsync_WithoutApiKey_ThrowsDnsCheckRequestException()
    {
        using DnsCheckClient client = new();

        await Assert.ThrowsAsync<DnsCheckRequestException>(
            () => client.Groups.ListAllAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task DnsRecords_GetAsync_ReturnsRecord()
    {
        QueuedHttpMessageHandler handler = new();
        handler.Enqueue(HttpStatusCode.OK, FixtureFiles.Read("dns-record-get.json"));

        using DnsCheckClient client = CreateClient(handler);

        DnsRecord record = await client.DnsRecords.GetAsync(ExampleGroupUuid, 5530, TestContext.Current.CancellationToken);

        Assert.Equal(5530, record.Id);
        Assert.Equal(DnsRecordType.ALIAS, record.RecordType);
        Assert.Equal(DnsCheckStatus.Pass, record.Status);

        HttpRequestMessage request = Assert.Single(handler.SentRequests);
        Assert.Contains($"/groups/{ExampleGroupUuid}/5530", request.RequestUri!.AbsolutePath, StringComparison.Ordinal);
    }

    [Fact]
    public async Task DnsRecords_ListInGroupAsync_ReturnsRecords()
    {
        QueuedHttpMessageHandler handler = new();
        handler.Enqueue(HttpStatusCode.OK, FixtureFiles.Read("dns-records-list.json"));

        using DnsCheckClient client = CreateClient(handler);

        IReadOnlyList<DnsRecord> records = await client.DnsRecords.ListInGroupAsync(
            ExampleGroupUuid,
            TestContext.Current.CancellationToken);

        Assert.Equal(2, records.Count);
        Assert.Contains(records, r => r.RecordType == DnsRecordType.ALIAS);
        Assert.Contains(records, r => r.RecordType == DnsRecordType.NS);

        HttpRequestMessage request = Assert.Single(handler.SentRequests);
        Assert.Contains($"/groups/{ExampleGroupUuid}/all", request.RequestUri!.AbsolutePath, StringComparison.Ordinal);
    }

    [Fact]
    public async Task DnsRecords_ListAllAsync_ReturnsRecords()
    {
        QueuedHttpMessageHandler handler = new();
        handler.Enqueue(HttpStatusCode.OK, FixtureFiles.Read("groups-all.json"));
        handler.Enqueue(HttpStatusCode.OK, FixtureFiles.Read("dns-records-list.json"));

        using DnsCheckClient client = CreateClient(handler, apiKey: "test-api-key");

        IReadOnlyList<DnsRecord> records = await client.DnsRecords.ListAllAsync(TestContext.Current.CancellationToken);

        Assert.Equal(2, records.Count);

        Assert.Equal(2, handler.SentRequests.Count);
        Assert.Contains("/groups/all", handler.SentRequests[0].RequestUri!.AbsolutePath, StringComparison.Ordinal);
        Assert.Contains(
            $"/groups/{ExampleGroupUuid}/all",
            handler.SentRequests[1].RequestUri!.AbsolutePath,
            StringComparison.Ordinal);
        Assert.All(handler.SentRequests, r => Assert.Contains("api_key=test-api-key", r.RequestUri!.Query, StringComparison.Ordinal));
    }

    [Fact]
    public async Task DnsRecords_ListAllAsync_WithoutApiKey_ThrowsDnsCheckRequestException()
    {
        using DnsCheckClient client = new();

        await Assert.ThrowsAsync<DnsCheckRequestException>(
            () => client.DnsRecords.ListAllAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Groups_GetAsync_WhenUnauthorized_ThrowsDnsCheckApiException()
    {
        QueuedHttpMessageHandler handler = new();
        handler.Enqueue(HttpStatusCode.Unauthorized, "\"Unauthorized\"");

        using DnsCheckClient client = CreateClient(handler);

        DnsCheckApiException exception = await Assert.ThrowsAsync<DnsCheckApiException>(
            () => client.Groups.GetAsync(ExampleGroupUuid, TestContext.Current.CancellationToken));

        Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
        Assert.Equal("Unauthorized", exception.Detail);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task DnsRecords_GetAsync_WhenRecordIdNotPositive_ThrowsDnsCheckRequestException(int recordId)
    {
        using DnsCheckClient client = new();

        await Assert.ThrowsAsync<DnsCheckRequestException>(
            () => client.DnsRecords.GetAsync(ExampleGroupUuid, recordId, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task DnsRecords_GetAsync_WhenCancelled_ThrowsOperationCanceledException()
    {
        using DnsCheckClient client = new();
        using CancellationTokenSource cts = new();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(
            () => client.DnsRecords.GetAsync(ExampleGroupUuid, recordId: 1, cts.Token));
    }

    [Fact]
    public async Task DnsRecords_ListInGroupAsync_WhenCancelled_ThrowsOperationCanceledException()
    {
        using DnsCheckClient client = new();
        using CancellationTokenSource cts = new();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(
            () => client.DnsRecords.ListInGroupAsync(ExampleGroupUuid, cts.Token));
    }

    [Fact]
    public async Task Groups_GetAsync_WhenCancelled_ThrowsOperationCanceledException()
    {
        using DnsCheckClient client = new();
        using CancellationTokenSource cts = new();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(
            () => client.Groups.GetAsync(ExampleGroupUuid, cts.Token));
    }

    [Fact]
    public async Task Groups_ListAllAsync_WhenCancelled_ThrowsOperationCanceledException()
    {
        using DnsCheckClient client = new("test-api-key");
        using CancellationTokenSource cts = new();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(
            () => client.Groups.ListAllAsync(cts.Token));
    }

    [Fact]
    public async Task Groups_ListAllAsync_WhenCancelledWithoutApiKey_ThrowsOperationCanceledException()
    {
        using DnsCheckClient client = new();
        using CancellationTokenSource cts = new();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(
            () => client.Groups.ListAllAsync(cts.Token));
    }

    [Theory]
    [InlineData("all")]
    [InlineData("ALL")]
    public async Task Groups_GetAsync_WhenAll_ThrowsDnsCheckRequestException(string groupUuid)
    {
        using DnsCheckClient client = new();

        DnsCheckRequestException exception = await Assert.ThrowsAsync<DnsCheckRequestException>(
            () => client.Groups.GetAsync(groupUuid, TestContext.Current.CancellationToken));

        Assert.Contains("ListAllAsync", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task DnsRecords_ListInGroupAsync_WhenAll_ThrowsDnsCheckRequestException()
    {
        using DnsCheckClient client = new();

        DnsCheckRequestException exception = await Assert.ThrowsAsync<DnsCheckRequestException>(
            () => client.DnsRecords.ListInGroupAsync("all", TestContext.Current.CancellationToken));

        Assert.Contains("ListAllAsync", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Groups_GetAsync_InvalidGroupUuid_ThrowsDnsCheckRequestException()
    {
        using DnsCheckClient client = new();

        await Assert.ThrowsAsync<DnsCheckRequestException>(
            () => client.Groups.GetAsync("all?api_key=other", TestContext.Current.CancellationToken));
    }

    private static DnsCheckClient CreateClient(QueuedHttpMessageHandler handler, string? apiKey = null)
    {
        HttpClient httpClient = new(handler);
        return apiKey is null
            ? new DnsCheckClient(httpClient)
            : new DnsCheckClient(httpClient, apiKey);
    }
}
