using ElegiBien.Application.DTOs;

namespace ElegiBien.Application.Interfaces;

public interface IPaintComparisonStore
{
    Task SaveAsync(Guid analysisId, PaintProductAlternativeDto firstProduct, PaintProductAlternativeDto secondProduct, PaintComparisonResultDto result, CancellationToken cancellationToken = default);
}
