namespace DnsCheck.MonitorConsole;

/// <summary>
/// Command-line options for the monitor console sample.
/// </summary>
internal sealed class ConsoleRunOptions
{
    /// <summary>When <see langword="true"/>, runs every registered check without the interactive menu.</summary>
    public bool RunAll { get; init; }

    /// <summary>Prints usage and exits.</summary>
    public bool ShowHelp { get; init; }

    /// <summary>
    /// Parses command-line arguments.
    /// </summary>
    /// <param name="args">Application arguments.</param>
    /// <returns>Parsed options.</returns>
    internal static ConsoleRunOptions Parse(string[] args)
    {
        bool runAll = false;
        bool showHelp = false;

        foreach (string arg in args)
        {
            if (arg is "--run-all" or "-a")
            {
                runAll = true;
                continue;
            }

            if (arg is "--help" or "-h" or "-?")
            {
                showHelp = true;
                continue;
            }

            throw new InvalidOperationException($"Unknown argument '{arg}'. Use --help for usage.");
        }

        return new ConsoleRunOptions
        {
            RunAll = runAll,
            ShowHelp = showHelp,
        };
    }

    internal static void WriteHelp()
    {
        Console.WriteLine("DnsCheck.MonitorConsole — live API smoke checks for DnsCheck.Client");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  dotnet run --project samples/MonitorConsole [options]");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  --run-all, -a   Run all checks and print a dotnet test-style summary (for CI and scripts)");
        Console.WriteLine("  --help, -h      Show this help");
        Console.WriteLine();
        Console.WriteLine("With no options, an interactive menu is shown.");
        Console.WriteLine();
        Console.WriteLine("Environment:");
        Console.WriteLine("  DNSCHECK_API_KEY       Optional; required for account-wide list checks");
        Console.WriteLine("  DNSCHECK_GROUP_UUID    Optional; defaults to the public example group");
        Console.WriteLine();
        Console.WriteLine("Note: monitoring status Pass/Fail/Unknown comes from DNS Check, not this tool.");
        Console.WriteLine("The public example group is documented to include failing checks on purpose.");
    }
}
