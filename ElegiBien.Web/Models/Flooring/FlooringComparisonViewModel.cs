using System.ComponentModel.DataAnnotations;
using ElegiBien.Application.DTOs;

namespace ElegiBien.Web.Models.Flooring;

public class FlooringComparisonViewModel
{
    public Guid AnalysisId { get; set; }

    [Range(0.1, 100000, ErrorMessage = "Ingresá una superficie requerida mayor que 0."),
     Display(Name = "Superficie requerida")]
    public decimal RequiredAreaSquareMeters { get; set; }

    public FlooringProductAlternativeDto FirstProduct { get; set; } = new()
    {
        Name = "Producto A"
    };

    public FlooringProductAlternativeDto SecondProduct { get; set; } = new()
    {
        Name = "Producto B"
    };

    public FlooringComparisonResultDto? Result { get; set; }
    public string? ShareUrl { get; set; }
}
