using ElegiBien.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ElegiBien.Application.DTOs;

public class AirConditioningQuickInputDto
{
    [Required]
    [Range(1, 30)]
    [Display(Name = "Largo del ambiente")]
    public decimal LengthMeters { get; set; }

    [Required]
    [Range(1, 30)]
    [Display(Name = "Ancho del ambiente")]
    public decimal WidthMeters { get; set; }

    [Required]
    [Range(1, 20)]
    [Display(Name = "Cantidad habitual de personas")]
    public int PeopleCount { get; set; } = 2;

    [Required]
    [Display(Name = "Exposición al sol")]
    public SunExposure SunExposure { get; set; } =
        SunExposure.Medium;
}