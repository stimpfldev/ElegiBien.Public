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
        var analysisExists = await _dbContext.Analyses
            .AnyAsync(
                x => x.AnalysisId == analysisId,
                cancellationToken);

        if (!analysisExists)
        {
            throw new InvalidOperationException(
                "No se encontró el análisis.");
        }

        var existingSharedResult =
            await _dbContext.SharedResults
                .SingleOrDefaultAsync(
                    x => x.AnalysisId == analysisId,
                    cancellationToken);

        var now = DateTime.UtcNow;

        /*
         * Si el enlace ya existe, está activo y no venció,
         * reutilizamos el mismo token.
         */
        if (existingSharedResult is not null &&
            existingSharedResult.IsActive &&
            existingSharedResult.ExpiresAtUtc > now)
        {
            return existingSharedResult.PublicToken;
        }

        var newToken = await GenerateUniqueTokenAsync(
            cancellationToken);

        /*
         * Si ya existe un registro vencido o desactivado,
         * lo actualizamos en lugar de insertar otro.
         */
        if (existingSharedResult is not null)
        {
            existingSharedResult.PublicToken = newToken;
            existingSharedResult.IsActive = true;
            existingSharedResult.CreatedAtUtc = now;
            existingSharedResult.ExpiresAtUtc =
                now.AddMonths(12);
            existingSharedResult.AccessCount = 0;
            existingSharedResult.LastAccessedAtUtc = null;

            await _dbContext.SaveChangesAsync(
                cancellationToken);

            return newToken;
        }

        /*
         * Solo se inserta un registro cuando el análisis
         * todavía no tiene ningún resultado compartido.
         */
        var sharedResult = new SharedResult
        {
            AnalysisId = analysisId,
            PublicToken = newToken,
            IsActive = true,
            CreatedAtUtc = now,
            ExpiresAtUtc = now.AddMonths(12),
            AccessCount = 0,
            LastAccessedAtUtc = null
        };

        _dbContext.SharedResults.Add(sharedResult);

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return newToken;
    }

    public async Task<Guid?> GetAnalysisIdAsync(
        string publicToken,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(publicToken))
        {
            return null;
        }

        var now = DateTime.UtcNow;

        var sharedResult =
            await _dbContext.SharedResults
                .SingleOrDefaultAsync(
                    x =>
                        x.PublicToken == publicToken &&
                        x.IsActive &&
                        x.ExpiresAtUtc > now,
                    cancellationToken);

        if (sharedResult is null)
        {
            return null;
        }

        sharedResult.AccessCount++;
        sharedResult.LastAccessedAtUtc = now;

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return sharedResult.AnalysisId;
    }

    private async Task<string> GenerateUniqueTokenAsync(
        CancellationToken cancellationToken)
    {
        string token;
        bool tokenExists;

        do
        {
            token = Convert.ToHexString(
                    RandomNumberGenerator.GetBytes(24))
                .ToLowerInvariant();

            tokenExists = await _dbContext.SharedResults
                .AnyAsync(
                    x => x.PublicToken == token,
                    cancellationToken);
        }
        while (tokenExists);

        return token;
    }
}