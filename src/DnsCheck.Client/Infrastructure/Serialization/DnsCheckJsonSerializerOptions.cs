using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace DnsCheck.Client.Infrastructure.Serialization;

internal static class DnsCheckJsonSerializerOptions
{
    internal static JsonSerializerOptions Default { get; } = CreateDefault();

    private static JsonSerializerOptions CreateDefault()
    {
        return new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            TypeInfoResolver = JsonTypeInfoResolver.Combine(
                DnsCheckJsonContext.Default,
                new DefaultJsonTypeInfoResolver()),
        };
    }
}
