using System.Text.Json.Serialization;

namespace DnsCheck.Client.Models.DnsRecords;

internal sealed record DnsRecordResponse
{
    [JsonPropertyName("dns_record")]
    public DnsRecord? DnsRecord { get; init; }
}
