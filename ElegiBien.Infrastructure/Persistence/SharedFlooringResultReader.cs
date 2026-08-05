using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Enums;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Infrastructure.Persistence;

public class SharedFlooringResultReader : ISharedFlooringResultReader
{
    private readonly ElegiBienDbContext _dbContext;

    public SharedFlooringResultReader(ElegiBienDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SharedFlooringResultDto?> GetAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default)
    {
        var calculation = await _dbContext.FlooringCalculationResults
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.AnalysisId == analysisId, cancellationToken);

        if (calculation is null)
        {
            return null;
        }

        var alternatives = await GenericComparisonReader.LoadAsync(
            _dbContext,
            analysisId,
            CategoryCode.CeramicAndFlooring,
            cancellationToken);

        var products = alternatives
            .Where(x => x.Score is not null)
            .Select(x =>
            {
                var score = x.Score!;
                return new FlooringProductScoreResultDto
                {
                    ProductName = x.Name,
                    TotalScore = decimal.ToInt32(score.TotalScore),
                    CoverageStatus = GenericComparisonReader.GetStatus(
                        score,
                        FlooringCoverageStatus.Insufficient),
                    ConfidenceLevel = GenericComparisonReader.GetEnum(
                        score.DetailsJson,
                        "ConfidenceLevel",
                        ConfidenceLevel.Medium),
                    BoxesRequired = GenericComparisonReader.GetInt(
                        score.DetailsJson,
                        "BoxesRequired"),
                    RequiredAreaSquareMeters = GenericComparisonReader.GetDecimal(
                        score.DetailsJson,
                        "RequiredAreaSquareMeters"),
                    PurchasedAreaSquareMeters = GenericComparisonReader.GetDecimal(
                        score.DetailsJson,
                        "PurchasedAreaSquareMeters"),
                    ExcessAreaSquareMeters = GenericComparisonReader.GetDecimal(
                        score.DetailsJson,
                        "ExcessAreaSquareMeters"),
                    ExcessPercentage = GenericComparisonReader.GetDecimal(
                        score.DetailsJson,
                        "ExcessPercentage"),
                    TotalCost = x.TotalCost ?? GenericComparisonReader.GetDecimal(
                        score.DetailsJson,
                        "TotalCost"),
                    Factors = score.Factors
                        .Select(factor => new FlooringScoreFactorDto
                        {
                            FactorType = ParseFactor<FlooringScoreFactorType>(factor.FactorCode),
                            Score = factor.Score,
                            MaximumScore = factor.MaximumScore,
                            Explanation = factor.Explanation
                        })
                        .ToList()
                };
            })
            .OrderByDescending(x => x.TotalScore)
            .ToList();

        return new SharedFlooringResultDto
        {
            TotalAreaSquareMeters = calculation.TotalAreaSquareMeters,
            WastePercentage = calculation.WastePercentage,
            RequiredAreaSquareMeters = calculation.RequiredAreaSquareMeters,
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
        IReadOnlyList<FlooringProductScoreResultDto> products)
    {
        if (products.Count == 0)
        {
            return "Todavía no se compararon productos para este análisis.";
        }

        return products.Count > 1 &&
               Math.Abs(products[0].TotalScore - products[1].TotalScore) <= 3
            ? "Los productos presentan un empate técnico."
            : $"ElegíBien recomienda {products[0].ProductName}.";
    }
}
