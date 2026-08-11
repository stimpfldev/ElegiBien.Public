using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.Interfaces;

public interface IPaintAnalysisStore
{
    Task<Guid> SaveAsync(PaintInput input, PaintCalculationResult result, AnalysisMode mode, CancellationToken cancellationToken = default);
}
