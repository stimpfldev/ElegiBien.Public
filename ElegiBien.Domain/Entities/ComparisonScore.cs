namespace ElegiBien.Domain.Entities;

public class ComparisonScore
{
    public Guid ComparisonScoreId { get; set; } = Guid.NewGuid();
    public Guid ComparisonAlternativeId { get; set; }
    public decimal TotalScore { get; set; }
    public decimal? AppliedMaximumScore { get; set; }
    public bool IsEligible { get; set; }
    public string? StatusCode { get; set; }
    public string DetailsJson { get; set; } = "{}";

    public ComparisonAlternative Alternative { get; set; } = null!;
    public ICollection<ComparisonFactor> Factors { get; set; } = new List<ComparisonFactor>();
}
