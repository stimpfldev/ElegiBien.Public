using System.Security.Cryptography;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Infrastructure.Persistence;

public class SharedResultService : ISharedResultService
{
    private readonly ElegiBienDbContext _dbContext;

    public SharedResultService(
        ElegiBienDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> CreateOrGetTokenAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.SharedResults
            .SingleOrDefaultAsync(
                x =>
                    x.AnalysisId == analysisId &&
                    x.IsActive &&
                    x.ExpiresAtUtc > DateTime.UtcNow,
                cancellationToken);

        if (existing is not null)
        {
            return existing.PublicToken;
        }

        var analysisExists = await _dbContext.Analyses
            .AnyAsync(
                x => x.AnalysisId == analysisId,
                cancellationToken);

        if (!analysisExists)
        {
            throw new InvalidOperationException(
                "No se encontró el análisis.");
        }

        var sharedResult = new SharedResult
        {
            AnalysisId = analysisId,
            PublicToken = CreateToken(),
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = DateTime.UtcNow.AddMonths(12),
            IsActive = true
        };

        _dbContext.SharedResults.Add(sharedResult);

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return sharedResult.PublicToken;
    }

    public async Task<Guid?> GetAnalysisIdAsync(
        string publicToken,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(publicToken))
        {
            return null;
        }

        var sharedResult = await _dbContext.SharedResults
            .SingleOrDefaultAsync(
                x =>
                    x.PublicToken == publicToken &&
                    x.IsActive &&
                    x.ExpiresAtUtc > DateTime.UtcNow,
                cancellationToken);

        if (sharedResult is null)
        {
            return null;
        }

        sharedResult.AccessCount++;
        sharedResult.LastAccessedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return sharedResult.AnalysisId;
    }

    private static string CreateToken()
    {
        return Convert
            .ToHexString(RandomNumberGenerator.GetBytes(24))
            .ToLowerInvariant();
    }
}