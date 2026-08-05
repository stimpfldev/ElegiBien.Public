using ElegiBien.Domain.Enums;

namespace ElegiBien.Domain.Entities;

public class HeatingInput
{
    public Guid HeatingInputId { get; set; } = Guid.NewGuid();

    public Guid AnalysisId { get; set; }

    public decimal LengthMeters { get; set; }

    public decimal WidthMeters { get; set; }

    public decimal HeightMeters { get; set; } = 2.60m;

    public bool IsHeightAssumed { get; set; }

    public HeatingClimateZone ClimateZone { get; set; } =
        HeatingClimateZone.TemperateCold;

    public InsulationLevel InsulationLevel { get; set; } =
        InsulationLevel.Normal;

    public int ExteriorWallsCount { get; set; } = 1;

    public WindowExposure WindowExposure { get; set; } =
        WindowExposure.Normal;

    public bool IsOpenToAnotherSpace { get; set; }

    public Analysis Analysis { get; set; } = null!;
}
