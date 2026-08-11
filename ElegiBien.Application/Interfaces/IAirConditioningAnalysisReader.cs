using ElegiBien.Domain.Entities;

namespace ElegiBien.Application.Interfaces;

public interface IAirConditioningAnalysisReader
{
    Task<DimensioningResult?> GetDimensioningResultAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default);
}