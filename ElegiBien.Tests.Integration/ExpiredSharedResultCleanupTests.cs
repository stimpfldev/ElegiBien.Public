using ElegiBien.Domain.Entities;
using ElegiBien.Infrastructure.Data;
using ElegiBien.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Tests.Integration;

public class ExpiredSharedResultCleanupTests
{
    [Fact]
    public async Task CleanupAsync_RemovesOnlyExpiredSharedResults()
    {
        var options = new DbContextOptionsBuilder<ElegiBienDbContext>()
            .UseInMemoryDatabase($"Cleanup_{Guid.NewGuid():N}")
            .Options;

        await using var dbContext = new ElegiBienDbContext(options);

        var expiredAnalysisId = Guid.NewGuid();
        var activeAnalysisId = Guid.NewGuid();
        var now = new DateTime(2026, 8, 10, 12, 0, 0, DateTimeKind.Utc);

        dbContext.Analyses.AddRange(
            new Analysis { AnalysisId = expiredAnalysisId },
            new Analysis { AnalysisId = activeAnalysisId });

        dbContext.SharedResults.AddRange(
            new SharedResult
            {
                AnalysisId = expiredAnalysisId,
                PublicToken = "expired-token",
                CreatedAtUtc = now.AddMonths(-12),
                ExpiresAtUtc = now.AddMinutes(-1),
                IsActive = true
            },
            new SharedResult
            {
                AnalysisId = activeAnalysisId,
                PublicToken = "active-token",
                CreatedAtUtc = now,
                ExpiresAtUtc = now.AddMonths(12),
                IsActive = true
            });

        await dbContext.SaveChangesAsync();

        var service = new ExpiredSharedResultCleanupService(dbContext);

        var removed = await service.CleanupAsync(now);

        Assert.Equal(1, removed);
        Assert.False(await dbContext.SharedResults
            .AnyAsync(x => x.PublicToken == "expired-token"));
        Assert.True(await dbContext.SharedResults
            .AnyAsync(x => x.PublicToken == "active-token"));

        Assert.True(await dbContext.Analyses
            .AnyAsync(x => x.AnalysisId == expiredAnalysisId));
    }

    [Fact]
    public async Task CleanupAsync_WithNoExpiredResults_DoesNothing()
    {
        var options = new DbContextOptionsBuilder<ElegiBienDbContext>()
            .UseInMemoryDatabase($"Cleanup_{Guid.NewGuid():N}")
            .Options;

        await using var dbContext = new ElegiBienDbContext(options);

        var service = new ExpiredSharedResultCleanupService(dbContext);

        var removed = await service.CleanupAsync(DateTime.UtcNow);

        Assert.Equal(0, removed);
    }
}
