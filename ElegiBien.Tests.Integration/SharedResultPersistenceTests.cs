using System.Text.RegularExpressions;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;
using ElegiBien.Infrastructure.Data;
using ElegiBien.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Tests.Integration;

public sealed class SharedResultPersistenceTests
{
    [Fact]
    public async Task CreateOrGetToken_CreatesSecureTokenAndReusesActiveToken()
    {
        await using var db = CreateDbContext();
        var analysis = CreateAnalysis();
        db.Analyses.Add(analysis);
        await db.SaveChangesAsync();

        var service = new SharedResultService(db);

        var firstToken = await service.CreateOrGetTokenAsync(analysis.AnalysisId);
        var secondToken = await service.CreateOrGetTokenAsync(analysis.AnalysisId);

        Assert.Equal(firstToken, secondToken);
        Assert.Matches(new Regex("^[a-f0-9]{48}$"), firstToken);

        var stored = await db.SharedResults.AsNoTracking().SingleAsync();
        Assert.Equal(firstToken, stored.PublicToken);
        Assert.True(stored.IsActive);
        Assert.True(stored.ExpiresAtUtc > stored.CreatedAtUtc);
        Assert.Equal(0, stored.AccessCount);
    }

    [Fact]
    public async Task GetAnalysisId_WithValidToken_ReturnsAnalysisAndTracksAccess()
    {
        await using var db = CreateDbContext();
        var analysis = CreateAnalysis();
        db.Analyses.Add(analysis);
        await db.SaveChangesAsync();

        var service = new SharedResultService(db);
        var token = await service.CreateOrGetTokenAsync(analysis.AnalysisId);

        var resolvedAnalysisId = await service.GetAnalysisIdAsync(token);

        Assert.Equal(analysis.AnalysisId, resolvedAnalysisId);

        var stored = await db.SharedResults.AsNoTracking().SingleAsync();
        Assert.Equal(1, stored.AccessCount);
        Assert.NotNull(stored.LastAccessedAtUtc);
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid-token")]
    [InlineData("000000000000000000000000000000000000000000000000")]
    public async Task GetAnalysisId_WithUnknownOrMalformedToken_ReturnsNull(string token)
    {
        await using var db = CreateDbContext();
        var service = new SharedResultService(db);

        var result = await service.GetAnalysisIdAsync(token);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAnalysisId_WithExpiredOrInactiveToken_ReturnsNull()
    {
        await using var db = CreateDbContext();
        var expiredAnalysis = CreateAnalysis();
        var inactiveAnalysis = CreateAnalysis();
        db.Analyses.AddRange(expiredAnalysis, inactiveAnalysis);
        db.SharedResults.AddRange(
            new SharedResult
            {
                AnalysisId = expiredAnalysis.AnalysisId,
                PublicToken = new string('a', 48),
                CreatedAtUtc = DateTime.UtcNow.AddMonths(-13),
                ExpiresAtUtc = DateTime.UtcNow.AddDays(-1),
                IsActive = true
            },
            new SharedResult
            {
                AnalysisId = inactiveAnalysis.AnalysisId,
                PublicToken = new string('b', 48),
                CreatedAtUtc = DateTime.UtcNow,
                ExpiresAtUtc = DateTime.UtcNow.AddMonths(12),
                IsActive = false
            });
        await db.SaveChangesAsync();

        var service = new SharedResultService(db);

        Assert.Null(await service.GetAnalysisIdAsync(new string('a', 48)));
        Assert.Null(await service.GetAnalysisIdAsync(new string('b', 48)));
    }

    [Fact]
    public async Task CreateOrGetToken_ReplacesExpiredTokenWithoutDuplicatingRecord()
    {
        await using var db = CreateDbContext();
        var analysis = CreateAnalysis();
        var previousToken = new string('c', 48);
        db.Analyses.Add(analysis);
        db.SharedResults.Add(new SharedResult
        {
            AnalysisId = analysis.AnalysisId,
            PublicToken = previousToken,
            CreatedAtUtc = DateTime.UtcNow.AddMonths(-13),
            ExpiresAtUtc = DateTime.UtcNow.AddDays(-1),
            IsActive = true,
            AccessCount = 7,
            LastAccessedAtUtc = DateTime.UtcNow.AddDays(-2)
        });
        await db.SaveChangesAsync();

        var service = new SharedResultService(db);
        var newToken = await service.CreateOrGetTokenAsync(analysis.AnalysisId);

        Assert.NotEqual(previousToken, newToken);
        var stored = await db.SharedResults.AsNoTracking().ToListAsync();
        var record = Assert.Single(stored);
        Assert.Equal(newToken, record.PublicToken);
        Assert.Equal(0, record.AccessCount);
        Assert.Null(record.LastAccessedAtUtc);
        Assert.True(record.IsActive);
    }

    private static ElegiBienDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ElegiBienDbContext>()
            .UseInMemoryDatabase($"SharedResultTests_{Guid.NewGuid():N}")
            .Options;

        return new ElegiBienDbContext(options);
    }

    private static Analysis CreateAnalysis()
    {
        return new Analysis
        {
            AnalysisId = Guid.NewGuid(),
            CategoryId = 1,
            MethodologyVersionId = 1,
            Mode = AnalysisMode.Quick,
            ConfidenceLevel = ConfidenceLevel.Medium,
            CreatedAtUtc = DateTime.UtcNow,
            CompletedAtUtc = DateTime.UtcNow,
            IsCompleted = true
        };
    }
}
