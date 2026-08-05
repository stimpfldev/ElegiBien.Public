using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.DTOs;

public class PaintProductScoreResultDto
{
    public string ProductName { get; set; } = string.Empty;
    public int TotalScore { get; set; }
    public PaintCoverageStatus CoverageStatus { get; set; }
    public ConfidenceLevel ConfidenceLevel { get; set; }
    public int ContainersRequired { get; set; }
    public decimal LitersRequired { get; set; }
    public decimal LitersPurchased { get; set; }
    public decimal TotalCost { get; set; }
    public IReadOnlyCollection<PaintScoreFactorDto> Factors { get; set; } = [];
}
