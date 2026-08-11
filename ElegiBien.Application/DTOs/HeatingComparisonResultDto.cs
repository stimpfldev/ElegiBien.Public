namespace ElegiBien.Application.DTOs;

public class HeatingComparisonResultDto
{
    public HeatingProductScoreResultDto FirstProduct { get; set; } = null!;

    public HeatingProductScoreResultDto SecondProduct { get; set; } = null!;

    public string? RecommendedProductName { get; set; }

    public bool IsTie { get; set; }
}
