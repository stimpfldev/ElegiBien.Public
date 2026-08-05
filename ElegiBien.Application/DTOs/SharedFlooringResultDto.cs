namespace ElegiBien.Application.DTOs;

public class SharedFlooringResultDto
{
    public decimal TotalAreaSquareMeters { get; set; }
    public decimal WastePercentage { get; set; }
    public decimal RequiredAreaSquareMeters { get; set; }
    public string Recommendation { get; set; } = string.Empty;
    public IReadOnlyCollection<FlooringProductScoreResultDto> Products { get; set; } = [];
}
