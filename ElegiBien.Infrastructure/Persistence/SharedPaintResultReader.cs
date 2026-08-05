using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Enums;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Infrastructure.Persistence;

public class SharedPaintResultReader : ISharedPaintResultReader
{
    private readonly ElegiBienDbContext _dbContext;

    public SharedPaintResultReader(ElegiBienDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SharedPaintResultDto?> GetAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default)
    {
        var analysis = await _dbContext.Analyses
            .AsNoTracking()
            .Include(x => x.PaintInput)
            .Include(x => x.PaintCalculationResult)
            .SingleOrDefaultAsync(x => x.AnalysisId == analysisId, cancellationToken);

        if (analysis?.PaintInput is null || analysis.PaintCalculationResult is null)
        {
            return null;
        }

        var alternatives = await GenericComparisonReader.LoadAsync(
            _dbContext,
            analysisId,
            CategoryCode.Paint,
            cancellationToken);

        var products = alternatives
            .Where(x => x.Score is not null)
            .Select(x =>
            {
                var score = x.Score!;
                return new PaintProductScoreResultDto
                {
                    ProductName = x.Name,
                    TotalScore = decimal.ToInt32(score.TotalScore),
                    CoverageStatus = GenericComparisonReader.GetStatus(
                        score,
                        PaintCoverageStatus.Insufficient),
                    ConfidenceLevel = GenericComparisonReader.GetEnum(
                        score.DetailsJson,
                        "ConfidenceLevel",
                        ConfidenceLevel.Medium),
                    ContainersRequired = GenericComparisonReader.GetInt(
                        score.DetailsJson,
                        "ContainersRequired"),
                    LitersRequired = GenericComparisonReader.GetDecimal(
                        score.DetailsJson,
                        "LitersRequired"),
                    LitersPurchased = GenericComparisonReader.GetDecimal(
                        score.DetailsJson,
                        "LitersPurchased"),
                    TotalCost = x.TotalCost ?? GenericComparisonReader.GetDecimal(
                        score.DetailsJson,
                        "TotalCost"),
                    Factors = score.Factors
                        .Select(factor => new PaintScoreFactorDto
                        {
                            FactorType = ParseFactor<PaintScoreFactorType>(factor.FactorCode),
                            Score = factor.Score,
                            MaximumScore = factor.MaximumScore,
                            Explanation = factor.Explanation
                        })
                        .ToList()
                };
            })
            .OrderByDescending(x => x.TotalScore)
            .ToList();

        return new SharedPaintResultDto
        {
            NetAreaSquareMeters = analysis.PaintCalculationResult.NetAreaSquareMeters,
            ReferenceLiters = analysis.PaintCalculationResult.ReferenceLiters,
            CoatCount = analysis.PaintInput.CoatCount,
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
        IReadOnlyList<PaintProductScoreResultDto> products)
    {
        if (products.Count == 0)
        {
            return "Todavía no se compararon pinturas para este análisis.";
        }

        return products.Count > 1 &&
               Math.Abs(products[0].TotalScore - products[1].TotalScore) <= 3
            ? "Las pinturas presentan un empate técnico."
            : $"ElegíBien recomienda {products[0].ProductName}.";
    }
}
