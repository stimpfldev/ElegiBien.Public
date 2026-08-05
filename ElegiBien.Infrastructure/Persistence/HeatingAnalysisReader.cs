using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Infrastructure.Persistence;

public class HeatingAnalysisReader : IHeatingAnalysisReader
{
    private readonly ElegiBienDbContext _dbContext;

    public HeatingAnalysisReader(ElegiBienDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<HeatingCalculationResult?> GetCalculationResultAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default) =>
        _dbContext.HeatingCalculationResults
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.AnalysisId == analysisId, cancellationToken);
}
