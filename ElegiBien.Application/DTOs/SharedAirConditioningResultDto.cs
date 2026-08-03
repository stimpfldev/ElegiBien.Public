namespace ElegiBien.Application.DTOs;

public class SharedAirConditioningResultDto
{
    public decimal RecommendedMinimumFrigories { get; set; }

    public decimal RecommendedMaximumFrigories { get; set; }

    public decimal IdealFrigories { get; set; }

    public List<ProductScoreResultDto> Products { get; set; } = [];

    public string Recommendation { get; set; } = string.Empty;
}