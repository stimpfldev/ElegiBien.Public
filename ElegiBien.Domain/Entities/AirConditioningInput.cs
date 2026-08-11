using ElegiBien.Domain.Enums;

namespace ElegiBien.Domain.Entities;

public class AirConditioningInput
{
    public Guid AirConditioningInputId { get; set; } = Guid.NewGuid();

    public Guid AnalysisId { get; set; }

    public decimal LengthMeters { get; set; }

    public decimal WidthMeters { get; set; }

    public decimal HeightMeters { get; set; } = 2.60m;

    public bool IsHeightAssumed { get; set; }

    public int PeopleCount { get; set; }

    public SunExposure SunExposure { get; set; }

    public ClimateZone ClimateZone { get; set; } = ClimateZone.Temperate;

    public InsulationLevel InsulationLevel { get; set; } =
        InsulationLevel.Normal;

    public WindowExposure WindowExposure { get; set; } =
        WindowExposure.Normal;

    public bool IsOpenToAnotherSpace { get; set; }

    public bool HasHighHeatEquipment { get; set; }

    public Analysis Analysis { get; set; } = null!;
}