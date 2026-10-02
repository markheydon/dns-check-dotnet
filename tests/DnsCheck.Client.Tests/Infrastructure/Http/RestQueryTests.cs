using DnsCheck.Client.Infrastructure.Http;

namespace DnsCheck.Client.Tests.Infrastructure.Http;

public sealed class RestQueryTests
{
    [Fact]
    public void Append_NoParameters_ReturnsRelativePath()
    {
        string result = RestQuery.Append("groups/all", []);

        Assert.Equal("groups/all", result);
    }

    [Fact]
    public void Append_NullParameterValue_IsSkipped()
    {
        RestQuery.QueryParameter[] parameters =
        [
            new RestQuery.QueryParameter("api_key", null),
            new RestQuery.QueryParameter("other", "value"),
        ];

        string result = RestQuery.Append("groups/all", parameters);

        Assert.Equal("groups/all?other=value", result);
    }

    [Fact]
    public void Append_WithoutExistingQuery_UsesQuestionMarkSeparator()
    {
        RestQuery.QueryParameter[] parameters = [new RestQuery.QueryParameter("api_key", "secret")];

        string result = RestQuery.Append("groups/all", parameters);

        Assert.Equal("groups/all?api_key=secret", result);
    }

    [Fact]
    public void Append_WithExistingQuery_UsesAmpersandSeparator()
    {
        RestQuery.QueryParameter[] parameters = [new RestQuery.QueryParameter("api_key", "secret")];

        string result = RestQuery.Append("groups/all?verbose=1", parameters);

        Assert.Equal("groups/all?verbose=1&api_key=secret", result);
    }

    [Fact]
    public void Append_EscapesParameterNamesAndValues()
    {
        RestQuery.QueryParameter[] parameters = [new RestQuery.QueryParameter("a b", "c&d")];

        string result = RestQuery.Append("path", parameters);

        Assert.Equal("path?a%20b=c%26d", result);
    }
}
