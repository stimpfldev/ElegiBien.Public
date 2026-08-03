namespace ElegiBien.Application.DTOs;

public class ProductComparisonResultDto
{
    public ProductScoreResultDto FirstProduct { get; set; } = new();

    public ProductScoreResultDto SecondProduct { get; set; } = new();

    public string Recommendation { get; set; } = string.Empty;

    public bool IsTechnicalTie { get; set; }

    public bool HasRecommendedProduct { get; set; }

    public string? RecommendedProductName { get; set; }
}