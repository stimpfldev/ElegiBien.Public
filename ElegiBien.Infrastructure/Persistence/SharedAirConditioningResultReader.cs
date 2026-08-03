using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Infrastructure.Persistence;

public class SharedAirConditioningResultReader
    : ISharedAirConditioningResultReader
{
    private readonly ElegiBienDbContext _dbContext;

    public SharedAirConditioningResultReader(
        ElegiBienDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SharedAirConditioningResultDto?> GetAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default)
    {
        var dimensioning = await _dbContext.DimensioningResults
            .AsNoTracking()
            .SingleOrDefaultAsync(
                x => x.AnalysisId == analysisId,
                cancellationToken);

        if (dimensioning is null)
        {
            return null;
        }

        var alternatives = await _dbContext.ProductAlternatives
            .AsNoTracking()
            .Where(x => x.AnalysisId == analysisId)
            .Include(x => x.ProductScore!)
                .ThenInclude(x => x.Factors)
            .ToListAsync(cancellationToken);

        var products = alternatives
            .Where(x => x.ProductScore is not null)
            .Select(x => new ProductScoreResultDto
            {
                ProductName = x.Name,
                TotalScore = x.ProductScore!.TotalScore,
                CapacityStatus =
                    x.ProductScore.CapacityStatus,
                ConfidenceLevel =
                    x.ProductScore.ConfidenceLevel,
                IsEligible =
                    x.ProductScore.IsEligible,
                AppliedMaximumScore =
                    x.ProductScore.AppliedMaximumScore,
                Factors = x.ProductScore.Factors
                    .Select(factor => new ScoreFactorDto
                    {
                        FactorType = factor.FactorType,
                        Score = factor.Score,
                        MaximumScore =
                            factor.MaximumScore,
                        Explanation =
                            factor.Explanation
                    })
                    .ToList()
            })
            .ToList();

        var recommendation = BuildRecommendation(products);

        return new SharedAirConditioningResultDto
        {
            RecommendedMinimumFrigories =
                dimensioning.RecommendedMinimumFrigories,
            RecommendedMaximumFrigories =
                dimensioning.RecommendedMaximumFrigories,
            IdealFrigories =
                dimensioning.IdealFrigories,
            Products = products,
            Recommendation = recommendation
        };
    }

    private static string BuildRecommendation(
        IReadOnlyCollection<ProductScoreResultDto> products)
    {
        if (products.Count == 0)
        {
            return
                "Todavía no se compararon productos para este análisis.";
        }

        var eligible = products
            .Where(x => x.IsEligible)
            .OrderByDescending(x => x.TotalScore)
            .ToList();

        if (eligible.Count == 0)
        {
            return
                "Ninguna de las alternativas analizadas se adapta correctamente a la capacidad necesaria.";
        }

        if (eligible.Count == 1)
        {
            return
                $"ElegíBien recomienda {eligible[0].ProductName}.";
        }

        var difference =
            Math.Abs(
                eligible[0].TotalScore -
                eligible[1].TotalScore);

        if (difference <= 3)
        {
            return
                "Las alternativas presentan un empate técnico.";
        }

        return
            $"ElegíBien recomienda {eligible[0].ProductName}.";
    }
}