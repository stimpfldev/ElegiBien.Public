using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Infrastructure.Persistence;

public class AirConditioningComparisonStore
    : IAirConditioningComparisonStore
{
    private readonly ElegiBienDbContext _dbContext;

    public AirConditioningComparisonStore(
        ElegiBienDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveAsync(
        Guid analysisId,
        ProductAlternativeDto firstProduct,
        ProductAlternativeDto secondProduct,
        ProductComparisonResultDto comparisonResult,
        CancellationToken cancellationToken = default)
    {
        var analysisExists = await _dbContext.Analyses
            .AnyAsync(
                x => x.AnalysisId == analysisId,
                cancellationToken);

        if (!analysisExists)
        {
            throw new InvalidOperationException(
                "No se encontró el análisis asociado.");
        }

        var existingAlternatives =
            await _dbContext.ProductAlternatives
                .Where(x => x.AnalysisId == analysisId)
                .Include(x => x.ProductScore!)
                    .ThenInclude(x => x.Factors)
                .ToListAsync(cancellationToken);

        if (existingAlternatives.Count > 0)
        {
            _dbContext.ProductAlternatives.RemoveRange(
                existingAlternatives);

            await _dbContext.SaveChangesAsync(
                cancellationToken);
        }

        var firstEntity = CreateAlternative(
            analysisId,
            firstProduct,
            comparisonResult.FirstProduct);

        var secondEntity = CreateAlternative(
            analysisId,
            secondProduct,
            comparisonResult.SecondProduct);

        _dbContext.ProductAlternatives.AddRange(
            firstEntity,
            secondEntity);

        await _dbContext.SaveChangesAsync(
            cancellationToken);
    }

    private static ProductAlternative CreateAlternative(
        Guid analysisId,
        ProductAlternativeDto product,
        ProductScoreResultDto scoreResult)
    {
        var alternativeId = Guid.NewGuid();
        var productScoreId = Guid.NewGuid();

        return new ProductAlternative
        {
            ProductAlternativeId = alternativeId,
            AnalysisId = analysisId,
            Name = product.Name.Trim(),
            CapacityFrigories = product.CapacityFrigories,
            Price = product.Price,
            Technology = product.Technology,
            NominalConsumptionWatts =
                product.NominalConsumptionWatts,
            WarrantyMonths = product.WarrantyMonths,
            ProductScore = new ProductScore
            {
                ProductScoreId = productScoreId,
                ProductAlternativeId = alternativeId,
                TotalScore = scoreResult.TotalScore,
                AppliedMaximumScore =
                    scoreResult.AppliedMaximumScore,
                CapacityStatus =
                    scoreResult.CapacityStatus,
                ConfidenceLevel =
                    scoreResult.ConfidenceLevel,
                IsEligible =
                    scoreResult.IsEligible,
                Factors = scoreResult.Factors
                    .Select(
                        factor => new ScoreFactor
                        {
                            ScoreFactorId = Guid.NewGuid(),
                            ProductScoreId = productScoreId,
                            FactorType = factor.FactorType,
                            Score = factor.Score,
                            MaximumScore =
                                factor.MaximumScore,
                            Explanation =
                                factor.Explanation
                        })
                    .ToList()
            }
        };
    }
}