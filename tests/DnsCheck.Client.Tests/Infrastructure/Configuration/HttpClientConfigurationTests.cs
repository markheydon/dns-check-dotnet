using DnsCheck.Client.Infrastructure.Configuration;

namespace DnsCheck.Client.Tests.Infrastructure.Configuration;

public sealed class HttpClientConfigurationTests
{
    [Fact]
    public void NormalizeBaseAddress_WithoutTrailingSlash_AppendsSlash()
    {
        Uri result = HttpClientConfiguration.NormalizeBaseAddress(new Uri("https://api.example.test/v1"));

        Assert.Equal("https://api.example.test/v1/", result.AbsoluteUri);
    }

    [Fact]
    public void NormalizeBaseAddress_WithTrailingSlash_IsUnchanged()
    {
        Uri result = HttpClientConfiguration.NormalizeBaseAddress(new Uri("https://api.example.test/v1/"));

        Assert.Equal("https://api.example.test/v1/", result.AbsoluteUri);
    }

    [Fact]
    public void NormalizeBaseAddress_RelativeUri_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            HttpClientConfiguration.NormalizeBaseAddress(new Uri("/v1/", UriKind.Relative)));
    }
}
