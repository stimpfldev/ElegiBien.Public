using ElegiBien.Application.DTOs;

namespace ElegiBien.Application.Interfaces;

public interface ISharedAirConditioningResultReader
{
    Task<SharedAirConditioningResultDto?> GetAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default);
}