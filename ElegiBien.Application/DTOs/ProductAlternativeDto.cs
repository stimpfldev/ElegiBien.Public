using ElegiBien.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ElegiBien.Application.DTOs;

public class ProductAlternativeDto
{
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [StringLength(200, ErrorMessage = "El campo {0} no puede superar los {1} caracteres.")]
    [Display(Name = "Nombre o modelo")]
    public string Name { get; set; } = string.Empty;

    [Range(1, 100000, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Capacidad en frigorías")]
    public decimal CapacityFrigories { get; set; }

    [Range(0.01, 999999999, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Precio total")]
    public decimal Price { get; set; }

    [Display(Name = "Tecnología")]
    public AirConditionerTechnology Technology { get; set; } =
        AirConditionerTechnology.Unknown;

    [Range(1, 100000, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Consumo nominal en watts")]
    public decimal? NominalConsumptionWatts { get; set; }

    [Range(0, 120, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Garantía en meses")]
    public int WarrantyMonths { get; set; }
}
