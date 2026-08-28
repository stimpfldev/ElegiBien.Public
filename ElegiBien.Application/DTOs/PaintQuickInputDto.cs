using System.ComponentModel.DataAnnotations;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.DTOs;

public class PaintQuickInputDto
{
    [Required, Range(1, 200), Display(Name = "Largo del ambiente")]
    public decimal LengthMeters { get; set; }

    [Required, Range(1, 200), Display(Name = "Ancho del ambiente")]
    public decimal WidthMeters { get; set; }

    [Required, Range(2, 6), Display(Name = "Altura del ambiente")]
    public decimal HeightMeters { get; set; } = 2.60m;

    [Display(Name = "Incluir el techo")]
    public bool IncludeCeiling { get; set; }

    [Range(0, 20), Display(Name = "Cantidad de puertas")]
    public int DoorCount { get; set; } = 1;

    [Range(0, 30), Display(Name = "Cantidad de ventanas")]
    public int WindowCount { get; set; } = 1;

    [Range(1, 5), Display(Name = "Cantidad de manos")]
    public int CoatCount { get; set; } = 2;

    [Required, Display(Name = "Estado de la superficie")]
    public PaintSurfaceCondition SurfaceCondition { get; set; } = PaintSurfaceCondition.Good;
}
