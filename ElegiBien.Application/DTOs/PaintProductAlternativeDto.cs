using System.ComponentModel.DataAnnotations;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.DTOs;

public class PaintProductAlternativeDto
{
    [Required, StringLength(200), Display(Name = "Nombre o modelo")]
    public string Name { get; set; } = string.Empty;

    [Range(0.1, 100), Display(Name = "Litros por envase")]
    public decimal ContainerLiters { get; set; }

    [Range(0.01, 999999999), Display(Name = "Precio por envase")]
    public decimal PricePerContainer { get; set; }

    [Range(1, 30), Display(Name = "Rendimiento por litro y por mano")]
    public decimal CoverageSquareMetersPerLiterPerCoat { get; set; } = 10m;

    [Display(Name = "Lavabilidad")]
    public PaintWashability Washability { get; set; } = PaintWashability.Unknown;

    [Range(0.1, 72), Display(Name = "Secado en horas")]
    public decimal? DryingHours { get; set; }
}
