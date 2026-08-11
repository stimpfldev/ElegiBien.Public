using ElegiBien.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ElegiBien.Application.DTOs;

public class HeatingQuickInputDto
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
    [Range(2, 6)]
    [Display(Name = "Altura del ambiente")]
    public decimal HeightMeters { get; set; } = 2.60m;

    [Required]
    [Display(Name = "Zona climática")]
    public HeatingClimateZone ClimateZone { get; set; } =
        HeatingClimateZone.TemperateCold;

    [Required]
    [Display(Name = "Nivel de aislamiento")]
    public InsulationLevel InsulationLevel { get; set; } =
        InsulationLevel.Normal;

    [Required]
    [Range(0, 4)]
    [Display(Name = "Paredes que dan al exterior")]
    public int ExteriorWallsCount { get; set; } = 1;

    [Required]
    [Display(Name = "Cantidad de ventanas")]
    public WindowExposure WindowExposure { get; set; } =
        WindowExposure.Normal;

    [Display(Name = "Ambiente abierto hacia otro espacio")]
    public bool IsOpenToAnotherSpace { get; set; }
}
