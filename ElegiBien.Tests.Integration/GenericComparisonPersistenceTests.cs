using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ElegiBien.Tests.Integration;

public class GenericComparisonPersistenceTests :
    IClassFixture<ElegiBienWebApplicationFactory>
{
    private readonly ElegiBienWebApplicationFactory _factory;

    public GenericComparisonPersistenceTests(
        ElegiBienWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task HeatingComparison_WritesOnlyGenericTables()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ElegiBienDbContext>();
        var store = scope.ServiceProvider.GetRequiredService<IHeatingComparisonStore>();

        var analysisId = Guid.NewGuid();
        db.Analyses.Add(new Analysis
        {
            AnalysisId = analysisId,
            CategoryId = 5,
            MethodologyVersionId = 1,
            Mode = AnalysisMode.Quick,
            ConfidenceLevel = ConfidenceLevel.Medium,
            CreatedAtUtc = DateTime.UtcNow,
            IsCompleted = true
        });
        await db.SaveChangesAsync();

        var first = CreateProduct("Equipo A", 2200m, 120000m);
        var second = CreateProduct("Equipo B", 2500m, 140000m);
        var result = new HeatingComparisonResultDto
        {
            FirstProduct = CreateScore("Equipo A", 82),
            SecondProduct = CreateScore("Equipo B", 76)
        };

        await store.SaveAsync(analysisId, first, second, result);

        Assert.Equal(
            2,
            await db.ComparisonAlternatives.CountAsync(x =>
                x.AnalysisId == analysisId &&
                x.CategoryCode == CategoryCode.Heating));
        Assert.Equal(
            2,
            await db.ComparisonScores.CountAsync(x =>
                x.Alternative.AnalysisId == analysisId));
    }

    private static HeatingProductAlternativeDto CreateProduct(
        string name,
        decimal watts,
        decimal price)
    {
        return new HeatingProductAlternativeDto
        {
            Name = name,
            HeatingCapacityWatts = watts,
            PurchasePrice = price
        };
    }

    private static HeatingProductScoreResultDto CreateScore(
        string name,
        int total)
    {
        return new HeatingProductScoreResultDto
        {
            ProductName = name,
            TotalScore = total,
            CapacityStatus = HeatingCapacityStatus.Correct,
            IsEligible = true,
            AppliedMaximumScore = 100,
            Factors = []
        };
    }
}
