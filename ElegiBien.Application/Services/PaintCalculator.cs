using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.Services;

public class PaintCalculator : IPaintCalculator
{
    private const decimal DoorArea = 1.80m;
    private const decimal WindowArea = 1.50m;
    private const decimal ReferenceCoverage = 10m;

    public PaintCalculationResult Calculate(PaintInput input)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (input.LengthMeters <= 0 || input.WidthMeters <= 0 || input.HeightMeters <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(input), "Las dimensiones deben ser mayores que cero.");
        }

        if (input.CoatCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(input.CoatCount));
        }

        var wallArea = 2m * (input.LengthMeters + input.WidthMeters) * input.HeightMeters;
        var ceilingArea = input.IncludeCeiling ? input.LengthMeters * input.WidthMeters : 0m;
        var deductedArea = input.DoorCount * DoorArea + input.WindowCount * WindowArea;
        var netArea = Math.Max(0m, wallArea + ceilingArea - deductedArea);

        var conditionFactor = input.SurfaceCondition switch
        {
            PaintSurfaceCondition.Good => 1.00m,
            PaintSurfaceCondition.NewOrPorous => 1.15m,
            PaintSurfaceCondition.Damaged => 1.25m,
            _ => 1.00m
        };

        var wasteFactor = 1m + input.WastePercentage / 100m;
        var adjustedArea = netArea * input.CoatCount * conditionFactor * wasteFactor;
        var liters = adjustedArea / ReferenceCoverage;
        var requiresReview = input.SurfaceCondition == PaintSurfaceCondition.Damaged || netArea >= 250m;

        return new PaintCalculationResult
        {
            AnalysisId = input.AnalysisId,
            WallAreaSquareMeters = Math.Round(wallArea, 2),
            CeilingAreaSquareMeters = Math.Round(ceilingArea, 2),
            DeductedAreaSquareMeters = Math.Round(deductedArea, 2),
            NetAreaSquareMeters = Math.Round(netArea, 2),
            AdjustedAreaSquareMeters = Math.Round(adjustedArea, 2),
            ReferenceCoverageSquareMetersPerLiter = ReferenceCoverage,
            ReferenceLiters = Math.Round(liters, 2),
            ConfidenceLevel = input.SurfaceCondition == PaintSurfaceCondition.Good ? ConfidenceLevel.High : ConfidenceLevel.Medium,
            RequiresProfessionalReview = requiresReview
        };
    }
}
