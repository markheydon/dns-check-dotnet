using System.Text.Json.Serialization;
using DnsCheck.Client.Infrastructure.Serialization;

namespace DnsCheck.Client.Models;

/// <summary>
/// Pass/fail monitoring status returned by the DNS Check API.
/// </summary>
[JsonConverter(typeof(DnsCheckStatusJsonConverter))]
public enum DnsCheckStatus
{
    /// <summary>All checks in scope are passing.</summary>
    Pass,

    /// <summary>At least one check in scope is failing.</summary>
    Fail,

    /// <summary>Status could not be determined.</summary>
    Unknown,
}
