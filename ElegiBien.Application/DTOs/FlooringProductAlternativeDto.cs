using System.ComponentModel.DataAnnotations;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.DTOs;

public class FlooringProductAlternativeDto
{
    [Required, StringLength(200), Display(Name = "Nombre o modelo")]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 100), Display(Name = "Cobertura por caja")]
    public decimal CoverageSquareMetersPerBox { get; set; }

    [Range(0.01, 999999999), Display(Name = "Precio por caja")]
    public decimal PricePerBox { get; set; }

    [Display(Name = "Resistencia de uso")]
    public FlooringUseResistance UseResistance { get; set; } =
        FlooringUseResistance.Unknown;

    [Display(Name = "Facilidad de reposición")]
    public FlooringReplacementEase ReplacementEase { get; set; } =
        FlooringReplacementEase.Unknown;
}
