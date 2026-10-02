namespace DnsCheck.MonitorConsole;

internal static class ConsoleColorSupport
{
    private static readonly Lazy<bool> Enabled = new(Detect);

    internal static bool IsEnabled => Enabled.Value;

    private static bool Detect()
    {
        if (Console.IsOutputRedirected)
        {
            return false;
        }

        string? noColor = Environment.GetEnvironmentVariable("NO_COLOR");
        return string.IsNullOrEmpty(noColor);
    }

    internal static void WriteLine(string text, ConsoleColor colour)
    {
        if (!IsEnabled)
        {
            Console.WriteLine(text);
            return;
        }

        Console.ForegroundColor = colour;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    internal static void Write(string text, ConsoleColor colour)
    {
        if (!IsEnabled)
        {
            Console.Write(text);
            return;
        }

        Console.ForegroundColor = colour;
        Console.Write(text);
        Console.ResetColor();
    }
}
