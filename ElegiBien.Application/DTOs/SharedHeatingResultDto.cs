namespace ElegiBien.Application.DTOs;

public class SharedHeatingResultDto
{
    public decimal SurfaceSquareMeters { get; set; }
    public decimal VolumeCubicMeters { get; set; }
    public decimal RecommendedMinimumWatts { get; set; }
    public decimal RecommendedMaximumWatts { get; set; }
    public decimal IdealPowerWatts { get; set; }
    public decimal IdealPowerKcalPerHour { get; set; }
    public string Recommendation { get; set; } = string.Empty;
    public IReadOnlyCollection<HeatingProductScoreResultDto> Products { get; set; } = [];
}
