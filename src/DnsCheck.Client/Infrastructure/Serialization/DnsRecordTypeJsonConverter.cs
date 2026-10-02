using System.Text.Json;
using System.Text.Json.Serialization;
using DnsCheck.Client.Models;

namespace DnsCheck.Client.Infrastructure.Serialization;

/// <summary>
/// Maps DNS Check uppercase record type wire values. Unknown values fail deserialisation with <see cref="JsonException"/>.
/// </summary>
internal sealed class DnsRecordTypeJsonConverter : JsonConverter<DnsRecordType>
{
    public override DnsRecordType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        return value switch
        {
            "A" => DnsRecordType.A,
            "AAAA" => DnsRecordType.AAAA,
            "ALIAS" => DnsRecordType.ALIAS,
            "CAA" => DnsRecordType.CAA,
            "CNAME" => DnsRecordType.CNAME,
            "HTTPS" => DnsRecordType.HTTPS,
            "MX" => DnsRecordType.MX,
            "NS" => DnsRecordType.NS,
            "PTR" => DnsRecordType.PTR,
            "SOA" => DnsRecordType.SOA,
            "SPF" => DnsRecordType.SPF,
            "SRV" => DnsRecordType.SRV,
            "SVCB" => DnsRecordType.SVCB,
            "TXT" => DnsRecordType.TXT,
            _ => throw new JsonException($"Unknown DNS record type value '{value}'."),
        };
    }

    public override void Write(Utf8JsonWriter writer, DnsRecordType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(
            value switch
            {
                DnsRecordType.A => "A",
                DnsRecordType.AAAA => "AAAA",
                DnsRecordType.ALIAS => "ALIAS",
                DnsRecordType.CAA => "CAA",
                DnsRecordType.CNAME => "CNAME",
                DnsRecordType.HTTPS => "HTTPS",
                DnsRecordType.MX => "MX",
                DnsRecordType.NS => "NS",
                DnsRecordType.PTR => "PTR",
                DnsRecordType.SOA => "SOA",
                DnsRecordType.SPF => "SPF",
                DnsRecordType.SRV => "SRV",
                DnsRecordType.SVCB => "SVCB",
                DnsRecordType.TXT => "TXT",
                _ => throw new JsonException($"Unknown DNS record type value '{value}'."),
            });
    }
}
