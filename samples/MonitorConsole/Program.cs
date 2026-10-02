using DnsCheck.MonitorConsole;

using CancellationTokenSource cancellation = new();
Console.CancelKeyPress += (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    cancellation.Cancel();
};

ConsoleRunOptions runOptions;
try
{
    runOptions = ConsoleRunOptions.Parse(args);
}
catch (InvalidOperationException ex)
{
    Console.Error.WriteLine(ex.Message);
    return 1;
}

if (runOptions.ShowHelp)
{
    ConsoleRunOptions.WriteHelp();
    return 0;
}

try
{
    using MonitorSampleContext context = MonitorSampleContext.Create();

    if (runOptions.RunAll)
    {
        return await MonitorSampleRunner.RunAllAsync(context, cancellation.Token);
    }

    return await MonitorSampleRunner.RunInteractiveAsync(context, cancellation.Token);
}
catch (OperationCanceledException)
{
    return 130;
}
