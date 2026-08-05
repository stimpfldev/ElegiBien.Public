using ElegiBien.Application.DTOs;

namespace ElegiBien.Application.Interfaces;

public interface ISharedPaintResultReader
{
    Task<SharedPaintResultDto?> GetAsync(Guid analysisId, CancellationToken cancellationToken = default);
}
