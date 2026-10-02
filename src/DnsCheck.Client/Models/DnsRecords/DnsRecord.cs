using System.Text.Json.Serialization;
using DnsCheck.Client.Models;

namespace DnsCheck.Client.Models.DnsRecords;

/// <summary>
/// A monitored DNS record within a DNS record group.
/// </summary>
public sealed record DnsRecord
{
    /// <summary>Unique identifier for the DNS record.</summary>
    [JsonPropertyName("id")]
    public int Id { get; init; }

    /// <summary>Fully qualified domain name of the record.</summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>DNS record type.</summary>
    [JsonPropertyName("record_type")]
    public DnsRecordType RecordType { get; init; }

    /// <summary>Expected value of the DNS record.</summary>
    [JsonPropertyName("value")]
    public string Value { get; init; } = string.Empty;

    /// <summary>Whether this record must be the only one of this name and type.</summary>
    [JsonPropertyName("exclusive")]
    public bool Exclusive { get; init; }

    /// <summary>Whether check logic is inverted (paid feature).</summary>
    [JsonPropertyName("invert")]
    public bool Invert { get; init; }

    /// <summary>When the record was created.</summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>When the record was last updated.</summary>
    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; init; }

    /// <summary>Pass/fail status of the record check.</summary>
    [JsonPropertyName("status")]
    public DnsCheckStatus Status { get; init; }

    /// <summary>Consecutive failed checks when status is failing.</summary>
    [JsonPropertyName("fail_count")]
    public int FailCount { get; init; }

    /// <summary>URL of the record details page on DNS Check.</summary>
    [JsonPropertyName("url")]
    public string Url { get; init; } = string.Empty;
}
