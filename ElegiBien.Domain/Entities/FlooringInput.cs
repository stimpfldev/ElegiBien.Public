using ElegiBien.Domain.Enums;

namespace ElegiBien.Domain.Entities;

public class FlooringInput
{
    public Guid FlooringInputId { get; set; } = Guid.NewGuid();
    public Guid AnalysisId { get; set; }
    public decimal LengthMeters { get; set; }
    public decimal WidthMeters { get; set; }
    public FlooringInstallationPattern InstallationPattern { get; set; }
    public decimal WastePercentage { get; set; }
    public Analysis Analysis { get; set; } = null!;
}
