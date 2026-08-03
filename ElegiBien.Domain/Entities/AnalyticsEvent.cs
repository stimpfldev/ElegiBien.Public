using ElegiBien.Domain.Enums;

namespace ElegiBien.Domain.Entities;

public class AnalyticsEvent
{
    public Guid AnalyticsEventId { get; set; } = Guid.NewGuid();

    public Guid? AnalysisId { get; set; }

    public int CategoryId { get; set; }

    public AnalyticsEventType EventType { get; set; }

    public AnalysisMode? Mode { get; set; }

    public DateTime OccurredAtUtc { get; set; }

    public Analysis? Analysis { get; set; }

    public Category Category { get; set; } = null!;
}