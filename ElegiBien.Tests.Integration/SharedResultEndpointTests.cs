using System.Net;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ElegiBien.Tests.Integration;

public sealed class SharedResultEndpointTests : IClassFixture<ElegiBienWebApplicationFactory>
{
    private readonly ElegiBienWebApplicationFactory _factory;

    public SharedResultEndpointTests(ElegiBienWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task HeatingSharedResult_WithValidToken_ReturnsPageWithoutIndexing()
    {
        const string token = "0123456789abcdef0123456789abcdef0123456789abcdef";

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ElegiBienDbContext>();
            var analysisId = Guid.NewGuid();

            db.Analyses.Add(new Analysis
            {
                AnalysisId = analysisId,
                CategoryId = 4,
                MethodologyVersionId = 1,
                Mode = AnalysisMode.Quick,
                ConfidenceLevel = ConfidenceLevel.Medium,
                CreatedAtUtc = DateTime.UtcNow,
                CompletedAtUtc = DateTime.UtcNow,
                IsCompleted = true
            });

            db.HeatingCalculationResults.Add(new HeatingCalculationResult
            {
                AnalysisId = analysisId,
                SurfaceSquareMeters = 20,
                VolumeCubicMeters = 52,
                BasePowerWatts = 2000,
                AdjustmentPowerWatts = 300,
                EstimatedPowerWatts = 2300,
                RecommendedMinimumWatts = 2100,
                RecommendedMaximumWatts = 2500,
                IdealPowerWatts = 2300,
                IdealPowerKcalPerHour = 1978,
                ConfidenceLevel = ConfidenceLevel.Medium,
                RequiresProfessionalReview = false
            });

            db.SharedResults.Add(new SharedResult
            {
                AnalysisId = analysisId,
                PublicToken = token,
                CreatedAtUtc = DateTime.UtcNow,
                ExpiresAtUtc = DateTime.UtcNow.AddMonths(12),
                IsActive = true
            });

            await db.SaveChangesAsync();
        }

        var client = _factory.CreateClient();
        var response = await client.GetAsync($"/Shared/HeatingResult?token={token}");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("noindex, nofollow", html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Potencia ideal", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("AnalysisId", html, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("unknown")]
    [InlineData("")]
    public async Task SharedResult_WithInvalidToken_ReturnsNotFound(string token)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync(
            $"/Shared/HeatingResult?token={Uri.EscapeDataString(token)}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
