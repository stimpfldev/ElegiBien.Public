using ElegiBien.Application.DTOs;

namespace ElegiBien.Application.Interfaces;

public interface IHeatingComparisonStore
{
    Task SaveAsync(
        Guid analysisId,
        HeatingProductAlternativeDto firstProduct,
        HeatingProductAlternativeDto secondProduct,
        HeatingComparisonResultDto result,
        CancellationToken cancellationToken = default);
}
