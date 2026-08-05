using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ElegiBien.Tests.Integration;

public class LegacyComparisonModelRemovalTests :
    IClassFixture<ElegiBienWebApplicationFactory>
{
    private static readonly string[] LegacyTables =
    [
        "ProductAlternatives",
        "ProductScores",
        "ScoreFactors",
        "PaintProductAlternatives",
        "PaintProductScores",
        "PaintScoreFactors",
        "FlooringProductAlternatives",
        "FlooringProductScores",
        "FlooringScoreFactors",
        "HeatingProductAlternatives",
        "HeatingProductScores",
        "HeatingScoreFactors"
    ];

    private readonly ElegiBienWebApplicationFactory _factory;

    public LegacyComparisonModelRemovalTests(
        ElegiBienWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public void EfModel_ContainsOnlyGenericComparisonTables()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ElegiBienDbContext>();

        var tables = db.Model
            .GetEntityTypes()
            .Select(entity => entity.GetTableName())
            .Where(table => table is not null)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        Assert.Contains("ComparisonAlternatives", tables);
        Assert.Contains("ComparisonScores", tables);
        Assert.Contains("ComparisonFactors", tables);

        foreach (var legacyTable in LegacyTables)
        {
            Assert.DoesNotContain(legacyTable, tables);
        }
    }
}
