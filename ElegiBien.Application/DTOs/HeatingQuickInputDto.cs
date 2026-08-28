using ElegiBien.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ElegiBien.Application.DTOs;

public class HeatingQuickInputDto
{
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Range(1, 200, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Largo del ambiente")]
    public decimal LengthMeters { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Range(1, 200, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Ancho del ambiente")]
    public decimal WidthMeters { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Range(2, 6, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Altura del ambiente")]
    public decimal HeightMeters { get; set; } = 2.60m;

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Display(Name = "Zona climática")]
    public HeatingClimateZone ClimateZone { get; set; } =
        HeatingClimateZone.TemperateCold;

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Display(Name = "Nivel de aislamiento")]
    public InsulationLevel InsulationLevel { get; set; } =
        InsulationLevel.Normal;

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Range(0, 4, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Paredes que dan al exterior")]
    public int ExteriorWallsCount { get; set; } = 1;

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Display(Name = "Cantidad de ventanas")]
    public WindowExposure WindowExposure { get; set; } =
        WindowExposure.Normal;

    [Display(Name = "Ambiente abierto hacia otro espacio")]
    public bool IsOpenToAnotherSpace { get; set; }
}
