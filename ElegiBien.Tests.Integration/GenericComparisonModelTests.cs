using ElegiBien.Domain.Entities;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ElegiBien.Tests.Integration;

public class GenericComparisonModelTests
{
    [Fact]
    public void DbModel_ContainsGenericComparisonEntities()
    {
        var options = new DbContextOptionsBuilder<ElegiBienDbContext>()
            .UseInMemoryDatabase($"comparison-model-{Guid.NewGuid()}")
            .Options;

        using var db = new ElegiBienDbContext(options);

        Assert.NotNull(db.Model.FindEntityType(typeof(ComparisonAlternative)));
        Assert.NotNull(db.Model.FindEntityType(typeof(ComparisonScore)));
        Assert.NotNull(db.Model.FindEntityType(typeof(ComparisonFactor)));
    }
}
