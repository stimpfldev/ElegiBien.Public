using ElegiBien.Application.Interfaces;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Infrastructure.Persistence;

public sealed class ExpiredSharedResultCleanupService
    : IExpiredSharedResultCleanupService
{
    private readonly ElegiBienDbContext _dbContext;

    public ExpiredSharedResultCleanupService(
        ElegiBienDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CleanupAsync(
        DateTime utcNow,
        CancellationToken cancellationToken = default)
    {
        var expired = await _dbContext.SharedResults
            .Where(x => x.ExpiresAtUtc <= utcNow)
            .ToListAsync(cancellationToken);

        if (expired.Count == 0)
        {
            return 0;
        }

        _dbContext.SharedResults.RemoveRange(expired);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return expired.Count;
    }
}
