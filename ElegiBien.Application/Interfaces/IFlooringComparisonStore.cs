using ElegiBien.Application.DTOs;

namespace ElegiBien.Application.Interfaces;

public interface IFlooringComparisonStore
{
    Task SaveAsync(
        Guid analysisId,
        FlooringProductAlternativeDto firstProduct,
        FlooringProductAlternativeDto secondProduct,
        FlooringComparisonResultDto result,
        CancellationToken cancellationToken = default);
}
