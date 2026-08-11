using ElegiBien.Application.DTOs;

namespace ElegiBien.Application.Interfaces;

public interface ISharedFlooringResultReader
{
    Task<SharedFlooringResultDto?> GetAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default);
}
