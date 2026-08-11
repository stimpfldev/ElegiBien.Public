namespace ElegiBien.Domain.Entities;

public class MethodologyVersion
{
    public int MethodologyVersionId { get; set; }

    public int CategoryId { get; set; }

    public string Version { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime EffectiveFromUtc { get; set; }

    public DateTime? EffectiveToUtc { get; set; }

    public bool IsActive { get; set; }

    public Category Category { get; set; } = null!;
}