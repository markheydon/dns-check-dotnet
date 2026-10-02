using System.Text.Json.Serialization;
using DnsCheck.Client.Models;

namespace DnsCheck.Client.Models.Groups;

/// <summary>
/// A DNS record group monitored by DNS Check.
/// </summary>
public sealed class DnsRecordGroup
{
    /// <summary>Universally unique identifier for the group.</summary>
    [JsonPropertyName("uuid")]
    public string Uuid { get; init; } = string.Empty;

    /// <summary>Display name of the group.</summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>Description of the group.</summary>
    [JsonPropertyName("description")]
    public string Description { get; init; } = string.Empty;

    /// <summary>Comma-delimited custom name servers, or empty for DNS Check defaults.</summary>
    [JsonPropertyName("nameservers")]
    public string Nameservers { get; init; } = string.Empty;

    /// <summary>Whether the group is publicly viewable.</summary>
    [JsonPropertyName("public")]
    public bool IsPublic { get; init; }

    /// <summary>When the group was created.</summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>When the group was last updated.</summary>
    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; init; }

    /// <summary>Whether notifications are enabled.</summary>
    [JsonPropertyName("notifications")]
    public bool Notifications { get; init; }

    /// <summary>Aggregate pass/fail status of records in the group.</summary>
    [JsonPropertyName("status")]
    public DnsCheckStatus Status { get; init; }

    /// <summary>URL of the group page on DNS Check.</summary>
    [JsonPropertyName("url")]
    public string Url { get; init; } = string.Empty;
}
