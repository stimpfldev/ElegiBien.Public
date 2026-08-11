using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.Services;

public class PaintProductComparer : IPaintProductComparer
{
    public PaintComparisonResultDto Compare(
        PaintInput input,
        PaintCalculationResult calculation,
        PaintProductAlternativeDto firstProduct,
        PaintProductAlternativeDto secondProduct)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(calculation);

        Validate(firstProduct);
        Validate(secondProduct);

        var firstCost = CalculateTotalCost(calculation, firstProduct);
        var secondCost = CalculateTotalCost(calculation, secondProduct);
        var minimumCost = Math.Min(firstCost.TotalCost, secondCost.TotalCost);

        var first = Score(firstProduct, firstCost, minimumCost);
        var second = Score(secondProduct, secondCost, minimumCost);

        var difference = Math.Abs(first.TotalScore - second.TotalScore);

        if (difference <= 3)
        {
            return new PaintComparisonResultDto
            {
                FirstProduct = first,
                SecondProduct = second,
                IsTechnicalTie = true,
                Recommendation =
                    "Las dos pinturas presentan un empate técnico. Revisá terminación, disponibilidad y preferencia de marca."
            };
        }

        var winner = first.TotalScore > second.TotalScore
            ? first.ProductName
            : second.ProductName;

        return new PaintComparisonResultDto
        {
            FirstProduct = first,
            SecondProduct = second,
            RecommendedProductName = winner,
            Recommendation =
                $"ElegíBien recomienda {winner} porque ofrece la mejor relación entre cobertura necesaria, costo total, lavabilidad y tiempo de secado."
        };
    }

    private static CostResult CalculateTotalCost(
        PaintCalculationResult calculation,
        PaintProductAlternativeDto product)
    {
        var litersRequired =
            calculation.AdjustedAreaSquareMeters /
            product.CoverageSquareMetersPerLiterPerCoat;

        var containers =
            (int)Math.Ceiling(
                litersRequired /
                product.ContainerLiters);

        var litersPurchased =
            containers *
            product.ContainerLiters;

        var totalCost =
            containers *
            product.PricePerContainer;

        return new CostResult(
            Math.Round(litersRequired, 2),
            containers,
            litersPurchased,
            totalCost);
    }

    private static PaintProductScoreResultDto Score(
        PaintProductAlternativeDto product,
        CostResult cost,
        decimal minimumCost)
    {
        var excessRatio =
            cost.LitersPurchased /
            cost.LitersRequired;

        var coverageStatus =
            excessRatio <= 1.25m
                ? PaintCoverageStatus.Adequate
                : PaintCoverageStatus.Excessive;

        var coverageScore = excessRatio switch
        {
            <= 1.10m => 45m,
            <= 1.25m => 40m,
            <= 1.50m => 30m,
            _ => 20m
        };

        var costScore =
            Math.Round(
                30m * minimumCost / cost.TotalCost,
                2);

        var washabilityScore = product.Washability switch
        {
            PaintWashability.High => 15m,
            PaintWashability.Medium => 11m,
            PaintWashability.Low => 6m,
            _ => 4m
        };

        var dryingScore = product.DryingHours switch
        {
            null => 5m,
            <= 2m => 10m,
            <= 4m => 8m,
            <= 8m => 6m,
            _ => 3m
        };

        var washabilityText = product.Washability switch
        {
            PaintWashability.High => "Alta",
            PaintWashability.Medium => "Media",
            PaintWashability.Low => "Baja",
            _ => "No informada"
        };

        return new PaintProductScoreResultDto
        {
            ProductName = product.Name,
            TotalScore = (int)Math.Round(
                coverageScore +
                costScore +
                washabilityScore +
                dryingScore,
                MidpointRounding.AwayFromZero),
            CoverageStatus = coverageStatus,
            ConfidenceLevel =
                product.DryingHours.HasValue &&
                product.Washability != PaintWashability.Unknown
                    ? ConfidenceLevel.High
                    : ConfidenceLevel.Medium,
            ContainersRequired = cost.Containers,
            LitersRequired = cost.LitersRequired,
            LitersPurchased = cost.LitersPurchased,
            TotalCost = cost.TotalCost,
            Factors =
            [
                new PaintScoreFactorDto
                {
                    FactorType = PaintScoreFactorType.CoverageAdequacy,
                    Score = coverageScore,
                    MaximumScore = 45,
                    Explanation =
                        $"Se necesitan {cost.LitersRequired:N2} litros y se comprarían {cost.LitersPurchased:N2} litros."
                },
                new PaintScoreFactorDto
                {
                    FactorType = PaintScoreFactorType.TotalCost,
                    Score = costScore,
                    MaximumScore = 30,
                    Explanation =
                        $"Costo total estimado: ${cost.TotalCost:N2}."
                },
                new PaintScoreFactorDto
                {
                    FactorType = PaintScoreFactorType.Washability,
                    Score = washabilityScore,
                    MaximumScore = 15,
                    Explanation =
                        $"Lavabilidad informada: {washabilityText}."
                },
                new PaintScoreFactorDto
                {
                    FactorType = PaintScoreFactorType.DryingTime,
                    Score = dryingScore,
                    MaximumScore = 10,
                    Explanation = product.DryingHours.HasValue
                        ? $"Secado informado: {product.DryingHours:N1} horas."
                        : "No se informó el tiempo de secado."
                }
            ]
        };
    }

    private static void Validate(
        PaintProductAlternativeDto product)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (string.IsNullOrWhiteSpace(product.Name))
        {
            throw new ArgumentException(
                "El nombre es obligatorio.",
                nameof(product));
        }

        if (product.ContainerLiters <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(product.ContainerLiters));
        }

        if (product.PricePerContainer <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(product.PricePerContainer));
        }

        if (product.CoverageSquareMetersPerLiterPerCoat <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(product.CoverageSquareMetersPerLiterPerCoat));
        }
    }

    private sealed record CostResult(
        decimal LitersRequired,
        int Containers,
        decimal LitersPurchased,
        decimal TotalCost);
}
