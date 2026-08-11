using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.Services;

public class FlooringCalculator : IFlooringCalculator
{
    public decimal GetRecommendedWastePercentage(
        FlooringInstallationPattern installationPattern)
    {
        return installationPattern switch
        {
            FlooringInstallationPattern.Straight => 10m,
            FlooringInstallationPattern.Staggered => 12m,
            FlooringInstallationPattern.Diagonal => 15m,
            _ => throw new ArgumentOutOfRangeException(
                nameof(installationPattern))
        };
    }

    public FlooringCalculationResult Calculate(FlooringInput input)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (input.LengthMeters <= 0 || input.WidthMeters <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(input),
                "Las dimensiones deben ser mayores que cero.");
        }

        if (input.WastePercentage < 0 || input.WastePercentage > 30)
        {
            throw new ArgumentOutOfRangeException(
                nameof(input.WastePercentage),
                "El desperdicio debe estar entre 0 y 30 %. ");
        }

        if (!Enum.IsDefined(input.InstallationPattern))
        {
            throw new ArgumentOutOfRangeException(
                nameof(input.InstallationPattern));
        }

        var totalArea = input.LengthMeters * input.WidthMeters;
        var wasteArea = totalArea * input.WastePercentage / 100m;
        var requiredArea = totalArea + wasteArea;

        var requiresProfessionalReview =
            totalArea >= 250m ||
            input.InstallationPattern == FlooringInstallationPattern.Diagonal;

        return new FlooringCalculationResult
        {
            AnalysisId = input.AnalysisId,
            TotalAreaSquareMeters = Math.Round(totalArea, 2),
            WastePercentage = input.WastePercentage,
            WasteAreaSquareMeters = Math.Round(wasteArea, 2),
            RequiredAreaSquareMeters = Math.Round(requiredArea, 2),
            ConfidenceLevel =
                input.InstallationPattern == FlooringInstallationPattern.Straight
                    ? ConfidenceLevel.High
                    : ConfidenceLevel.Medium,
            RequiresProfessionalReview = requiresProfessionalReview
        };
    }
}
