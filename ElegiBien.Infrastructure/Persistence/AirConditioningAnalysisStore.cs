using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Infrastructure.Persistence;

public class AirConditioningAnalysisStore
    : IAirConditioningAnalysisStore
{
    private readonly ElegiBienDbContext _dbContext;

    public AirConditioningAnalysisStore(
        ElegiBienDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> SaveAsync(
        AirConditioningInput input,
        DimensioningResult result,
        AnalysisMode mode,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(result);

        var category = await _dbContext.Categories
            .SingleAsync(
                x =>
                    x.Code == CategoryCode.AirConditioning &&
                    x.IsActive,
                cancellationToken);

        var methodology = await _dbContext.MethodologyVersions
            .SingleAsync(
                x =>
                    x.CategoryId == category.CategoryId &&
                    x.IsActive,
                cancellationToken);

        var analysisId = input.AnalysisId;

        if (analysisId == Guid.Empty)
        {
            analysisId = Guid.NewGuid();
            input.AnalysisId = analysisId;
            result.AnalysisId = analysisId;
        }

        var analysis = new Analysis
        {
            AnalysisId = analysisId,
            CategoryId = category.CategoryId,
            MethodologyVersionId =
                methodology.MethodologyVersionId,
            Mode = mode,
            ConfidenceLevel = result.ConfidenceLevel,
            CreatedAtUtc = DateTime.UtcNow,
            CompletedAtUtc = DateTime.UtcNow,
            IsCompleted = true,
            AirConditioningInput = input,
            DimensioningResult = result
        };

        _dbContext.Analyses.Add(analysis);

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return analysis.AnalysisId;
    }
}