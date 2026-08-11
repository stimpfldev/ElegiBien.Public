using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.Interfaces;

public interface IFlooringAnalysisStore
{
    Task<Guid> SaveAsync(
        FlooringInput input,
        FlooringCalculationResult result,
        AnalysisMode mode,
        CancellationToken cancellationToken = default);
}
