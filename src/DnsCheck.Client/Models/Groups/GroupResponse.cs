using System.Text.Json.Serialization;

namespace DnsCheck.Client.Models.Groups;

internal sealed class GroupResponse
{
    [JsonPropertyName("group")]
    public DnsRecordGroup? Group { get; init; }
}
