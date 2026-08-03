using ElegiBien.Domain.Enums;

namespace ElegiBien.Domain.Entities;

public class ProductAlternative
{
    public Guid ProductAlternativeId { get; set; } = Guid.NewGuid();

    public Guid AnalysisId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Brand { get; set; }

    public decimal CapacityFrigories { get; set; }

    public decimal Price { get; set; }

    public AirConditionerTechnology Technology { get; set; }

    public decimal? NominalConsumptionWatts { get; set; }

    public int WarrantyMonths { get; set; }

    public string? ReferenceUrl { get; set; }

    public Analysis Analysis { get; set; } = null!;

    public ProductScore? ProductScore { get; set; }
}