using System.Reflection;

namespace DnsCheck.Client.Tests.TestSupport;

internal static class DnsCheckClientTestAccess
{
    internal static HttpClient GetHttpClient(DnsCheck.Client.DnsCheckClient client)
    {
        FieldInfo field = typeof(DnsCheck.Client.DnsCheckClient).GetField(
            "_httpClient",
            BindingFlags.Instance | BindingFlags.NonPublic)!;

        return (HttpClient)field.GetValue(client)!;
    }

    internal static string? GetApiKey(DnsCheck.Client.DnsCheckClient client)
    {
        PropertyInfo groupsProperty = typeof(DnsCheck.Client.DnsCheckClient).GetProperty(
            nameof(DnsCheck.Client.DnsCheckClient.Groups),
            BindingFlags.Instance | BindingFlags.Public)!;

        object groups = groupsProperty.GetValue(client)!;

        FieldInfo restField = groups.GetType().GetField("_rest", BindingFlags.Instance | BindingFlags.NonPublic)!;
        object rest = restField.GetValue(groups)!;

        PropertyInfo apiKeyProperty = rest.GetType().GetProperty(
            "ApiKey",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!;

        return (string?)apiKeyProperty.GetValue(rest);
    }
}
