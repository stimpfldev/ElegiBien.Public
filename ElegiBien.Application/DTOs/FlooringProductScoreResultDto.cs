using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.DTOs;

public class FlooringProductScoreResultDto
{
    public string ProductName { get; set; } = string.Empty;
    public int TotalScore { get; set; }
    public FlooringCoverageStatus CoverageStatus { get; set; }
    public ConfidenceLevel ConfidenceLevel { get; set; }
    public int BoxesRequired { get; set; }
    public decimal RequiredAreaSquareMeters { get; set; }
    public decimal PurchasedAreaSquareMeters { get; set; }
    public decimal ExcessAreaSquareMeters { get; set; }
    public decimal ExcessPercentage { get; set; }
    public decimal TotalCost { get; set; }
    public IReadOnlyCollection<FlooringScoreFactorDto> Factors { get; set; } = [];
}
