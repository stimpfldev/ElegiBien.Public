using ElegiBien.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ElegiBien.Application.DTOs;

public class ProductAlternativeDto
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 100000)]
    public decimal CapacityFrigories { get; set; }

    [Range(0.01, 999999999)]
    public decimal Price { get; set; }

    public AirConditionerTechnology Technology { get; set; } =
        AirConditionerTechnology.Unknown;

    [Range(1, 100000)]
    public decimal? NominalConsumptionWatts { get; set; }

    [Range(0, 120)]
    public int WarrantyMonths { get; set; }
}