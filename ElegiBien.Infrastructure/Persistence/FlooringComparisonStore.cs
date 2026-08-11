using System.Globalization;
using System.Text.Json;
using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Enums;
using ElegiBien.Infrastructure.Data;

namespace ElegiBien.Infrastructure.Persistence;

public class FlooringComparisonStore : IFlooringComparisonStore
{
    private readonly ElegiBienDbContext _dbContext;

    public FlooringComparisonStore(ElegiBienDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveAsync(
        Guid analysisId,
        FlooringProductAlternativeDto firstProduct,
        FlooringProductAlternativeDto secondProduct,
        FlooringComparisonResultDto result,
        CancellationToken cancellationToken = default)
    {
        return GenericComparisonPersistence.ReplaceAsync(
            _dbContext,
            analysisId,
            CategoryCode.CeramicAndFlooring,
            [
                CreateAlternative(1, firstProduct, result.FirstProduct),
                CreateAlternative(2, secondProduct, result.SecondProduct)
            ],
            cancellationToken);
    }

    private static GenericComparisonAlternativeData CreateAlternative(
        int position,
        FlooringProductAlternativeDto product,
        FlooringProductScoreResultDto score)
    {
        return new GenericComparisonAlternativeData(
            position,
            product.Name,
            score.TotalCost,
            JsonSerializer.Serialize(new
            {
                product.CoverageSquareMetersPerBox,
                product.PricePerBox,
                product.UseResistance,
                product.ReplacementEase
            }),
            score.TotalScore,
            null,
            true,
            Convert.ToInt32(score.CoverageStatus, CultureInfo.InvariantCulture)
                .ToString(CultureInfo.InvariantCulture),
            JsonSerializer.Serialize(new
            {
                score.CoverageStatus,
                score.ConfidenceLevel,
                score.BoxesRequired,
                score.RequiredAreaSquareMeters,
                score.PurchasedAreaSquareMeters,
                score.ExcessAreaSquareMeters,
                score.ExcessPercentage,
                score.TotalCost
            }),
            score.Factors.Select(factor => new GenericComparisonFactorData(
                Convert.ToInt32(factor.FactorType, CultureInfo.InvariantCulture)
                    .ToString(CultureInfo.InvariantCulture),
                factor.FactorType.ToString(),
                factor.Score,
                factor.MaximumScore,
                factor.Explanation)).ToList());
    }
}
