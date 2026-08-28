using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.Services;

public class HeatingProductComparer : IHeatingProductComparer
{
    public HeatingComparisonResultDto Compare(
        HeatingCalculationResult calculationResult,
        HeatingProductAlternativeDto firstProduct,
        HeatingProductAlternativeDto secondProduct)
    {
        ArgumentNullException.ThrowIfNull(calculationResult);
        ArgumentNullException.ThrowIfNull(firstProduct);
        ArgumentNullException.ThrowIfNull(secondProduct);

        ValidateProduct(firstProduct);
        ValidateProduct(secondProduct);

        var firstEligible = IsEligible(calculationResult, firstProduct.HeatingCapacityWatts);
        var secondEligible = IsEligible(calculationResult, secondProduct.HeatingCapacityWatts);

        var minimumEligiblePrice = GetMinimumEligibleValue(
            firstProduct.PurchasePrice,
            secondProduct.PurchasePrice,
            firstEligible,
            secondEligible);

        var minimumEligibleHourlyCost = GetMinimumEligibleValue(
            firstProduct.EstimatedHourlyCost,
            secondProduct.EstimatedHourlyCost,
            firstEligible,
            secondEligible);

        var firstScore = CalculateScore(
            calculationResult,
            firstProduct,
            firstEligible,
            minimumEligiblePrice,
            minimumEligibleHourlyCost);

        var secondScore = CalculateScore(
            calculationResult,
            secondProduct,
            secondEligible,
            minimumEligiblePrice,
            minimumEligibleHourlyCost);

        return BuildResult(firstScore, secondScore);
    }

    private static HeatingProductScoreResultDto CalculateScore(
        HeatingCalculationResult result,
        HeatingProductAlternativeDto product,
        bool isEligible,
        decimal? minimumEligiblePrice,
        decimal? minimumEligibleHourlyCost)
    {
        var capacity = CalculateCapacityScore(result, product.HeatingCapacityWatts);
        var operatingCost = CalculateRelativeScore(
            product.EstimatedHourlyCost,
            minimumEligibleHourlyCost,
            isEligible,
            25m);
        var efficiency = CalculateEfficiencyScore(product.EfficiencyLevel);
        var safety = CalculateSafetyScore(product.SafetyLevel);
        var purchasePrice = CalculateRelativeScore(
            product.PurchasePrice,
            minimumEligiblePrice,
            isEligible,
            10m);

        var total = capacity.Score + operatingCost + efficiency + safety + purchasePrice;

        if (capacity.MaximumTotalScore.HasValue)
        {
            total = Math.Min(total, capacity.MaximumTotalScore.Value);
        }

        return new HeatingProductScoreResultDto
        {
            ProductName = product.Name,
            TotalScore = (int)Math.Round(total, MidpointRounding.AwayFromZero),
            CapacityStatus = capacity.Status,
            IsEligible = isEligible,
            AppliedMaximumScore = capacity.MaximumTotalScore,
            Factors =
            [
                new HeatingScoreFactorDto
                {
                    FactorType = HeatingScoreFactorType.CapacityAdequacy,
                    Score = capacity.Score,
                    MaximumScore = 35m,
                    Explanation = capacity.Explanation
                },
                new HeatingScoreFactorDto
                {
                    FactorType = HeatingScoreFactorType.EstimatedOperatingCost,
                    Score = operatingCost,
                    MaximumScore = 25m,
                    Explanation = isEligible
                        ? "El costo por hora se comparó entre alternativas con capacidad suficiente."
                        : "No recibió puntos porque la capacidad informada no resulta elegible."
                },
                new HeatingScoreFactorDto
                {
                    FactorType = HeatingScoreFactorType.Efficiency,
                    Score = efficiency,
                    MaximumScore = 15m,
                    Explanation = $"Nivel de eficiencia informado: {EfficiencyText(product.EfficiencyLevel)}."
                },
                new HeatingScoreFactorDto
                {
                    FactorType = HeatingScoreFactorType.SafetyAndInstallation,
                    Score = safety,
                    MaximumScore = 15m,
                    Explanation = $"Condición de instalación informada: {SafetyText(product.SafetyLevel)}."
                },
                new HeatingScoreFactorDto
                {
                    FactorType = HeatingScoreFactorType.PurchasePrice,
                    Score = purchasePrice,
                    MaximumScore = 10m,
                    Explanation = isEligible
                        ? "El precio se comparó entre alternativas con capacidad suficiente."
                        : "No recibió puntos porque la capacidad informada no resulta elegible."
                }
            ]
        };
    }

