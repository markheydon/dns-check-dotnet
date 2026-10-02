using DnsCheck.Client;
using DnsCheck.Client.Models.DnsRecords;
using DnsCheck.Client.Models.Groups;

namespace DnsCheck.MonitorConsole;

internal static class MonitorSampleRunner
{
    internal static async Task<int> RunAllAsync(MonitorSampleContext context, CancellationToken cancellationToken)
    {
        context.WriteBanner();
        MonitorRunReporter reporter = new();

        await reporter.RunCheckAsync(
            "Groups.GetAsync",
            async ct =>
            {
                DnsRecordGroup group = await context.Client.Groups.GetAsync(context.GroupUuid, ct);
                Console.WriteLine($"  Retrieved group '{group.Name}' — monitoring status {group.Status}");
            },
            cancellationToken);

        await reporter.RunCheckAsync(
            "DnsRecords.ListInGroupAsync",
            async ct =>
            {
                IReadOnlyList<DnsRecord> records = await context.Client.DnsRecords.ListInGroupAsync(context.GroupUuid, ct);
                Console.WriteLine($"  Listed {records.Count} record(s) in group");
                if (records.Count > 0)
                {
                    context.LastRecordId = records[0].Id;
                }
            },
            cancellationToken);

        await reporter.RunCheckAsync(
            "DnsRecords.GetAsync",
            async ct =>
            {
                if (context.LastRecordId is null)
                {
                    throw new MonitorCheckSkippedException("No records returned from ListInGroupAsync.");
                }

                DnsRecord record = await context.Client.DnsRecords.GetAsync(
                    context.GroupUuid,
                    context.LastRecordId.Value,
                    ct);
                Console.WriteLine($"  Retrieved record id {record.Id} — monitoring status {record.Status}");
            },
            cancellationToken);

        await reporter.RunCheckAsync(
            "Groups.ListAllAsync",
            async ct =>
            {
                if (!context.HasApiKey)
                {
                    throw new MonitorCheckSkippedException(GetAccountApiKeySkipMessage(context));
                }

                IReadOnlyList<DnsRecordGroup> groups = await context.Client.Groups.ListAllAsync(ct);
                Console.WriteLine($"  Listed {groups.Count} group(s) for account");
            },
            cancellationToken);

        await reporter.RunCheckAsync(
            "DnsRecords.ListAllAsync",
            async ct =>
            {
                if (!context.HasApiKey)
                {
                    throw new MonitorCheckSkippedException(GetAccountApiKeySkipMessage(context));
                }

                IReadOnlyList<DnsRecord> records = await context.Client.DnsRecords.ListAllAsync(ct);
                Console.WriteLine($"  Listed {records.Count} record(s) for account");
            },
            cancellationToken);

        return reporter.WriteSummaryAndGetExitCode();
    }

    internal static async Task<int> RunInteractiveAsync(MonitorSampleContext context, CancellationToken cancellationToken)
    {
        context.WriteBanner();

        while (!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine("Choose a check:");
            Console.WriteLine("  1  Get DNS record group");
            Console.WriteLine("  2  List DNS records in group");
            Console.WriteLine("  3  Get one DNS record (by id)");
            Console.WriteLine("  4  List all groups (API key)");
            Console.WriteLine("  5  List all records (API key)");
            Console.WriteLine("  6  Run all checks (same as --run-all)");
            Console.WriteLine("  q  Quit");
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            try
            {
                switch (line.Trim().ToLowerInvariant())
                {
                    case "q":
                    case "quit":
                    case "exit":
                        return 0;
                    case "1":
                        await InteractiveGetGroupAsync(context, cancellationToken);
                        break;
                    case "2":
                        await InteractiveListRecordsAsync(context, cancellationToken);
                        break;
                    case "3":
                        await InteractiveGetRecordAsync(context, cancellationToken);
                        break;
                    case "4":
                        await InteractiveListAllGroupsAsync(context, cancellationToken);
                        break;
                    case "5":
                        await InteractiveListAllRecordsAsync(context, cancellationToken);
                        break;
                    case "6":
                        return await RunAllAsync(context, cancellationToken);
                    default:
                        Console.WriteLine("Unknown option.");
                        break;
                }
            }
            catch (DnsCheckException ex)
            {
                ConsoleColorSupport.WriteLine($"Error: {ex.Message}", ConsoleColor.Red);
            }

            Console.WriteLine();
        }

        return 0;
    }

