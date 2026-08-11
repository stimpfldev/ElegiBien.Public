using ElegiBien.Domain.Enums;

namespace ElegiBien.Domain.Entities;

public class PaintInput
{
    public Guid PaintInputId { get; set; } = Guid.NewGuid();
    public Guid AnalysisId { get; set; }
    public decimal LengthMeters { get; set; }
    public decimal WidthMeters { get; set; }
    public decimal HeightMeters { get; set; }
    public bool IncludeCeiling { get; set; }
    public int DoorCount { get; set; }
    public int WindowCount { get; set; }
    public int CoatCount { get; set; }
    public PaintSurfaceCondition SurfaceCondition { get; set; }
    public decimal WastePercentage { get; set; }
    public Analysis Analysis { get; set; } = null!;
}
