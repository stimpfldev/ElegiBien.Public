using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ElegiBien.Tests.Integration;

public sealed class ProductionErrorHandlingTests : IClassFixture<ElegiBienProductionWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProductionErrorHandlingTests(ElegiBienProductionWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task UnhandledException_ReturnsProfessionalErrorPage()
    {
        using var response = await _client.GetAsync("/__integration/error");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Contains("Ocurrió un problema", html, StringComparison.Ordinal);
        Assert.Contains("Volver al inicio", html, StringComparison.Ordinal);
        Assert.DoesNotContain("Development Mode", html, StringComparison.Ordinal);
        Assert.DoesNotContain("integration-test-exception", html, StringComparison.Ordinal);
    }
}

[ApiController]
[Route("__integration/error")]
public sealed class IntegrationFailureController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        throw new InvalidOperationException("integration-test-exception");
    }
}
