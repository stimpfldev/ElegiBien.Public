using System.Globalization;
using System.Text.Json;
using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Enums;
using ElegiBien.Infrastructure.Data;

namespace ElegiBien.Infrastructure.Persistence;

public class AirConditioningComparisonStore : IAirConditioningComparisonStore
{
    private readonly ElegiBienDbContext _dbContext;

    public AirConditioningComparisonStore(ElegiBienDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveAsync(
        Guid analysisId,
        ProductAlternativeDto firstProduct,
        ProductAlternativeDto secondProduct,
        ProductComparisonResultDto comparisonResult,
        CancellationToken cancellationToken = default)
    {
        return GenericComparisonPersistence.ReplaceAsync(
            _dbContext,
            analysisId,
            CategoryCode.AirConditioning,
            [
                CreateAlternative(1, firstProduct, comparisonResult.FirstProduct),
                CreateAlternative(2, secondProduct, comparisonResult.SecondProduct)
            ],
            cancellationToken);
    }

    private static GenericComparisonAlternativeData CreateAlternative(
        int position,
        ProductAlternativeDto product,
        ProductScoreResultDto score)
    {
        return new GenericComparisonAlternativeData(
            position,
            product.Name,
            product.Price,
            JsonSerializer.Serialize(new
            {
                product.CapacityFrigories,
                product.Price,
                product.Technology,
                product.NominalConsumptionWatts,
                product.WarrantyMonths
            }),
            score.TotalScore,
            score.AppliedMaximumScore,
            score.IsEligible,
            Convert.ToInt32(score.CapacityStatus, CultureInfo.InvariantCulture)
                .ToString(CultureInfo.InvariantCulture),
            JsonSerializer.Serialize(new
            {
                score.CapacityStatus,
                score.ConfidenceLevel
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
