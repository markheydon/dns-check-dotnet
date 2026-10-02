using DnsCheck.Client;
using DnsCheck.Client.Infrastructure;

namespace DnsCheck.Client.Tests.Services;

public sealed class MonitoringServiceTests
{
    private const string ExampleGroupUuid = "ea883d67-d9f6-45e3-b3a1-844dd1857824";

    [Fact]
    public async Task Groups_GetAsync_BeforeImplementation_ReturnsFaultedTask()
    {
        using DnsCheckClient client = new();

        DnsCheckMonitoringNotImplementedException exception = await Assert.ThrowsAsync<DnsCheckMonitoringNotImplementedException>(
            () => client.Groups.GetAsync(ExampleGroupUuid, TestContext.Current.CancellationToken));

        Assert.Equal(ServiceAvailability.MonitoringNotImplementedMessage, exception.Message);
    }

    [Fact]
    public async Task Groups_ListAllAsync_BeforeImplementation_ReturnsFaultedTask()
    {
        using DnsCheckClient client = new("test-api-key");

        DnsCheckMonitoringNotImplementedException exception = await Assert.ThrowsAsync<DnsCheckMonitoringNotImplementedException>(
            () => client.Groups.ListAllAsync(TestContext.Current.CancellationToken));

        Assert.Equal(ServiceAvailability.MonitoringNotImplementedMessage, exception.Message);
    }

    [Fact]
    public async Task Groups_ListAllAsync_WithoutApiKey_ThrowsDnsCheckRequestException()
    {
        using DnsCheckClient client = new();

        await Assert.ThrowsAsync<DnsCheckRequestException>(
            () => client.Groups.ListAllAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task DnsRecords_GetAsync_BeforeImplementation_ReturnsFaultedTask()
    {
        using DnsCheckClient client = new();

        DnsCheckMonitoringNotImplementedException exception = await Assert.ThrowsAsync<DnsCheckMonitoringNotImplementedException>(
            () => client.DnsRecords.GetAsync(ExampleGroupUuid, recordId: 1, TestContext.Current.CancellationToken));

        Assert.Equal(ServiceAvailability.MonitoringNotImplementedMessage, exception.Message);
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
    public async Task DnsRecords_ListInGroupAsync_BeforeImplementation_ReturnsFaultedTask()
    {
        using DnsCheckClient client = new();

        DnsCheckMonitoringNotImplementedException exception = await Assert.ThrowsAsync<DnsCheckMonitoringNotImplementedException>(
            () => client.DnsRecords.ListInGroupAsync(ExampleGroupUuid, TestContext.Current.CancellationToken));

        Assert.Equal(ServiceAvailability.MonitoringNotImplementedMessage, exception.Message);
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
}
