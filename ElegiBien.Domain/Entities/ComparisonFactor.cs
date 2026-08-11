namespace ElegiBien.Domain.Entities;

public class ComparisonFactor
{
    public Guid ComparisonFactorId { get; set; } = Guid.NewGuid();
    public Guid ComparisonScoreId { get; set; }
    public string FactorCode { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public decimal Score { get; set; }
    public decimal MaximumScore { get; set; }
    public decimal? Weight { get; set; }
    public string Explanation { get; set; } = string.Empty;

    public ComparisonScore ComparisonScore { get; set; } = null!;
}
