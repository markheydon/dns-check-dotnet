using System.Text.Json.Serialization;

namespace DnsCheck.Client.Models.Groups;

internal sealed record GroupResponse
{
    [JsonPropertyName("group")]
    public DnsRecordGroup? Group { get; init; }
}
