using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Infrastructure.Persistence;

public class AirConditioningAnalysisReader
    : IAirConditioningAnalysisReader
{
    private readonly ElegiBienDbContext _dbContext;

    public AirConditioningAnalysisReader(
        ElegiBienDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<DimensioningResult?> GetDimensioningResultAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.DimensioningResults
            .AsNoTracking()
            .SingleOrDefaultAsync(
                x => x.AnalysisId == analysisId,
                cancellationToken);
    }
}