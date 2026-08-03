using ElegiBien.Domain.Enums;

namespace ElegiBien.Domain.Entities;

public class Analysis
{
    public Guid AnalysisId { get; set; } = Guid.NewGuid();

    public int CategoryId { get; set; }

    public int MethodologyVersionId { get; set; }

    public AnalysisMode Mode { get; set; }

    public ConfidenceLevel ConfidenceLevel { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? CompletedAtUtc { get; set; }

    public bool IsCompleted { get; set; }

    public Category Category { get; set; } = null!;

    public MethodologyVersion MethodologyVersion { get; set; } = null!;

    public AirConditioningInput? AirConditioningInput { get; set; }

    public DimensioningResult? DimensioningResult { get; set; }

    public SharedResult? SharedResult { get; set; }

    public ICollection<ProductAlternative> ProductAlternatives { get; set; } =
        new List<ProductAlternative>();

    public ICollection<ConsentRecord> ConsentRecords { get; set; } =
        new List<ConsentRecord>();

    public ICollection<AnalyticsEvent> AnalyticsEvents { get; set; } =
        new List<AnalyticsEvent>();
}