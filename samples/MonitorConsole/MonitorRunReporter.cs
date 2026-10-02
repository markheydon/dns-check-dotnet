using System.Diagnostics;

namespace DnsCheck.MonitorConsole;

internal enum MonitorCheckOutcome
{
    Passed,
    Failed,
    Skipped,
}

internal sealed record MonitorCheckResult(
    string Name,
    MonitorCheckOutcome Outcome,
    TimeSpan Duration,
    string? Message);

/// <summary>
/// Prints dotnet test-style status lines and summary for <c>--run-all</c> mode.
/// </summary>
internal sealed class MonitorRunReporter
{
    private readonly List<MonitorCheckResult> _results = [];
    private readonly Stopwatch _totalStopwatch = Stopwatch.StartNew();

    internal async Task RunCheckAsync(
        string name,
        Func<CancellationToken, Task> action,
        CancellationToken cancellationToken = default)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        try
        {
            await action(cancellationToken);
            stopwatch.Stop();
            MonitorCheckResult result = new(name, MonitorCheckOutcome.Passed, stopwatch.Elapsed, null);
            _results.Add(result);
            WriteLine(result);
        }
        catch (MonitorCheckSkippedException ex)
        {
            stopwatch.Stop();
            MonitorCheckResult result = new(name, MonitorCheckOutcome.Skipped, stopwatch.Elapsed, ex.Message);
            _results.Add(result);
            WriteLine(result);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            MonitorCheckResult result = new(name, MonitorCheckOutcome.Failed, stopwatch.Elapsed, ex.Message);
            _results.Add(result);
            WriteLine(result);
        }
    }

    internal int WriteSummaryAndGetExitCode()
    {
        _totalStopwatch.Stop();

        int passed = _results.Count(r => r.Outcome == MonitorCheckOutcome.Passed);
        int failed = _results.Count(r => r.Outcome == MonitorCheckOutcome.Failed);
        int skipped = _results.Count(r => r.Outcome == MonitorCheckOutcome.Skipped);
        int total = _results.Count;

        Console.WriteLine();
        Console.WriteLine($"Test run for DnsCheck.MonitorConsole ({Environment.Version})");
        Console.WriteLine($"Total checks: {total}");
        WriteSummaryCount("     Passed", passed, ConsoleColor.Green);
        WriteSummaryCount("     Failed", failed, ConsoleColor.Red);
        WriteSummaryCount("    Skipped", skipped, ConsoleColor.DarkYellow);
        Console.WriteLine($" Total time: {_totalStopwatch.Elapsed.TotalSeconds:F4} Seconds");
        Console.WriteLine();

        if (failed > 0 || passed == 0)
        {
            ConsoleColorSupport.WriteLine(
                $"Failed!  - Failed: {failed}, Passed: {passed}, Skipped: {skipped}, Total: {total}",
                ConsoleColor.Red);
        }
        else
        {
            ConsoleColorSupport.WriteLine(
                $"Passed!  - Failed: {failed}, Passed: {passed}, Skipped: {skipped}, Total: {total}",
                ConsoleColor.Green);
        }

        Console.WriteLine();
        return failed > 0 || passed == 0 ? 1 : 0;
    }

    private static void WriteLine(MonitorCheckResult result)
    {
        string duration = $"({result.Duration.TotalMilliseconds:F0} ms)";
        string label = result.Name;

        switch (result.Outcome)
        {
            case MonitorCheckOutcome.Passed:
                ConsoleColorSupport.WriteLine($"  {SymbolPassed}  {label,-52} {duration}", ConsoleColor.Green);
                break;
            case MonitorCheckOutcome.Failed:
                ConsoleColorSupport.WriteLine($"  {SymbolFailed}  {label,-52} {duration}", ConsoleColor.Red);
                if (!string.IsNullOrWhiteSpace(result.Message))
                {
                    ConsoleColorSupport.WriteLine($"      {result.Message}", ConsoleColor.Red);
                }

                break;
            case MonitorCheckOutcome.Skipped:
                ConsoleColorSupport.WriteLine($"  {SymbolSkipped}  {label,-52} {duration}", ConsoleColor.DarkYellow);
                if (!string.IsNullOrWhiteSpace(result.Message))
                {
                    ConsoleColorSupport.WriteLine($"      Skipped: {result.Message}", ConsoleColor.DarkYellow);
                }

                break;
        }
    }

    private static void WriteSummaryCount(string label, int count, ConsoleColor colour)
    {
        if (ConsoleColorSupport.IsEnabled)
        {
            ConsoleColorSupport.Write(label, colour);
            Console.WriteLine($": {count}");
        }
        else
        {
            Console.WriteLine($"{label}: {count}");
        }
    }

    private const string SymbolPassed = "\u2713";
    private const string SymbolFailed = "\u2717";
    private const string SymbolSkipped = "\u2298";
}

internal sealed class MonitorCheckSkippedException : Exception
{
    internal MonitorCheckSkippedException(string message)
        : base(message)
    {
    }
}
