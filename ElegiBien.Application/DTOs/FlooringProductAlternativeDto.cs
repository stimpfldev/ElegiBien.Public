using System.ComponentModel.DataAnnotations;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.DTOs;

public class FlooringProductAlternativeDto
{
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [StringLength(200, ErrorMessage = "El campo {0} no puede superar los {1} caracteres.")]
    [Display(Name = "Nombre o modelo")]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 100, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Cobertura por caja")]
    public decimal CoverageSquareMetersPerBox { get; set; }

    [Range(0.01, 999999999, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Precio por caja")]
    public decimal PricePerBox { get; set; }

    [Display(Name = "Resistencia de uso")]
    public FlooringUseResistance UseResistance { get; set; } =
        FlooringUseResistance.Unknown;

    [Display(Name = "Facilidad de reposición")]
    public FlooringReplacementEase ReplacementEase { get; set; } =
        FlooringReplacementEase.Unknown;
}
