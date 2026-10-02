using System.Text.Json.Serialization;

namespace DnsCheck.Client.Models.DnsRecords;

internal sealed class DnsRecordResponse
{
    [JsonPropertyName("dns_record")]
    public DnsRecord? DnsRecord { get; init; }
}
