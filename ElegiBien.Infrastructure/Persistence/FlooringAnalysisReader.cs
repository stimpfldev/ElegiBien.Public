using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Infrastructure.Persistence;

public class FlooringAnalysisReader : IFlooringAnalysisReader
{
    private readonly ElegiBienDbContext _dbContext;

    public FlooringAnalysisReader(ElegiBienDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<FlooringInput?> GetInputAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default) =>
        _dbContext.FlooringInputs
            .AsNoTracking()
            .SingleOrDefaultAsync(
                x => x.AnalysisId == analysisId,
                cancellationToken);

    public Task<FlooringCalculationResult?> GetCalculationResultAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default) =>
        _dbContext.FlooringCalculationResults
            .AsNoTracking()
            .SingleOrDefaultAsync(
                x => x.AnalysisId == analysisId,
                cancellationToken);
}
