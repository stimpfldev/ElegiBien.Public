using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Infrastructure.Persistence;

public class HeatingAnalysisStore : IHeatingAnalysisStore
{
    private readonly ElegiBienDbContext _dbContext;

    public HeatingAnalysisStore(ElegiBienDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> SaveAsync(
        HeatingInput input,
        HeatingCalculationResult result,
        AnalysisMode mode,
        CancellationToken cancellationToken = default)
    {
        var category = await _dbContext.Categories.SingleAsync(
            x => x.Code == CategoryCode.Heating && x.IsActive,
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
            HeatingInput = input,
            HeatingCalculationResult = result
        };

        _dbContext.Analyses.Add(analysis);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return analysis.AnalysisId;
    }
}