    private static CapacityScore CalculateCapacityScore(
        HeatingCalculationResult result,
        decimal capacityWatts)
    {
        var minimum = result.RecommendedMinimumWatts;
        var maximum = result.RecommendedMaximumWatts;

        if (capacityWatts >= minimum && capacityWatts <= maximum)
        {
            return new CapacityScore(
                35m,
                HeatingCapacityStatus.Correct,
                null,
                "La potencia está dentro del rango recomendado.");
        }

        if (capacityWatts < minimum)
        {
            var difference = (minimum - capacityWatts) / minimum;

            if (difference <= 0.10m)
            {
                return new CapacityScore(
                    22m,
                    HeatingCapacityStatus.Insufficient,
                    74m,
                    "La potencia está hasta un 10 % por debajo del mínimo recomendado.");
            }

            if (difference <= 0.20m)
            {
                return new CapacityScore(
                    12m,
                    HeatingCapacityStatus.Insufficient,
                    59m,
                    "La potencia está entre un 10 % y un 20 % por debajo del mínimo recomendado.");
            }

            return new CapacityScore(
                0m,
                HeatingCapacityStatus.Insufficient,
                39m,
                "La potencia está más de un 20 % por debajo del mínimo recomendado.");
        }

        var excess = (capacityWatts - maximum) / maximum;

        if (excess <= 0.10m)
        {
            return new CapacityScore(
                30m,
                HeatingCapacityStatus.AcceptableOversized,
                null,
                "La potencia supera hasta un 10 % el máximo recomendado.");
        }

        if (excess <= 0.20m)
        {
            return new CapacityScore(
                20m,
                HeatingCapacityStatus.AcceptableOversized,
                79m,
                "La potencia supera entre un 10 % y un 20 % el máximo recomendado.");
        }

        return new CapacityScore(
            8m,
            HeatingCapacityStatus.RelevantOversized,
            64m,
            "La potencia supera más de un 20 % el máximo recomendado.");
    }

    private static decimal CalculateRelativeScore(
        decimal productValue,
        decimal? minimumEligibleValue,
        bool isEligible,
        decimal maximumScore)
    {
        if (!isEligible || !minimumEligibleValue.HasValue)
        {
            return 0m;
        }

        if (productValue == 0m)
        {
            return minimumEligibleValue.Value == 0m ? maximumScore : 0m;
        }

        return Math.Round(
            maximumScore * minimumEligibleValue.Value / productValue,
            2);
    }

    private static decimal CalculateEfficiencyScore(HeatingEfficiencyLevel level)
    {
        return level switch
        {
            HeatingEfficiencyLevel.Low => 4m,
            HeatingEfficiencyLevel.Medium => 8m,
            HeatingEfficiencyLevel.High => 12m,
            HeatingEfficiencyLevel.VeryHigh => 15m,
            _ => 0m
        };
    }

    private static string EfficiencyText(HeatingEfficiencyLevel level)
    {
        return level switch
        {
            HeatingEfficiencyLevel.Low => "Baja",
            HeatingEfficiencyLevel.Medium => "Media",
            HeatingEfficiencyLevel.High => "Alta",
            HeatingEfficiencyLevel.VeryHigh => "Muy alta",
            _ => "Sin datos"
        };
    }

    private static decimal CalculateSafetyScore(HeatingSafetyLevel level)
    {
        return level switch
        {
            HeatingSafetyLevel.RequiresSpecialistInstallation => 7m,
            HeatingSafetyLevel.RequiresDedicatedElectricalCheck => 9m,
            HeatingSafetyLevel.StandardInstallation => 12m,
            HeatingSafetyLevel.SimpleInstallation => 15m,
            _ => 0m
        };
    }

    private static string SafetyText(HeatingSafetyLevel level)
    {
        return level switch
        {
            HeatingSafetyLevel.RequiresSpecialistInstallation => "Requiere instalación profesional",
            HeatingSafetyLevel.RequiresDedicatedElectricalCheck => "Requiere revisión eléctrica dedicada",
            HeatingSafetyLevel.StandardInstallation => "Instalación estándar",
            HeatingSafetyLevel.SimpleInstallation => "Instalación simple",
            _ => "Sin datos"
        };
    }

    private static bool IsEligible(HeatingCalculationResult result, decimal capacityWatts)
    {
        return capacityWatts >= result.RecommendedMinimumWatts &&
               capacityWatts <= result.RecommendedMaximumWatts * 1.10m;
    }

    private static decimal? GetMinimumEligibleValue(
        decimal firstValue,
        decimal secondValue,
        bool firstEligible,
        bool secondEligible)
    {
        if (firstEligible && secondEligible)
        {
            return Math.Min(firstValue, secondValue);
        }

        if (firstEligible)
        {
            return firstValue;
        }

        if (secondEligible)
        {
            return secondValue;
        }

        return null;
    }

    private static void ValidateProduct(HeatingProductAlternativeDto product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            throw new ArgumentException("El nombre del producto es obligatorio.", nameof(product));
        }

        if (product.HeatingCapacityWatts <= 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(product.HeatingCapacityWatts));
        }

        if (product.PurchasePrice <= 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(product.PurchasePrice));
        }

        if (product.EstimatedHourlyCost < 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(product.EstimatedHourlyCost));
        }
    }

    private static HeatingComparisonResultDto BuildResult(
        HeatingProductScoreResultDto first,
        HeatingProductScoreResultDto second)
    {
        var isTie = first.TotalScore == second.TotalScore;

        return new HeatingComparisonResultDto
        {
            FirstProduct = first,
            SecondProduct = second,
            IsTie = isTie,
            RecommendedProductName = isTie
                ? null
                : first.TotalScore > second.TotalScore
                    ? first.ProductName
                    : second.ProductName
        };
    }

    private sealed record CapacityScore(
        decimal Score,
        HeatingCapacityStatus Status,
        decimal? MaximumTotalScore,
        string Explanation);
}
