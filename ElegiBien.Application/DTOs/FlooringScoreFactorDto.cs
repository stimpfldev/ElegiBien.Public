using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.DTOs;

public class FlooringScoreFactorDto
{
    public FlooringScoreFactorType FactorType { get; set; }
    public decimal Score { get; set; }
    public decimal MaximumScore { get; set; }
    public string Explanation { get; set; } = string.Empty;
}
