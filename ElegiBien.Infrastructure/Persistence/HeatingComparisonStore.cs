using System.Globalization;
using System.Text.Json;
using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Enums;
using ElegiBien.Infrastructure.Data;

namespace ElegiBien.Infrastructure.Persistence;

public class HeatingComparisonStore : IHeatingComparisonStore
{
    private readonly ElegiBienDbContext _dbContext;

    public HeatingComparisonStore(ElegiBienDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveAsync(
        Guid analysisId,
        HeatingProductAlternativeDto firstProduct,
        HeatingProductAlternativeDto secondProduct,
        HeatingComparisonResultDto result,
        CancellationToken cancellationToken = default)
    {
        return GenericComparisonPersistence.ReplaceAsync(
            _dbContext,
            analysisId,
            CategoryCode.Heating,
            [
                CreateAlternative(1, firstProduct, result.FirstProduct),
                CreateAlternative(2, secondProduct, result.SecondProduct)
            ],
            cancellationToken);
    }

    private static GenericComparisonAlternativeData CreateAlternative(
        int position,
        HeatingProductAlternativeDto product,
        HeatingProductScoreResultDto score)
    {
        return new GenericComparisonAlternativeData(
            position,
            product.Name,
            product.PurchasePrice,
            JsonSerializer.Serialize(new
            {
                product.SystemType,
                product.HeatingCapacityWatts,
                product.PurchasePrice,
                product.EstimatedHourlyCost,
                product.EfficiencyLevel,
                product.SafetyLevel
            }),
            score.TotalScore,
            score.AppliedMaximumScore,
            score.IsEligible,
            Convert.ToInt32(score.CapacityStatus, CultureInfo.InvariantCulture)
                .ToString(CultureInfo.InvariantCulture),
            JsonSerializer.Serialize(new
            {
                score.CapacityStatus,
                score.IsEligible
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
