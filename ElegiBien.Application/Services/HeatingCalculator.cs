using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.Services;

public class HeatingCalculator : IHeatingCalculator
{
    private const decimal BaseWattsPerCubicMeter = 50m;
    private const decimal WattsToKcalPerHour = 0.859845m;

    public HeatingCalculationResult Calculate(HeatingInput input)
    {
        Validate(input);

        var surface = input.LengthMeters * input.WidthMeters;
        var volume = surface * input.HeightMeters;
        var basePower = volume * BaseWattsPerCubicMeter;
        var adjustedPower = basePower;

        adjustedPower *= GetClimateMultiplier(input.ClimateZone);
        adjustedPower *= GetInsulationMultiplier(input.InsulationLevel);
        adjustedPower *= GetExteriorWallsMultiplier(input.ExteriorWallsCount);
        adjustedPower *= GetWindowMultiplier(input.WindowExposure);

        if (input.IsOpenToAnotherSpace)
        {
            adjustedPower *= 1.10m;
        }

        var minimum = Math.Ceiling(adjustedPower * 0.95m);
        var maximum = Math.Ceiling(adjustedPower * 1.10m);
        var ideal = Math.Ceiling((minimum + maximum) / 2m);

        return new HeatingCalculationResult
        {
            AnalysisId = input.AnalysisId,
            SurfaceSquareMeters = surface,
            VolumeCubicMeters = volume,
            BasePowerWatts = basePower,
            AdjustmentPowerWatts = adjustedPower - basePower,
            EstimatedPowerWatts = adjustedPower,
            RecommendedMinimumWatts = minimum,
            RecommendedMaximumWatts = maximum,
            IdealPowerWatts = ideal,
            IdealPowerKcalPerHour = Math.Ceiling(ideal * WattsToKcalPerHour),
            ConfidenceLevel = GetConfidence(input),
            RequiresProfessionalReview = RequiresProfessionalReview(input)
        };
    }

    private static void Validate(HeatingInput input)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (input.LengthMeters <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(input.LengthMeters));
        }

        if (input.WidthMeters <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(input.WidthMeters));
        }

        if (input.HeightMeters <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(input.HeightMeters));
        }

        if (input.ExteriorWallsCount is < 0 or > 4)
        {
            throw new ArgumentOutOfRangeException(nameof(input.ExteriorWallsCount));
        }
    }

    private static decimal GetClimateMultiplier(HeatingClimateZone climateZone)
    {
        return climateZone switch
        {
            HeatingClimateZone.Mild => 0.85m,
            HeatingClimateZone.TemperateCold => 1.00m,
            HeatingClimateZone.Cold => 1.15m,
            HeatingClimateZone.VeryCold => 1.30m,
            _ => throw new ArgumentOutOfRangeException(nameof(climateZone))
        };
    }

    private static decimal GetInsulationMultiplier(InsulationLevel insulationLevel)
    {
        return insulationLevel switch
        {
            InsulationLevel.Good => 0.85m,
            InsulationLevel.Normal => 1.00m,
            InsulationLevel.Poor => 1.20m,
            _ => throw new ArgumentOutOfRangeException(nameof(insulationLevel))
        };
    }

    private static decimal GetExteriorWallsMultiplier(int exteriorWallsCount)
    {
        return exteriorWallsCount switch
        {
            0 => 0.95m,
            1 => 1.00m,
            2 => 1.05m,
            3 => 1.10m,
            4 => 1.15m,
            _ => throw new ArgumentOutOfRangeException(nameof(exteriorWallsCount))
        };
    }

    private static decimal GetWindowMultiplier(WindowExposure windowExposure)
    {
        return windowExposure switch
        {
            WindowExposure.Normal => 1.00m,
            WindowExposure.Significant => 1.08m,
            WindowExposure.LargeGlazing => 1.15m,
            _ => throw new ArgumentOutOfRangeException(nameof(windowExposure))
        };
    }

    private static ConfidenceLevel GetConfidence(HeatingInput input)
    {
        if (RequiresProfessionalReview(input))
        {
            return ConfidenceLevel.Low;
        }

        if (input.IsHeightAssumed)
        {
            return ConfidenceLevel.Medium;
        }

        return ConfidenceLevel.High;
    }

    private static bool RequiresProfessionalReview(HeatingInput input)
    {
        var surface = input.LengthMeters * input.WidthMeters;

        return surface > 50m ||
               input.HeightMeters > 3.20m ||
               input.ClimateZone == HeatingClimateZone.VeryCold ||
               input.WindowExposure == WindowExposure.LargeGlazing ||
               input.IsOpenToAnotherSpace;
    }
}
