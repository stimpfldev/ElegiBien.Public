using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Enums;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Infrastructure.Persistence;

public class SharedHeatingResultReader : ISharedHeatingResultReader
{
    private readonly ElegiBienDbContext _dbContext;

    public SharedHeatingResultReader(ElegiBienDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SharedHeatingResultDto?> GetAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default)
    {
        var calculation = await _dbContext.HeatingCalculationResults
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.AnalysisId == analysisId, cancellationToken);

        if (calculation is null)
        {
            return null;
        }

        var alternatives = await GenericComparisonReader.LoadAsync(
            _dbContext,
            analysisId,
            CategoryCode.Heating,
            cancellationToken);

        var products = alternatives
            .Where(x => x.Score is not null)
            .Select(x =>
            {
                var score = x.Score!;
                return new HeatingProductScoreResultDto
                {
                    ProductName = x.Name,
                    TotalScore = decimal.ToInt32(score.TotalScore),
                    CapacityStatus = GenericComparisonReader.GetStatus(
                        score,
                        HeatingCapacityStatus.Insufficient),
                    IsEligible = score.IsEligible,
                    AppliedMaximumScore = score.AppliedMaximumScore,
                    Factors = score.Factors
                        .Select(factor => new HeatingScoreFactorDto
                        {
                            FactorType = ParseFactor<HeatingScoreFactorType>(factor.FactorCode),
                            Score = factor.Score,
                            MaximumScore = factor.MaximumScore,
                            Explanation = factor.Explanation
                        })
                        .ToList()
                };
            })
            .OrderByDescending(x => x.TotalScore)
            .ToList();

        return new SharedHeatingResultDto
        {
            SurfaceSquareMeters = calculation.SurfaceSquareMeters,
            VolumeCubicMeters = calculation.VolumeCubicMeters,
            RecommendedMinimumWatts = calculation.RecommendedMinimumWatts,
            RecommendedMaximumWatts = calculation.RecommendedMaximumWatts,
            IdealPowerWatts = calculation.IdealPowerWatts,
            IdealPowerKcalPerHour = calculation.IdealPowerKcalPerHour,
            Recommendation = BuildRecommendation(products),
            Products = products
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
        IReadOnlyList<HeatingProductScoreResultDto> products)
    {
        if (products.Count == 0)
        {
            return "Todavía no se compararon equipos para este análisis.";
        }

        return products.Count > 1 &&
               Math.Abs(products[0].TotalScore - products[1].TotalScore) <= 3
            ? "Los equipos presentan un empate técnico."
            : $"ElegíBien recomienda {products[0].ProductName}.";
    }
}
