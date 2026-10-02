namespace DnsCheck.Client.Infrastructure.Configuration;

internal static class HttpClientConfiguration
{
    internal static Uri NormalizeBaseAddress(Uri baseAddress)
    {
        ArgumentNullException.ThrowIfNull(baseAddress);

        if (!baseAddress.IsAbsoluteUri)
        {
            throw new ArgumentException("Base address must be an absolute URI.", nameof(baseAddress));
        }

        string absoluteUri = baseAddress.AbsoluteUri;

        if (!absoluteUri.EndsWith('/'))
        {
            absoluteUri += "/";
        }

        return new Uri(absoluteUri, UriKind.Absolute);
    }
}
