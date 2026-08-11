using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Infrastructure.Persistence;

public class PaintAnalysisReader : IPaintAnalysisReader
{
    private readonly ElegiBienDbContext _dbContext;
    public PaintAnalysisReader(ElegiBienDbContext dbContext) => _dbContext = dbContext;

    public Task<PaintInput?> GetInputAsync(Guid analysisId, CancellationToken cancellationToken = default) =>
        _dbContext.PaintInputs.AsNoTracking().SingleOrDefaultAsync(x => x.AnalysisId == analysisId, cancellationToken);

    public Task<PaintCalculationResult?> GetCalculationResultAsync(Guid analysisId, CancellationToken cancellationToken = default) =>
        _dbContext.PaintCalculationResults.AsNoTracking().SingleOrDefaultAsync(x => x.AnalysisId == analysisId, cancellationToken);
}
