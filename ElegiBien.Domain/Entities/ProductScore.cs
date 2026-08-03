using ElegiBien.Domain.Enums;

namespace ElegiBien.Domain.Entities;

public class ProductScore
{
    public Guid ProductScoreId { get; set; } = Guid.NewGuid();

    public Guid ProductAlternativeId { get; set; }

    public int TotalScore { get; set; }

    public int? AppliedMaximumScore { get; set; }

    public CapacityFitStatus CapacityStatus { get; set; }

    public ConfidenceLevel ConfidenceLevel { get; set; }

    public bool IsEligible { get; set; }

    public ProductAlternative ProductAlternative { get; set; } = null!;

    public ICollection<ScoreFactor> Factors { get; set; } =
        new List<ScoreFactor>();
}