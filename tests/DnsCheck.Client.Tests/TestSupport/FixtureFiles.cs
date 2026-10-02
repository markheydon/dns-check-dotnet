namespace DnsCheck.Client.Tests.TestSupport;

internal static class FixtureFiles
{
    internal static string Read(string fileName)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "TestSupport", "Fixtures", fileName);
        return File.ReadAllText(path);
    }
}
