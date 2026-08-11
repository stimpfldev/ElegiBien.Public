using System.Net;

namespace ElegiBien.Tests.Integration;

public sealed class HeatingPagesTests : IClassFixture<ElegiBienWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HeatingPagesTests(ElegiBienWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task HeatingIndex_ReturnsSuccess()
    {
        using var response = await _client.GetAsync("/Heating");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("calefacción", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Calcular potencia", content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task HeatingIndex_SendsSecurityHeaders()
    {
        using var response = await _client.GetAsync("/Heating");

        Assert.True(response.Headers.Contains("X-Content-Type-Options"));
        Assert.True(response.Headers.Contains("X-Frame-Options"));
        Assert.True(response.Headers.Contains("Referrer-Policy"));
        Assert.True(response.Headers.Contains("Permissions-Policy"));
    }

    [Fact]
    public async Task HeatingPost_WithoutAntiforgeryToken_IsRejected()
    {
        using var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Input.LengthMeters"] = "5",
            ["Input.WidthMeters"] = "4",
            ["Input.HeightMeters"] = "2.6",
            ["Input.ClimateZone"] = "1",
            ["Input.InsulationLevel"] = "2",
            ["Input.ExteriorWallsCount"] = "1",
            ["Input.WindowExposure"] = "1"
        });

        using var response = await _client.PostAsync("/Heating", form);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task HeatingCompare_UnknownAnalysis_ReturnsNotFound()
    {
        using var response = await _client.GetAsync($"/Heating/Compare?id={Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task SharedHeatingResult_UnknownToken_ReturnsNotFound()
    {
        using var response = await _client.GetAsync("/Shared/HeatingResult?token=token-inexistente");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Methodology_IncludesHeatingSection()
    {
        using var response = await _client.GetAsync("/Legal/Methodology");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Calefacción", content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ContactPage_ReturnsSuccess()
    {
        using var response = await _client.GetAsync("/Contact");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
