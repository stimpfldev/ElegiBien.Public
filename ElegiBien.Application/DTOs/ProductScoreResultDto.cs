using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.DTOs;

public class ProductScoreResultDto
{
    public string ProductName { get; set; } = string.Empty;

    public int TotalScore { get; set; }

    public CapacityFitStatus CapacityStatus { get; set; }

    public ConfidenceLevel ConfidenceLevel { get; set; }

    public bool IsEligible { get; set; }

    public int? AppliedMaximumScore { get; set; }

    public List<ScoreFactorDto> Factors { get; set; } = [];
}