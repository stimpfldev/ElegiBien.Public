using System.ComponentModel.DataAnnotations;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.DTOs;

public class PaintProductAlternativeDto
{
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [StringLength(200, ErrorMessage = "El campo {0} no puede superar los {1} caracteres.")]
    [Display(Name = "Nombre o modelo")]
    public string Name { get; set; } = string.Empty;

    [Range(0.1, 100, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Litros por envase")]
    public decimal ContainerLiters { get; set; }

    [Range(0.01, 999999999, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Precio por envase")]
    public decimal PricePerContainer { get; set; }

    [Range(1, 30, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Rendimiento por litro y por mano")]
    public decimal CoverageSquareMetersPerLiterPerCoat { get; set; } = 10m;

    [Display(Name = "Lavabilidad")]
    public PaintWashability Washability { get; set; } = PaintWashability.Unknown;

    [Range(0.1, 72, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Secado en horas")]
    public decimal? DryingHours { get; set; }
}
