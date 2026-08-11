namespace ElegiBien.Application.DTOs;

public class PaintComparisonResultDto
{
    public PaintProductScoreResultDto FirstProduct { get; set; } = new();
    public PaintProductScoreResultDto SecondProduct { get; set; } = new();
    public bool IsTechnicalTie { get; set; }
    public string? RecommendedProductName { get; set; }
    public string Recommendation { get; set; } = string.Empty;
}
