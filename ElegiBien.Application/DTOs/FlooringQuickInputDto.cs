using System.ComponentModel.DataAnnotations;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.DTOs;

public class FlooringQuickInputDto
{
    [Required, Range(0.1, 100), Display(Name = "Largo del piso")]
    public decimal LengthMeters { get; set; }

    [Required, Range(0.1, 100), Display(Name = "Ancho del piso")]
    public decimal WidthMeters { get; set; }

    [Required, Display(Name = "Tipo de colocación")]
    public FlooringInstallationPattern InstallationPattern { get; set; } =
        FlooringInstallationPattern.Straight;

    [Range(0, 30), Display(Name = "Material adicional recomendado")]
    public decimal WastePercentage { get; set; } = 10m;
}
