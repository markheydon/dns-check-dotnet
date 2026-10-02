using DnsCheck.Client;
using DnsCheck.Client.Infrastructure;

namespace DnsCheck.Client.Tests.Infrastructure;

public sealed class ApiPathValidationTests
{
    private const string ValidGroupUuid = "ea883d67-d9f6-45e3-b3a1-844dd1857824";

    [Fact]
    public void ValidateGroupUuid_ValidGuid_DoesNotThrow()
    {
        ApiPathValidation.ValidateGroupUuid(ValidGroupUuid);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateGroupUuid_WhenEmptyOrWhitespace_ThrowsDnsCheckRequestException(string groupUuid)
    {
        DnsCheckRequestException exception = Assert.Throws<DnsCheckRequestException>(
            () => ApiPathValidation.ValidateGroupUuid(groupUuid));

        Assert.Contains("required", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("not-a-guid")]
    [InlineData("all")]
    public void ValidateGroupUuid_WhenNotGuid_ThrowsDnsCheckRequestException(string groupUuid)
    {
        Assert.Throws<DnsCheckRequestException>(() => ApiPathValidation.ValidateGroupUuid(groupUuid));
    }

    [Theory]
    [InlineData("ea883d67-d9f6-45e3-b3a1-844dd1857824?x=1")]
    [InlineData("ea883d67-d9f6-45e3-b3a1-844dd1857824#frag")]
    [InlineData("ea883d67-d9f6-45e3-b3a1-844dd1857824/extra")]
    [InlineData("ea883d67-d9f6-45e3-b3a1-844dd1857824\\extra")]
    public void ValidateGroupUuid_WhenDelimiterPresent_ThrowsDnsCheckRequestException(string groupUuid)
    {
        Assert.Throws<DnsCheckRequestException>(() => ApiPathValidation.ValidateGroupUuid(groupUuid));
    }

    [Fact]
    public async Task DnsRecords_ListInGroupAsync_WhenGroupUuidWhitespace_ThrowsDnsCheckRequestException()
    {
        using DnsCheckClient client = new();

        await Assert.ThrowsAsync<DnsCheckRequestException>(
            () => client.DnsRecords.ListInGroupAsync("   ", TestContext.Current.CancellationToken));
    }
}
