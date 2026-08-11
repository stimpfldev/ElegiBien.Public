namespace ElegiBien.Application.DTOs;

public class FlooringComparisonResultDto
{
    public FlooringProductScoreResultDto FirstProduct { get; set; } = new();
    public FlooringProductScoreResultDto SecondProduct { get; set; } = new();
    public bool IsTechnicalTie { get; set; }
    public string? RecommendedProductName { get; set; }
    public string Recommendation { get; set; } = string.Empty;
}
