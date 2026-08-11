using ElegiBien.Application.DTOs;

namespace ElegiBien.Application.Interfaces;

public interface ISharedHeatingResultReader
{
    Task<SharedHeatingResultDto?> GetAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default);
}
