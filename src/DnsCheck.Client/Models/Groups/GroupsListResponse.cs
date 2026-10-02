using System.Text.Json.Serialization;

namespace DnsCheck.Client.Models.Groups;

internal sealed class GroupsListResponse
{
    [JsonPropertyName("groups")]
    public List<DnsRecordGroup>? Groups { get; init; }
}
