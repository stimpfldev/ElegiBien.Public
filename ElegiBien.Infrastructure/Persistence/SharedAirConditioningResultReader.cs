using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Enums;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Infrastructure.Persistence;

public class SharedAirConditioningResultReader : ISharedAirConditioningResultReader
{
    private readonly ElegiBienDbContext _dbContext;

    public SharedAirConditioningResultReader(ElegiBienDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SharedAirConditioningResultDto?> GetAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default)
    {
        var analysis = await _dbContext.Analyses
            .AsNoTracking()
            .Include(x => x.AirConditioningInput)
            .Include(x => x.DimensioningResult)
            .SingleOrDefaultAsync(x => x.AnalysisId == analysisId, cancellationToken);

        if (analysis?.AirConditioningInput is null || analysis.DimensioningResult is null)
        {
            return null;
        }

        var input = analysis.AirConditioningInput;
        var dimensioning = analysis.DimensioningResult;

        var alternatives = await GenericComparisonReader.LoadAsync(
            _dbContext,
            analysisId,
            CategoryCode.AirConditioning,
            cancellationToken);

        var products = alternatives
            .Where(x => x.Score is not null)
            .Select(x =>
            {
                var score = x.Score!;
                return new ProductScoreResultDto
                {
                    ProductName = x.Name,
                    TotalScore = decimal.ToInt32(score.TotalScore),
                    CapacityStatus = GenericComparisonReader.GetStatus(
                        score,
                        CapacityFitStatus.Insufficient),
                    ConfidenceLevel = GenericComparisonReader.GetEnum(
                        score.DetailsJson,
                        "ConfidenceLevel",
                        ConfidenceLevel.Medium),
                    IsEligible = score.IsEligible,
                    AppliedMaximumScore = score.AppliedMaximumScore.HasValue
                        ? decimal.ToInt32(score.AppliedMaximumScore.Value)
                        : null,
                    Factors = score.Factors
                        .Select(factor => new ScoreFactorDto
                        {
                            FactorType = ParseFactor<ScoreFactorType>(factor.FactorCode),
                            Score = factor.Score,
                            MaximumScore = factor.MaximumScore,
                            Explanation = factor.Explanation
                        })
                        .ToList()
                };
            })
            .ToList();

        return new SharedAirConditioningResultDto
        {
            RecommendedMinimumFrigories = dimensioning.RecommendedMinimumFrigories,
            RecommendedMaximumFrigories = dimensioning.RecommendedMaximumFrigories,
            IdealFrigories = dimensioning.IdealFrigories,
            SurfaceSquareMeters = input.LengthMeters * input.WidthMeters,
            VolumeCubicMeters = dimensioning.VolumeCubicMeters,
            ConfidenceLevel = dimensioning.ConfidenceLevel,
            RequiresProfessionalReview = dimensioning.RequiresProfessionalReview,
            Products = products,
            Recommendation = BuildRecommendation(products)
        };
    }

    private static TEnum ParseFactor<TEnum>(string value)
        where TEnum : struct, Enum
    {
        return int.TryParse(value, out var numeric) &&
               Enum.IsDefined(typeof(TEnum), numeric)
            ? (TEnum)Enum.ToObject(typeof(TEnum), numeric)
            : default;
    }

    private static string BuildRecommendation(
        IReadOnlyCollection<ProductScoreResultDto> products)
    {
        if (products.Count == 0)
        {
            return "Todavía no se compararon productos para este análisis.";
        }

        var eligible = products
            .Where(x => x.IsEligible)
            .OrderByDescending(x => x.TotalScore)
            .ToList();

        if (eligible.Count == 0)
        {
            return "Ninguna de las alternativas analizadas se adapta correctamente a la capacidad necesaria.";
        }

        if (eligible.Count == 1)
        {
            return $"ElegíBien recomienda {eligible[0].ProductName}.";
        }

        return Math.Abs(eligible[0].TotalScore - eligible[1].TotalScore) <= 3
            ? "Las alternativas presentan un empate técnico."
            : $"ElegíBien recomienda {eligible[0].ProductName}.";
    }
}
