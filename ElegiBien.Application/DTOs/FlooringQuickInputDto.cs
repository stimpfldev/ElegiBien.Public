using System.ComponentModel.DataAnnotations;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.DTOs;

public class FlooringQuickInputDto
{
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Range(0.1, 100, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Largo del piso")]
    public decimal LengthMeters { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Range(0.1, 100, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Ancho del piso")]
    public decimal WidthMeters { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Display(Name = "Tipo de colocación")]
    public FlooringInstallationPattern InstallationPattern { get; set; } =
        FlooringInstallationPattern.Straight;

    [Range(0, 30, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Material adicional recomendado")]
    public decimal WastePercentage { get; set; } = 10m;
}
