using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace DnsCheck.Client.Models;

/// <summary>
/// DNS record types supported by DNS Check monitoring.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<DnsRecordType>))]
[SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Enum names match DNS Check API wire values.")]
public enum DnsRecordType
{
    /// <summary>A record.</summary>
    A,

    /// <summary>AAAA record.</summary>
    AAAA,

    /// <summary>ALIAS record.</summary>
    ALIAS,

    /// <summary>CAA record.</summary>
    CAA,

    /// <summary>CNAME record.</summary>
    CNAME,

    /// <summary>HTTPS record.</summary>
    HTTPS,

    /// <summary>MX record.</summary>
    MX,

    /// <summary>NS record.</summary>
    NS,

    /// <summary>PTR record.</summary>
    PTR,

    /// <summary>SOA record.</summary>
    SOA,

    /// <summary>SPF record.</summary>
    SPF,

    /// <summary>SRV record.</summary>
    SRV,

    /// <summary>SVCB record.</summary>
    SVCB,

    /// <summary>TXT record.</summary>
    TXT,
}
