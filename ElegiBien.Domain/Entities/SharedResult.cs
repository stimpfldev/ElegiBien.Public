namespace ElegiBien.Domain.Entities;

public class SharedResult
{
    public Guid SharedResultId { get; set; } = Guid.NewGuid();

    public Guid AnalysisId { get; set; }

    public string PublicToken { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime ExpiresAtUtc { get; set; }

    public bool IsActive { get; set; } = true;

    public int AccessCount { get; set; }

    public DateTime? LastAccessedAtUtc { get; set; }

    public Analysis Analysis { get; set; } = null!;
}