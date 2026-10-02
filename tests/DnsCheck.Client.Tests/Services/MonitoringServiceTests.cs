using DnsCheck.Client;
using DnsCheck.Client.Infrastructure;

namespace DnsCheck.Client.Tests.Services;

public sealed class MonitoringServiceTests
{
    [Fact]
    public async Task Groups_GetAsync_BeforeImplementation_ReturnsFaultedTask()
    {
        using DnsCheckClient client = new();

        InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => client.Groups.GetAsync("ea883d67-d9f6-45e3-b3a1-844dd1857824", TestContext.Current.CancellationToken));

        Assert.Equal(ServiceAvailability.MonitoringNotImplementedMessage, exception.Message);
    }

    [Fact]
    public async Task DnsRecords_ListInGroupAsync_BeforeImplementation_ReturnsFaultedTask()
    {
        using DnsCheckClient client = new();

        InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => client.DnsRecords.ListInGroupAsync("ea883d67-d9f6-45e3-b3a1-844dd1857824", TestContext.Current.CancellationToken));

        Assert.Equal(ServiceAvailability.MonitoringNotImplementedMessage, exception.Message);
    }
}
