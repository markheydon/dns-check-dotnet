using System.Text.Json.Serialization;
using DnsCheck.Client.Models;
using DnsCheck.Client.Models.DnsRecords;
using DnsCheck.Client.Models.Groups;

namespace DnsCheck.Client.Infrastructure.Serialization;

[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(GroupResponse))]
[JsonSerializable(typeof(GroupsListResponse))]
[JsonSerializable(typeof(DnsRecordGroup))]
[JsonSerializable(typeof(DnsRecordResponse))]
[JsonSerializable(typeof(DnsRecordsListResponse))]
[JsonSerializable(typeof(DnsRecord))]
[JsonSerializable(typeof(DnsCheckStatus))]
[JsonSerializable(typeof(DnsRecordType))]
[JsonSerializable(typeof(string))]
internal partial class DnsCheckJsonContext : JsonSerializerContext;
