using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.Services;

public class AirConditioningCalculator
    : IAirConditioningCalculator
{
    private const decimal BaseCoefficient = 50m;
    private const decimal AdditionalPersonFrigories = 150m;

    public DimensioningResult Calculate(
        AirConditioningInput input)
    {
        Validate(input);

        var volume =
            input.LengthMeters *
            input.WidthMeters *
            input.HeightMeters;

        var baseFrigories = volume * BaseCoefficient;
        var adjustedFrigories = baseFrigories;

        adjustedFrigories *= GetSunMultiplier(input.SunExposure);
        adjustedFrigories *= GetClimateMultiplier(input.ClimateZone);
        adjustedFrigories *= GetInsulationMultiplier(
            input.InsulationLevel);
        adjustedFrigories *= GetWindowMultiplier(
            input.WindowExposure);

        if (input.IsOpenToAnotherSpace)
        {
            adjustedFrigories *= 1.10m;
        }

        if (input.HasHighHeatEquipment)
        {
            adjustedFrigories *= 1.05m;
        }

        var additionalPeople =
            Math.Max(0, input.PeopleCount - 2);

        adjustedFrigories +=
            additionalPeople * AdditionalPersonFrigories;

        var minimum = Math.Ceiling(adjustedFrigories);
        var maximum = Math.Ceiling(adjustedFrigories * 1.10m);
        var ideal = Math.Ceiling((minimum + maximum) / 2m);

        var confidence = GetConfidence(input);
        var requiresProfessionalReview =
            RequiresProfessionalReview(input);

        return new DimensioningResult
        {
            AnalysisId = input.AnalysisId,
            VolumeCubicMeters = volume,
            BaseFrigories = baseFrigories,
            AdjustmentFrigories =
                adjustedFrigories - baseFrigories,
            EstimatedFrigories = adjustedFrigories,
            RecommendedMinimumFrigories = minimum,
            RecommendedMaximumFrigories = maximum,
            IdealFrigories = ideal,
            ConfidenceLevel = confidence,
            RequiresProfessionalReview =
                requiresProfessionalReview
        };
    }

    private static void Validate(
        AirConditioningInput input)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (input.LengthMeters <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(input.LengthMeters));
        }

        if (input.WidthMeters <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(input.WidthMeters));
        }

        if (input.HeightMeters <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(input.HeightMeters));
        }

        if (input.PeopleCount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(input.PeopleCount));
        }
    }

    private static decimal GetSunMultiplier(
        SunExposure sunExposure)
    {
        return sunExposure switch
        {
            SunExposure.Low => 0.90m,
            SunExposure.Medium => 1.00m,
            SunExposure.High => 1.10m,
            _ => throw new ArgumentOutOfRangeException(
                nameof(sunExposure))
        };
    }

    private static decimal GetClimateMultiplier(
        ClimateZone climateZone)
    {
        return climateZone switch
        {
            ClimateZone.Temperate => 1.00m,
            ClimateZone.Warm => 1.10m,
            ClimateZone.VeryWarm => 1.15m,
            _ => throw new ArgumentOutOfRangeException(
                nameof(climateZone))
        };
    }

    private static decimal GetInsulationMultiplier(
        InsulationLevel insulationLevel)
    {
        return insulationLevel switch
        {
            InsulationLevel.Good => 0.95m,
            InsulationLevel.Normal => 1.00m,
            InsulationLevel.Poor => 1.10m,
            _ => throw new ArgumentOutOfRangeException(
                nameof(insulationLevel))
        };
    }

    private static decimal GetWindowMultiplier(
        WindowExposure windowExposure)
    {
        return windowExposure switch
        {
            WindowExposure.Normal => 1.00m,
            WindowExposure.Significant => 1.05m,
            WindowExposure.LargeGlazing => 1.10m,
            _ => throw new ArgumentOutOfRangeException(
                nameof(windowExposure))
        };
    }

    private static ConfidenceLevel GetConfidence(
        AirConditioningInput input)
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

    private static bool RequiresProfessionalReview(
        AirConditioningInput input)
    {
        var surface =
            input.LengthMeters * input.WidthMeters;

        return surface > 50m ||
               input.HeightMeters > 3.20m ||
               input.PeopleCount > 8 ||
               input.WindowExposure ==
                   WindowExposure.LargeGlazing ||
               input.IsOpenToAnotherSpace;
    }
}