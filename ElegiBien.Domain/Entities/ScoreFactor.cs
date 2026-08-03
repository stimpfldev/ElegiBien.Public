using ElegiBien.Domain.Enums;

namespace ElegiBien.Domain.Entities;

public class ScoreFactor
{
    public Guid ScoreFactorId { get; set; } = Guid.NewGuid();

    public Guid ProductScoreId { get; set; }

    public ScoreFactorType FactorType { get; set; }

    public decimal Score { get; set; }

    public decimal MaximumScore { get; set; }

    public string Explanation { get; set; } = string.Empty;

    public ProductScore ProductScore { get; set; } = null!;
}