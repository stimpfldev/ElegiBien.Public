using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.Interfaces;

public interface IHeatingAnalysisStore
{
    Task<Guid> SaveAsync(
        HeatingInput input,
        HeatingCalculationResult result,
        AnalysisMode mode,
        CancellationToken cancellationToken = default);
}
