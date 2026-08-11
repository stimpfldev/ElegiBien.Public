using ElegiBien.Domain.Entities;

namespace ElegiBien.Application.Interfaces;

public interface IHeatingAnalysisReader
{
    Task<HeatingCalculationResult?> GetCalculationResultAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default);
}
