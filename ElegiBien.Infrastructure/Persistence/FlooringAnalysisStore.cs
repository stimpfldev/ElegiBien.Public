using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Infrastructure.Persistence;

public class FlooringAnalysisStore : IFlooringAnalysisStore
{
    private readonly ElegiBienDbContext _dbContext;

    public FlooringAnalysisStore(ElegiBienDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> SaveAsync(
        FlooringInput input,
        FlooringCalculationResult result,
        AnalysisMode mode,
        CancellationToken cancellationToken = default)
    {
        var category = await _dbContext.Categories.SingleAsync(
            x => x.Code == CategoryCode.CeramicAndFlooring && x.IsActive,
            cancellationToken);

        var methodology = await _dbContext.MethodologyVersions.SingleAsync(
            x => x.CategoryId == category.CategoryId && x.IsActive,
            cancellationToken);

        var analysis = new Analysis
        {
            AnalysisId = input.AnalysisId,
            CategoryId = category.CategoryId,
            MethodologyVersionId = methodology.MethodologyVersionId,
            Mode = mode,
            ConfidenceLevel = result.ConfidenceLevel,
            CreatedAtUtc = DateTime.UtcNow,
            CompletedAtUtc = DateTime.UtcNow,
            IsCompleted = true,
            FlooringInput = input,
            FlooringCalculationResult = result
        };

        _dbContext.Analyses.Add(analysis);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return analysis.AnalysisId;
    }
}
