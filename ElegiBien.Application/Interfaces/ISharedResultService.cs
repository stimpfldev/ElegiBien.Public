namespace ElegiBien.Application.Interfaces;

public interface ISharedResultService
{
    Task<string> CreateOrGetTokenAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default);

    Task<Guid?> GetAnalysisIdAsync(
        string publicToken,
        CancellationToken cancellationToken = default);
}