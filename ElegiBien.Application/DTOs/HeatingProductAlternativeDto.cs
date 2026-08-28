using ElegiBien.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ElegiBien.Application.DTOs;

public class HeatingProductAlternativeDto
{
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [StringLength(120, ErrorMessage = "El campo {0} no puede superar los {1} caracteres.")]
    [Display(Name = "Nombre o modelo")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Display(Name = "Tipo de sistema")]
    public HeatingSystemType SystemType { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Range(100, 50000, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Potencia de calefacción en watts")]
    public decimal HeatingCapacityWatts { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Range(0.01, 100000000, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Precio de compra")]
    public decimal PurchasePrice { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Range(0, 1000000, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
    [Display(Name = "Costo estimado por hora")]
    public decimal EstimatedHourlyCost { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Display(Name = "Eficiencia")]
    public HeatingEfficiencyLevel EfficiencyLevel { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Display(Name = "Seguridad")]
    public HeatingSafetyLevel SafetyLevel { get; set; }
}
