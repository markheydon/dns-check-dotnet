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

        InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => client.Groups.GetAsync(ExampleGroupUuid, TestContext.Current.CancellationToken));

        Assert.Equal(ServiceAvailability.MonitoringNotImplementedMessage, exception.Message);
    }

    [Fact]
    public async Task Groups_ListAllAsync_BeforeImplementation_ReturnsFaultedTask()
    {
        using DnsCheckClient client = new();

        InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => client.Groups.ListAllAsync(TestContext.Current.CancellationToken));

        Assert.Equal(ServiceAvailability.MonitoringNotImplementedMessage, exception.Message);
    }

    [Fact]
    public async Task DnsRecords_GetAsync_BeforeImplementation_ReturnsFaultedTask()
    {
        using DnsCheckClient client = new();

        InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => client.DnsRecords.GetAsync(ExampleGroupUuid, recordId: 1, TestContext.Current.CancellationToken));

        Assert.Equal(ServiceAvailability.MonitoringNotImplementedMessage, exception.Message);
    }

    [Fact]
    public async Task DnsRecords_ListInGroupAsync_BeforeImplementation_ReturnsFaultedTask()
    {
        using DnsCheckClient client = new();

        InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => client.DnsRecords.ListInGroupAsync(ExampleGroupUuid, TestContext.Current.CancellationToken));

        Assert.Equal(ServiceAvailability.MonitoringNotImplementedMessage, exception.Message);
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
    public async Task Groups_GetAsync_InvalidGroupUuid_ThrowsArgumentException()
    {
        using DnsCheckClient client = new();

        await Assert.ThrowsAsync<ArgumentException>(
            () => client.Groups.GetAsync("all?api_key=other", TestContext.Current.CancellationToken));
    }
}
