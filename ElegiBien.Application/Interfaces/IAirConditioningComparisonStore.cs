using ElegiBien.Application.DTOs;

namespace ElegiBien.Application.Interfaces;

public interface IAirConditioningComparisonStore
{
    Task SaveAsync(
        Guid analysisId,
        ProductAlternativeDto firstProduct,
        ProductAlternativeDto secondProduct,
        ProductComparisonResultDto comparisonResult,
        CancellationToken cancellationToken = default);
}