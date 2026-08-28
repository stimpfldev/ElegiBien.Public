using ElegiBien.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ElegiBien.Application.DTOs;

public class AirConditioningQuickInputDto
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
    [Range(1, 20, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Cantidad habitual de personas")]
    public int PeopleCount { get; set; } = 2;

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Display(Name = "Exposición al sol")]
    public SunExposure SunExposure { get; set; } =
        SunExposure.Medium;
}
