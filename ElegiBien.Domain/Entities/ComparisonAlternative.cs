using ElegiBien.Domain.Enums;

namespace ElegiBien.Domain.Entities;

public class ComparisonAlternative
{
    public Guid ComparisonAlternativeId { get; set; } = Guid.NewGuid();
    public Guid AnalysisId { get; set; }
    public CategoryCode CategoryCode { get; set; }
    public int Position { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? TotalCost { get; set; }
    public string DetailsJson { get; set; } = "{}";

    public Analysis Analysis { get; set; } = null!;
    public ComparisonScore? Score { get; set; }
}
