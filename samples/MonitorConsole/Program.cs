using DnsCheck.Client;
using DnsCheck.Client.Models.DnsRecords;
using DnsCheck.Client.Models.Groups;

const string DefaultExampleGroupUuid = "ea883d67-d9f6-45e3-b3a1-844dd1857824";

string? apiKey = Environment.GetEnvironmentVariable("DNSCHECK_API_KEY");
string groupUuid = Environment.GetEnvironmentVariable("DNSCHECK_GROUP_UUID") ?? DefaultExampleGroupUuid;

try
{
    using DnsCheckClient client = string.IsNullOrWhiteSpace(apiKey)
        ? new DnsCheckClient()
        : new DnsCheckClient(apiKey);

    DnsRecordGroup group = await client.Groups.GetAsync(groupUuid);
    Console.WriteLine($"Group: {group.Name} ({group.Uuid}) — status {group.Status}");

    IReadOnlyList<DnsRecord> records = await client.DnsRecords.ListInGroupAsync(groupUuid);
    Console.WriteLine($"Records in group: {records.Count}");

    if (records.Count > 0)
    {
        DnsRecord first = records[0];
        DnsRecord record = await client.DnsRecords.GetAsync(groupUuid, first.Id);
        Console.WriteLine($"First record: {record.Name} ({record.RecordType}) — status {record.Status}");
    }

    if (!string.IsNullOrWhiteSpace(apiKey))
    {
        IReadOnlyList<DnsRecordGroup> groups = await client.Groups.ListAllAsync();
        Console.WriteLine($"Account groups: {groups.Count}");

        IReadOnlyList<DnsRecord> allRecords = await client.DnsRecords.ListAllAsync();
        Console.WriteLine($"Account records: {allRecords.Count}");
    }

    return 0;
}
catch (DnsCheckException ex)
{
    Console.Error.WriteLine(ex.Message);
    return 1;
}
