using ElegiBien.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace ElegiBien.Web.Models.Heating;

public class HeatingComparisonViewModel
{
    [Required]
    public Guid AnalysisId { get; set; }
    public decimal RecommendedMinimumWatts { get; set; }
    public decimal RecommendedMaximumWatts { get; set; }
    public HeatingProductAlternativeDto FirstProduct { get; set; } = new() { Name = "Equipo A" };
    public HeatingProductAlternativeDto SecondProduct { get; set; } = new() { Name = "Equipo B" };
    public HeatingComparisonResultDto? Result { get; set; }
    public string? ShareUrl { get; set; }
}
