using ElegiBien.Domain.Entities;

namespace ElegiBien.Application.Interfaces;

public interface IPaintAnalysisReader
{
    Task<PaintInput?> GetInputAsync(Guid analysisId, CancellationToken cancellationToken = default);
    Task<PaintCalculationResult?> GetCalculationResultAsync(Guid analysisId, CancellationToken cancellationToken = default);
}
