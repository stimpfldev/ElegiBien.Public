namespace ElegiBien.Application.DTOs;

public class SharedPaintResultDto
{
    public decimal NetAreaSquareMeters { get; set; }
    public decimal ReferenceLiters { get; set; }
    public int CoatCount { get; set; }
    public string Recommendation { get; set; } = string.Empty;
    public IReadOnlyCollection<PaintProductScoreResultDto> Products { get; set; } = [];
}
