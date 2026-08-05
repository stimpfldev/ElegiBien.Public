using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Infrastructure.Persistence;

internal sealed record GenericComparisonFactorData(
    string FactorCode,
    string Label,
    decimal Score,
    decimal MaximumScore,
    string Explanation,
    decimal? Weight = null);

internal sealed record GenericComparisonAlternativeData(
    int Position,
    string Name,
    decimal? TotalCost,
    string DetailsJson,
    decimal TotalScore,
    decimal? AppliedMaximumScore,
    bool IsEligible,
    string? StatusCode,
    string ScoreDetailsJson,
    IReadOnlyCollection<GenericComparisonFactorData> Factors);

internal static class GenericComparisonPersistence
{
    public static async Task ReplaceAsync(
        ElegiBienDbContext dbContext,
        Guid analysisId,
        CategoryCode categoryCode,
        IReadOnlyCollection<GenericComparisonAlternativeData> alternatives,
        CancellationToken cancellationToken)
    {
        var analysisExists = await dbContext.Analyses
            .AnyAsync(x => x.AnalysisId == analysisId, cancellationToken);

        if (!analysisExists)
        {
            throw new InvalidOperationException("No se encontró el análisis asociado.");
        }

        var existing = await dbContext.ComparisonAlternatives
            .Where(x => x.AnalysisId == analysisId && x.CategoryCode == categoryCode)
            .Include(x => x.Score!)
                .ThenInclude(x => x.Factors)
            .ToListAsync(cancellationToken);

        if (existing.Count > 0)
        {
            dbContext.ComparisonAlternatives.RemoveRange(existing);
        }

        foreach (var source in alternatives)
        {
            var alternativeId = Guid.NewGuid();
            var scoreId = Guid.NewGuid();

            dbContext.ComparisonAlternatives.Add(new ComparisonAlternative
            {
                ComparisonAlternativeId = alternativeId,
                AnalysisId = analysisId,
                CategoryCode = categoryCode,
                Position = source.Position,
                Name = source.Name.Trim(),
                TotalCost = source.TotalCost,
                DetailsJson = source.DetailsJson,
                Score = new ComparisonScore
                {
                    ComparisonScoreId = scoreId,
                    ComparisonAlternativeId = alternativeId,
                    TotalScore = source.TotalScore,
                    AppliedMaximumScore = source.AppliedMaximumScore,
                    IsEligible = source.IsEligible,
                    StatusCode = source.StatusCode,
                    DetailsJson = source.ScoreDetailsJson,
                    Factors = source.Factors.Select(factor => new ComparisonFactor
                    {
                        ComparisonFactorId = Guid.NewGuid(),
                        ComparisonScoreId = scoreId,
                        FactorCode = factor.FactorCode,
                        Label = factor.Label,
                        Score = factor.Score,
                        MaximumScore = factor.MaximumScore,
                        Weight = factor.Weight,
                        Explanation = factor.Explanation
                    }).ToList()
                }
            });
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
