using DnsCheck.Client;
using DnsCheck.Client.Models;
using DnsCheck.Client.Models.DnsRecords;
using DnsCheck.Client.Models.Groups;

namespace DnsCheck.MonitorConsole;

internal sealed class MonitorSampleContext : IDisposable
{
    internal const string DefaultExampleGroupUuid = "ea883d67-d9f6-45e3-b3a1-844dd1857824";

    internal const string ApiKeyEnvironmentVariable = "DNSCHECK_API_KEY";
    internal const string GroupUuidEnvironmentVariable = "DNSCHECK_GROUP_UUID";

    internal MonitorSampleContext(DnsCheckClient client, string groupUuid, bool hasApiKey)
    {
        Client = client;
        GroupUuid = groupUuid;
        HasApiKey = hasApiKey;
    }

    internal DnsCheckClient Client { get; }

    internal string GroupUuid { get; }

    internal bool HasApiKey { get; }

    internal int? LastRecordId { get; set; }

    internal static MonitorSampleContext Create()
    {
        string? apiKey = Environment.GetEnvironmentVariable(ApiKeyEnvironmentVariable);
        string groupUuid = Environment.GetEnvironmentVariable(GroupUuidEnvironmentVariable) ?? DefaultExampleGroupUuid;
        bool hasApiKey = !string.IsNullOrWhiteSpace(apiKey);

        DnsCheckClient client = hasApiKey
            ? new DnsCheckClient(apiKey!)
            : new DnsCheckClient();

        return new MonitorSampleContext(client, groupUuid, hasApiKey);
    }

    internal void WriteBanner()
    {
        Console.WriteLine("DnsCheck.MonitorConsole");
        Console.WriteLine($"Group UUID: {GroupUuid}");
        Console.WriteLine($"API key: {(HasApiKey ? "set (account-wide checks enabled)" : "not set (public example group only)")}");
        Console.WriteLine();
        Console.WriteLine(
            "Monitoring status Pass/Fail/Unknown is live data from DNS Check. "
            + "The public example group is documented to include failing checks on purpose — that is not an SDK error.");
        Console.WriteLine();
    }

    public void Dispose() => Client.Dispose();

    internal static string FormatMonitoringStatus(DnsCheckStatus status) =>
        status switch
        {
            DnsCheckStatus.Pass => "Pass",
            DnsCheckStatus.Fail => "Fail (expected on demo group for some records)",
            DnsCheckStatus.Unknown => "Unknown",
            _ => status.ToString(),
        };

    internal static void WriteGroupDetail(DnsRecordGroup group)
    {
        Console.WriteLine($"  Name: {group.Name}");
        Console.WriteLine($"  Monitoring status: {FormatMonitoringStatus(group.Status)}");
        Console.WriteLine($"  Public: {group.IsPublic}");
        Console.WriteLine($"  Records URL: {group.Url}");
    }

    internal static void WriteRecordDetail(DnsRecord record)
    {
        Console.WriteLine($"  {record.Name} ({record.RecordType}) id={record.Id}");
        Console.WriteLine($"  Monitoring status: {FormatMonitoringStatus(record.Status)}");
    }
}
