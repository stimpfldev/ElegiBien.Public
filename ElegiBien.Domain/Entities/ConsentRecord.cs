using ElegiBien.Domain.Enums;

namespace ElegiBien.Domain.Entities;

public class ConsentRecord
{
    public Guid ConsentRecordId { get; set; } = Guid.NewGuid();

    public Guid AnalysisId { get; set; }

    public ConsentType ConsentType { get; set; }

    public bool IsGranted { get; set; }

    public string LegalVersion { get; set; } = string.Empty;

    public DateTime RecordedAtUtc { get; set; }

    public Analysis Analysis { get; set; } = null!;
}