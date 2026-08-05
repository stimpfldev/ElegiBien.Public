using System.Net;

namespace ElegiBien.Tests.Integration;

public sealed class FlooringPagesTests : IClassFixture<ElegiBienWebApplicationFactory>
{
    private readonly HttpClient _client;

    public FlooringPagesTests(ElegiBienWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task FlooringIndex_ReturnsSuccess()
    {
        using var response = await _client.GetAsync("/Flooring");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Cerámicos", content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task FlooringIndex_SendsSecurityHeaders()
    {
        using var response = await _client.GetAsync("/Flooring");

        Assert.True(response.Headers.Contains("X-Content-Type-Options"));
        Assert.True(response.Headers.Contains("X-Frame-Options"));
        Assert.True(response.Headers.Contains("Referrer-Policy"));
        Assert.True(response.Headers.Contains("Permissions-Policy"));
    }

    [Fact]
    public async Task FlooringPost_WithoutAntiforgeryToken_IsRejected()
    {
        using var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["LengthMeters"] = "5",
            ["WidthMeters"] = "4",
            ["InstallationPattern"] = "1",
            ["WastePercentage"] = "10"
        });

        using var response = await _client.PostAsync("/Flooring", form);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Methodology_IncludesFlooringSection()
    {
        using var response = await _client.GetAsync("/Legal/Methodology");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Cerámicos", content, StringComparison.OrdinalIgnoreCase);
    }
}
