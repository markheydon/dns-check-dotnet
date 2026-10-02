using System.Text.Json.Serialization;

namespace DnsCheck.Client.Models.DnsRecords;

internal sealed record DnsRecordsListResponse
{
    [JsonPropertyName("dns_records")]
    public List<DnsRecord>? DnsRecords { get; init; }
}
