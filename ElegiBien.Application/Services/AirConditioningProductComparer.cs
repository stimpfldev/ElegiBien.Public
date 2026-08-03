using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.Services;

public class AirConditioningProductComparer
    : IAirConditioningProductComparer
{
    public ProductComparisonResultDto Compare(
        DimensioningResult dimensioningResult,
        ProductAlternativeDto firstProduct,
        ProductAlternativeDto secondProduct)
    {
        ArgumentNullException.ThrowIfNull(dimensioningResult);
        ArgumentNullException.ThrowIfNull(firstProduct);
        ArgumentNullException.ThrowIfNull(secondProduct);

        ValidateProduct(firstProduct);
        ValidateProduct(secondProduct);

        var firstEligible = IsEligible(
            dimensioningResult,
            firstProduct.CapacityFrigories);

        var secondEligible = IsEligible(
            dimensioningResult,
            secondProduct.CapacityFrigories);

        var minimumEligiblePrice = GetMinimumEligiblePrice(
            firstProduct,
            secondProduct,
            firstEligible,
            secondEligible);

        var consumptionComparable =
            firstProduct.NominalConsumptionWatts.HasValue &&
            secondProduct.NominalConsumptionWatts.HasValue;

        decimal? bestRelativeConsumption = null;

        if (consumptionComparable)
        {
            var firstRelativeConsumption =
                CalculateRelativeConsumption(firstProduct);

            var secondRelativeConsumption =
                CalculateRelativeConsumption(secondProduct);

            bestRelativeConsumption = Math.Min(
                firstRelativeConsumption,
                secondRelativeConsumption);
        }

        var firstScore = CalculateScore(
            dimensioningResult,
            firstProduct,
            firstEligible,
            minimumEligiblePrice,
            bestRelativeConsumption,
            consumptionComparable);

        var secondScore = CalculateScore(
            dimensioningResult,
            secondProduct,
            secondEligible,
            minimumEligiblePrice,
            bestRelativeConsumption,
            consumptionComparable);

        return BuildComparisonResult(
            firstScore,
            secondScore);
    }

    private static ProductScoreResultDto CalculateScore(
        DimensioningResult dimensioningResult,
        ProductAlternativeDto product,
        bool isEligible,
        decimal? minimumEligiblePrice,
        decimal? bestRelativeConsumption,
        bool consumptionComparable)
    {
        var capacityResult = CalculateCapacityScore(
            dimensioningResult,
            product.CapacityFrigories);

        var efficiencyScore = CalculateEfficiencyScore(
            product,
            bestRelativeConsumption,
            consumptionComparable);

        var priceScore = CalculatePriceScore(
            product,
            isEligible,
            minimumEligiblePrice);

        var warrantyScore =
            CalculateWarrantyScore(product.WarrantyMonths);

        var total =
            capacityResult.Score +
            efficiencyScore +
            priceScore +
            warrantyScore;

        if (capacityResult.MaximumTotalScore.HasValue)
        {
            total = Math.Min(
                total,
                capacityResult.MaximumTotalScore.Value);
        }

        return new ProductScoreResultDto
        {
            ProductName = product.Name,
            TotalScore = (int)Math.Round(
                total,
                MidpointRounding.AwayFromZero),
            CapacityStatus = capacityResult.Status,
            ConfidenceLevel = consumptionComparable
                ? ConfidenceLevel.High
                : ConfidenceLevel.Medium,
            IsEligible = isEligible,
            AppliedMaximumScore =
                capacityResult.MaximumTotalScore,
            Factors =
            [
                new ScoreFactorDto
                {
                    FactorType =
                        ScoreFactorType.CapacityAdequacy,
                    Score = capacityResult.Score,
                    MaximumScore = 55,
                    Explanation =
                        capacityResult.Explanation
                },
                new ScoreFactorDto
                {
                    FactorType =
                        ScoreFactorType.Efficiency,
                    Score = efficiencyScore,
                    MaximumScore = 20,
                    Explanation = consumptionComparable
                        ? "Se comparó el consumo nominal por cada 1.000 frigorías."
                        : "El consumo exacto no estaba disponible; se utilizó la tecnología como aproximación."
                },
                new ScoreFactorDto
                {
                    FactorType =
                        ScoreFactorType.RelativePrice,
                    Score = priceScore,
                    MaximumScore = 15,
                    Explanation = isEligible
                        ? "El precio se comparó únicamente entre alternativas técnicamente elegibles."
                        : "No recibió puntos de precio porque la capacidad no es elegible."
                },
                new ScoreFactorDto
                {
                    FactorType =
                        ScoreFactorType.Warranty,
                    Score = warrantyScore,
                    MaximumScore = 10,
                    Explanation =
                        $"Garantía informada: {product.WarrantyMonths} meses."
                }
            ]
        };
    }

    private static CapacityScoreResult CalculateCapacityScore(
        DimensioningResult result,
        decimal capacity)
    {
        var minimum =
            result.RecommendedMinimumFrigories;

        var maximum =
            result.RecommendedMaximumFrigories;

        if (capacity >= minimum &&
            capacity <= maximum)
        {
            return new CapacityScoreResult(
                55,
                CapacityFitStatus.Correct,
                null,
                "La capacidad está dentro del rango recomendado.");
        }

        if (capacity < minimum)
        {
            var difference =
                (minimum - capacity) / minimum;

            if (difference <= 0.10m)
            {
                return new CapacityScoreResult(
                    35,
                    CapacityFitStatus.Insufficient,
                    74,
                    "La capacidad está hasta un 10 % por debajo del mínimo recomendado.");
            }

            if (difference <= 0.20m)
            {
                return new CapacityScoreResult(
                    20,
                    CapacityFitStatus.Insufficient,
                    59,
                    "La capacidad está entre un 10 % y un 20 % por debajo del mínimo recomendado.");
            }

            return new CapacityScoreResult(
                0,
                CapacityFitStatus.Insufficient,
                39,
                "La capacidad está más de un 20 % por debajo del mínimo recomendado.");
        }

        var excess =
            (capacity - maximum) / maximum;

        if (excess <= 0.10m)
        {
            return new CapacityScoreResult(
                45,
                CapacityFitStatus.AcceptableOversized,
                null,
                "La capacidad supera hasta un 10 % el máximo recomendado.");
        }

        if (excess <= 0.20m)
        {
            return new CapacityScoreResult(
                30,
                CapacityFitStatus.AcceptableOversized,
                74,
                "La capacidad supera entre un 10 % y un 20 % el máximo recomendado.");
        }

        return new CapacityScoreResult(
            10,
            CapacityFitStatus.RelevantOversized,
            59,
            "La capacidad supera más de un 20 % el máximo recomendado.");
    }

    private static decimal CalculateEfficiencyScore(
        ProductAlternativeDto product,
        decimal? bestRelativeConsumption,
        bool consumptionComparable)
    {
        if (consumptionComparable &&
            bestRelativeConsumption.HasValue)
        {
            var relativeConsumption =
                CalculateRelativeConsumption(product);

            return Math.Round(
                20m *
                bestRelativeConsumption.Value /
                relativeConsumption,
                2);
        }

        return product.Technology switch
        {
            AirConditionerTechnology.Inverter => 14m,
            AirConditionerTechnology.Conventional => 8m,
            _ => 5m
        };
    }

    private static decimal CalculatePriceScore(
        ProductAlternativeDto product,
        bool isEligible,
        decimal? minimumEligiblePrice)
    {
        if (!isEligible ||
            !minimumEligiblePrice.HasValue)
        {
            return 0m;
        }

        return Math.Round(
            15m *
            minimumEligiblePrice.Value /
            product.Price,
            2);
    }

    private static decimal CalculateWarrantyScore(
        int warrantyMonths)
    {
        return warrantyMonths switch
        {
            <= 0 => 0m,
            <= 12 => 4m,
            <= 24 => 6m,
            <= 36 => 8m,
            <= 59 => 9m,
            _ => 10m
        };
    }

    private static bool IsEligible(
        DimensioningResult result,
        decimal capacity)
    {
        var maximumAllowed =
            result.RecommendedMaximumFrigories * 1.10m;

        return capacity >=
                   result.RecommendedMinimumFrigories &&
               capacity <= maximumAllowed;
    }

    private static decimal? GetMinimumEligiblePrice(
        ProductAlternativeDto firstProduct,
        ProductAlternativeDto secondProduct,
        bool firstEligible,
        bool secondEligible)
    {
        var eligiblePrices = new List<decimal>();

        if (firstEligible)
        {
            eligiblePrices.Add(firstProduct.Price);
        }

        if (secondEligible)
        {
            eligiblePrices.Add(secondProduct.Price);
        }

        return eligiblePrices.Count == 0
            ? null
            : eligiblePrices.Min();
    }

    private static decimal CalculateRelativeConsumption(
        ProductAlternativeDto product)
    {
        return
            product.NominalConsumptionWatts!.Value /
            product.CapacityFrigories *
            1000m;
    }

    private static ProductComparisonResultDto BuildComparisonResult(
        ProductScoreResultDto first,
        ProductScoreResultDto second)
    {
        if (!first.IsEligible &&
            !second.IsEligible)
        {
            return new ProductComparisonResultDto
            {
                FirstProduct = first,
                SecondProduct = second,
                HasRecommendedProduct = false,
                Recommendation =
                    "Ninguna de las alternativas analizadas se adapta correctamente a la capacidad necesaria."
            };
        }

        if (first.IsEligible &&
            !second.IsEligible)
        {
            return Recommended(
                first,
                second,
                first.ProductName);
        }

        if (!first.IsEligible &&
            second.IsEligible)
        {
            return Recommended(
                first,
                second,
                second.ProductName);
        }

        var difference =
            Math.Abs(first.TotalScore - second.TotalScore);

        if (difference <= 3)
        {
            return new ProductComparisonResultDto
            {
                FirstProduct = first,
                SecondProduct = second,
                IsTechnicalTie = true,
                HasRecommendedProduct = false,
                Recommendation =
                    "Las dos alternativas presentan un empate técnico. Revisá precio, consumo y garantía según tus prioridades."
            };
        }

        var recommended =
            first.TotalScore > second.TotalScore
                ? first.ProductName
                : second.ProductName;

        return Recommended(
            first,
            second,
            recommended);
    }

    private static ProductComparisonResultDto Recommended(
        ProductScoreResultDto first,
        ProductScoreResultDto second,
        string recommendedProduct)
    {
        return new ProductComparisonResultDto
        {
            FirstProduct = first,
            SecondProduct = second,
            HasRecommendedProduct = true,
            RecommendedProductName = recommendedProduct,
            Recommendation =
                $"ElegíBien recomienda {recommendedProduct} porque presenta la mejor adecuación general entre las alternativas analizadas."
        };
    }

    private static void ValidateProduct(
        ProductAlternativeDto product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            throw new ArgumentException(
                "El nombre del producto es obligatorio.");
        }

        if (product.CapacityFrigories <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(product.CapacityFrigories));
        }

        if (product.Price <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(product.Price));
        }

        if (product.NominalConsumptionWatts <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(product.NominalConsumptionWatts));
        }
    }

    private sealed record CapacityScoreResult(
        decimal Score,
        CapacityFitStatus Status,
        int? MaximumTotalScore,
        string Explanation);
}