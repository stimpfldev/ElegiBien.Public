using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.DTOs;

public class PaintScoreFactorDto
{
    public PaintScoreFactorType FactorType { get; set; }
    public decimal Score { get; set; }
    public decimal MaximumScore { get; set; }
    public string Explanation { get; set; } = string.Empty;
}