    private static async Task InteractiveGetGroupAsync(MonitorSampleContext context, CancellationToken cancellationToken)
    {
        DnsRecordGroup group = await context.Client.Groups.GetAsync(context.GroupUuid, cancellationToken);
        MonitorSampleContext.WriteGroupDetail(group);
    }

    private static async Task InteractiveListRecordsAsync(MonitorSampleContext context, CancellationToken cancellationToken)
    {
        IReadOnlyList<DnsRecord> records = await context.Client.DnsRecords.ListInGroupAsync(context.GroupUuid, cancellationToken);
        Console.WriteLine($"  {records.Count} record(s):");
        foreach (DnsRecord record in records.Take(10))
        {
            MonitorSampleContext.WriteRecordDetail(record);
        }

        if (records.Count > 10)
        {
            Console.WriteLine($"  … and {records.Count - 10} more");
        }

        if (records.Count > 0)
        {
            context.LastRecordId = records[0].Id;
        }
    }

    private static async Task InteractiveGetRecordAsync(MonitorSampleContext context, CancellationToken cancellationToken)
    {
        int recordId = context.LastRecordId ?? ReadRecordId();
        DnsRecord record = await context.Client.DnsRecords.GetAsync(context.GroupUuid, recordId, cancellationToken);
        MonitorSampleContext.WriteRecordDetail(record);
        context.LastRecordId = record.Id;
    }

    private static async Task InteractiveListAllGroupsAsync(MonitorSampleContext context, CancellationToken cancellationToken)
    {
        if (!context.HasApiKey)
        {
            throw new DnsCheckRequestException(GetAccountApiKeyRequiredMessage(context));
        }

        IReadOnlyList<DnsRecordGroup> groups = await context.Client.Groups.ListAllAsync(cancellationToken);
        Console.WriteLine($"  {groups.Count} group(s):");
        foreach (DnsRecordGroup group in groups.Take(10))
        {
            Console.WriteLine($"  - {group.Name} ({group.Uuid}) — {group.Status}");
        }

        if (groups.Count > 10)
        {
            Console.WriteLine($"  … and {groups.Count - 10} more");
        }
    }

    private static async Task InteractiveListAllRecordsAsync(MonitorSampleContext context, CancellationToken cancellationToken)
    {
        if (!context.HasApiKey)
        {
            throw new DnsCheckRequestException(GetAccountApiKeyRequiredMessage(context));
        }

        IReadOnlyList<DnsRecord> records = await context.Client.DnsRecords.ListAllAsync(cancellationToken);
        Console.WriteLine($"  {records.Count} record(s) across account (first 5 shown):");
        foreach (DnsRecord record in records.Take(5))
        {
            MonitorSampleContext.WriteRecordDetail(record);
        }
    }

    private static string GetAccountApiKeySkipMessage(MonitorSampleContext context) =>
        context.HasInvalidApiKey
            ? $"{MonitorSampleContext.ApiKeyEnvironmentVariable} is set but empty or whitespace."
            : $"{MonitorSampleContext.ApiKeyEnvironmentVariable} not set.";

    private static string GetAccountApiKeyRequiredMessage(MonitorSampleContext context) =>
        context.HasInvalidApiKey
            ? $"{MonitorSampleContext.ApiKeyEnvironmentVariable} is set but empty or whitespace."
            : $"Set {MonitorSampleContext.ApiKeyEnvironmentVariable} to run account-wide checks.";

    private static int ReadRecordId()
    {
        Console.Write("Record id: ");
        string? input = Console.ReadLine();
        if (!int.TryParse(input, out int recordId) || recordId <= 0)
        {
            throw new DnsCheckRequestException("Record id must be a positive integer.");
        }

        return recordId;
    }
}
