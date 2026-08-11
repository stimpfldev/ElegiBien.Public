using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.Interfaces;

public interface IAirConditioningAnalysisStore
{
    Task<Guid> SaveAsync(
        AirConditioningInput input,
        DimensioningResult result,
        AnalysisMode mode,
        CancellationToken cancellationToken = default);
}