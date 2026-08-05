using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.DTOs;

public class HeatingProductScoreResultDto
{
    public string ProductName { get; set; } = string.Empty;

    public int TotalScore { get; set; }

    public HeatingCapacityStatus CapacityStatus { get; set; }

    public bool IsEligible { get; set; }

    public decimal? AppliedMaximumScore { get; set; }

    public IReadOnlyCollection<HeatingScoreFactorDto> Factors { get; set; } =
        Array.Empty<HeatingScoreFactorDto>();
}
