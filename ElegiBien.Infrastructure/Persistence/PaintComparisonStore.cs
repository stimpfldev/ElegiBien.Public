using System.Globalization;
using System.Text.Json;
using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Enums;
using ElegiBien.Infrastructure.Data;

namespace ElegiBien.Infrastructure.Persistence;

public class PaintComparisonStore : IPaintComparisonStore
{
    private readonly ElegiBienDbContext _dbContext;

    public PaintComparisonStore(ElegiBienDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveAsync(
        Guid analysisId,
        PaintProductAlternativeDto firstProduct,
        PaintProductAlternativeDto secondProduct,
        PaintComparisonResultDto result,
        CancellationToken cancellationToken = default)
    {
        return GenericComparisonPersistence.ReplaceAsync(
            _dbContext,
            analysisId,
            CategoryCode.Paint,
            [
                CreateAlternative(1, firstProduct, result.FirstProduct),
                CreateAlternative(2, secondProduct, result.SecondProduct)
            ],
            cancellationToken);
    }

    private static GenericComparisonAlternativeData CreateAlternative(
        int position,
        PaintProductAlternativeDto product,
        PaintProductScoreResultDto score)
    {
        return new GenericComparisonAlternativeData(
            position,
            product.Name,
            score.TotalCost,
            JsonSerializer.Serialize(new
            {
                product.ContainerLiters,
                product.PricePerContainer,
                product.CoverageSquareMetersPerLiterPerCoat,
                product.Washability,
                product.DryingHours
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
                score.ContainersRequired,
                score.LitersRequired,
                score.LitersPurchased,
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
