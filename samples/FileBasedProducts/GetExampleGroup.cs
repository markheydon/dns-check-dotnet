#:project ../../src/DnsCheck.Client/DnsCheck.Client.csproj
#:property PublishAot=false

using DnsCheck.Client;
using DnsCheck.Client.Models.Groups;

using var httpClient = new HttpClient();
DnsCheckClient client = new(httpClient, apiKey: null);

const string exampleGroup = "ea883d67-d9f6-45e3-b3a1-844dd1857824";

DnsRecordGroup group = await client.Groups.GetAsync(exampleGroup);

if (string.IsNullOrWhiteSpace(group.Name))
{
    throw new InvalidOperationException("Expected a group name from the public example group.");
}

Console.WriteLine(group.Name);
