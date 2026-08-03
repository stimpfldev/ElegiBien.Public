using ElegiBien.Application.DTOs;

namespace ElegiBien.Web.Models.AirConditioning;

public class AirConditioningComparisonViewModel
{
    public Guid AnalysisId { get; set; }

    public decimal RecommendedMinimumFrigories { get; set; }

    public decimal RecommendedMaximumFrigories { get; set; }

    public ProductAlternativeDto FirstProduct { get; set; } = new();

    public ProductAlternativeDto SecondProduct { get; set; } = new();

    public ProductComparisonResultDto? Result { get; set; }

    public string? ShareUrl { get; set; }
}