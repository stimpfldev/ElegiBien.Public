using ElegiBien.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ElegiBien.Application.DTOs;

public class HeatingProductAlternativeDto
{
    [Required]
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public HeatingSystemType SystemType { get; set; }

    [Required]
    [Range(100, 50000)]
    public decimal HeatingCapacityWatts { get; set; }

    [Required]
    [Range(0.01, 100000000)]
    public decimal PurchasePrice { get; set; }

    [Required]
    [Range(0, 1000000)]
    public decimal EstimatedHourlyCost { get; set; }

    [Required]
    public HeatingEfficiencyLevel EfficiencyLevel { get; set; }

    [Required]
    public HeatingSafetyLevel SafetyLevel { get; set; }
}
