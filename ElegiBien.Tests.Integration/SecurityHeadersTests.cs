using System.Net;

namespace ElegiBien.Tests.Integration;

public sealed class SecurityHeadersTests : IClassFixture<ElegiBienWebApplicationFactory>
{
    private readonly HttpClient _client;

    public SecurityHeadersTests(ElegiBienWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Home_SendsStrictSecurityHeaders()
    {
        using var response = await _client.GetAsync("/");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.Headers.Contains("Content-Security-Policy"));
        Assert.True(response.Headers.Contains("X-Content-Type-Options"));
        Assert.True(response.Headers.Contains("Referrer-Policy"));

        var csp = string.Join(" ", response.Headers.GetValues("Content-Security-Policy"));
        Assert.Contains("default-src 'self'", csp, StringComparison.Ordinal);
        Assert.Contains("script-src 'nonce-", csp, StringComparison.Ordinal);
        Assert.Contains("'strict-dynamic'", csp, StringComparison.Ordinal);
        Assert.DoesNotContain("unsafe-inline", csp, StringComparison.Ordinal);
        Assert.DoesNotContain("unsafe-eval", csp, StringComparison.Ordinal);
    }
}
