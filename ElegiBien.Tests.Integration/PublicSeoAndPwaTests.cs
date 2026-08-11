using System.Net;

namespace ElegiBien.Tests.Integration;

public sealed class PublicSeoAndPwaTests : IClassFixture<ElegiBienWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PublicSeoAndPwaTests(ElegiBienWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Sitemap_UsesCurrentHostAndIncludesAllPublicTools()
    {
        using var response = await _client.GetAsync("/sitemap.xml");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("application/xml", response.Content.Headers.ContentType?.MediaType);
        Assert.DoesNotContain("DOMINIO-DEFINITIVO", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("/AirConditioning</loc>", content, StringComparison.Ordinal);
        Assert.Contains("/Paint</loc>", content, StringComparison.Ordinal);
        Assert.Contains("/Flooring</loc>", content, StringComparison.Ordinal);
        Assert.Contains("/Heating</loc>", content, StringComparison.Ordinal);
        Assert.DoesNotContain("/Shared/", content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Robots_BlocksSharedResultsAndReferencesCurrentSitemap()
    {
        using var response = await _client.GetAsync("/robots.txt");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Disallow: /Shared/", content, StringComparison.Ordinal);
        Assert.Contains("Sitemap: http://localhost/sitemap.xml", content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Home_ExposesDescriptionAndCanonicalUrl()
    {
        using var response = await _client.GetAsync("/");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("<meta name=\"description\"", html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("<link rel=\"canonical\" href=\"http://localhost/\"", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ServiceWorker_CachesHeatingVisualAsset()
    {
        using var response = await _client.GetAsync("/service-worker.js");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("elegibien-static-v3", content, StringComparison.Ordinal);
        Assert.Contains("/images/category-heating.svg", content, StringComparison.Ordinal);
    }
}
