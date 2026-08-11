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

    public PaintInput? PaintInput { get; set; }

    public PaintCalculationResult? PaintCalculationResult { get; set; }

    public FlooringInput? FlooringInput { get; set; }

    public FlooringCalculationResult? FlooringCalculationResult { get; set; }

    public HeatingInput? HeatingInput { get; set; }

    public HeatingCalculationResult? HeatingCalculationResult { get; set; }

    public SharedResult? SharedResult { get; set; }

    public ICollection<ComparisonAlternative> ComparisonAlternatives { get; set; } =
        new List<ComparisonAlternative>();

    public ICollection<ConsentRecord> ConsentRecords { get; set; } =
        new List<ConsentRecord>();

    public ICollection<AnalyticsEvent> AnalyticsEvents { get; set; } =
        new List<AnalyticsEvent>();
}
