using System.Net;

namespace ElegiBien.Tests.Integration;

public sealed class CoreNavigationPagesTests : IClassFixture<ElegiBienWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CoreNavigationPagesTests(ElegiBienWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/AirConditioning")]
    [InlineData("/Paint")]
    [InlineData("/Flooring")]
    [InlineData("/Heating")]
    [InlineData("/Contact")]
    public async Task CorePages_ReturnSuccess(string url)
    {
        using var response = await _client.GetAsync(url);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Home_IncludesHeatingAndContactLinks()
    {
        using var response = await _client.GetAsync("/");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("Calefacción", html);
        Assert.Contains("Contacto", html);
    }

    [Fact]
    public async Task Home_GivesEqualPrimaryWeightToAllTools()
    {
        var response = await _client.GetAsync("/");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("btn btn-primary btn-lg", html);
        Assert.Contains("AirConditioning", html);
        Assert.Contains("Paint", html);
        Assert.Contains("Flooring", html);
        Assert.Contains("Heating", html);
        Assert.DoesNotContain("Calcular cerámicos y pisos", html);
        Assert.DoesNotContain("category-card-featured", html);
    }

    [Fact]
    public async Task Home_IncludesLanguageAndUnitPreferences()
    {
        using var response = await _client.GetAsync("/");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("data-pref-language=\"es\"", html);
        Assert.Contains("data-pref-language=\"en\"", html);
        Assert.Contains("data-pref-units=\"metric\"", html);
        Assert.Contains("data-pref-units=\"imperial\"", html);
        Assert.Contains("/js/presentation-preferences.js", html);
        Assert.Contains("/js/presentation-translations-extra.js", html);
    }
}
