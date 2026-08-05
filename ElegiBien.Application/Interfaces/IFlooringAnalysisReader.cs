using ElegiBien.Domain.Entities;

namespace ElegiBien.Application.Interfaces;

public interface IFlooringAnalysisReader
{
    Task<FlooringInput?> GetInputAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default);

    Task<FlooringCalculationResult?> GetCalculationResultAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default);
}
