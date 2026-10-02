using System.Text.Json;
using System.Text.Json.Serialization;
using DnsCheck.Client.Models;

namespace DnsCheck.Client.Infrastructure.Serialization;

internal sealed class DnsCheckStatusJsonConverter : JsonConverter<DnsCheckStatus>
{
    public override DnsCheckStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        return value switch
        {
            "pass" => DnsCheckStatus.Pass,
            "fail" => DnsCheckStatus.Fail,
            "unknown" => DnsCheckStatus.Unknown,
            _ => throw new JsonException($"Unknown DNS Check status value '{value}'."),
        };
    }

    public override void Write(Utf8JsonWriter writer, DnsCheckStatus value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(
            value switch
            {
                DnsCheckStatus.Pass => "pass",
                DnsCheckStatus.Fail => "fail",
                DnsCheckStatus.Unknown => "unknown",
                _ => throw new JsonException($"Unknown DNS Check status value '{value}'."),
            });
    }
}
