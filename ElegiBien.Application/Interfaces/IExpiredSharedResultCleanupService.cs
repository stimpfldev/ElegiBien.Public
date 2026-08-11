namespace ElegiBien.Application.Interfaces;

public interface IExpiredSharedResultCleanupService
{
    Task<int> CleanupAsync(
        DateTime utcNow,
        CancellationToken cancellationToken = default);
}
