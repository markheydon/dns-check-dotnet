using System.Text.Json;

namespace DnsCheck.Client.Infrastructure.Serialization;

internal static class DnsCheckJsonSerializerOptions
{
    internal static JsonSerializerOptions Default { get; } = CreateDefault();

    private static JsonSerializerOptions CreateDefault()
    {
        return new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }
}
