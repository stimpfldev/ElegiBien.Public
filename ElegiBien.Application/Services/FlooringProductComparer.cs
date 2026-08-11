using ElegiBien.Application.DTOs;
using ElegiBien.Application.Interfaces;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.Services;

public class FlooringProductComparer : IFlooringProductComparer
{
    public FlooringComparisonResultDto Compare(
        FlooringCalculationResult calculation,
        FlooringProductAlternativeDto firstProduct,
        FlooringProductAlternativeDto secondProduct)
    {
        ArgumentNullException.ThrowIfNull(calculation);

        Validate(firstProduct);
        Validate(secondProduct);

        if (calculation.RequiredAreaSquareMeters <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(calculation.RequiredAreaSquareMeters));
        }

        var firstPurchase = CalculatePurchase(calculation, firstProduct);
        var secondPurchase = CalculatePurchase(calculation, secondProduct);
        var minimumCost = Math.Min(firstPurchase.TotalCost, secondPurchase.TotalCost);

        var first = Score(firstProduct, firstPurchase, minimumCost);
        var second = Score(secondProduct, secondPurchase, minimumCost);
        var difference = Math.Abs(first.TotalScore - second.TotalScore);

        if (difference <= 3)
        {
            return new FlooringComparisonResultDto
            {
                FirstProduct = first,
                SecondProduct = second,
                IsTechnicalTie = true,
                Recommendation =
                    "Los dos productos presentan un empate técnico. Revisá disponibilidad, terminación y posibilidad de conseguir cajas del mismo lote en el futuro."
            };
        }

        var winner = first.TotalScore > second.TotalScore
            ? first.ProductName
            : second.ProductName;

        return new FlooringComparisonResultDto
        {
            FirstProduct = first,
            SecondProduct = second,
            RecommendedProductName = winner,
            Recommendation =
                $"ElegíBien recomienda {winner} porque ofrece la mejor relación entre cobertura comprada, costo total, excedente, resistencia y facilidad de reposición."
        };
    }

    private static PurchaseResult CalculatePurchase(
        FlooringCalculationResult calculation,
        FlooringProductAlternativeDto product)
    {
        var boxes = (int)Math.Ceiling(
            calculation.RequiredAreaSquareMeters /
            product.CoverageSquareMetersPerBox);

        var purchasedArea = boxes * product.CoverageSquareMetersPerBox;
        var excessArea = purchasedArea - calculation.RequiredAreaSquareMeters;
        var excessPercentage =
            excessArea /
            calculation.RequiredAreaSquareMeters *
            100m;
        var totalCost = boxes * product.PricePerBox;

        return new PurchaseResult(
            boxes,
            Math.Round(purchasedArea, 2),
            Math.Round(excessArea, 2),
            Math.Round(excessPercentage, 2),
            totalCost);
    }

    private static FlooringProductScoreResultDto Score(
        FlooringProductAlternativeDto product,
        PurchaseResult purchase,
        decimal minimumCost)
    {
        var coverageStatus = purchase.ExcessPercentage switch
        {
            <= 15m => FlooringCoverageStatus.Adequate,
            _ => FlooringCoverageStatus.Excessive
        };

        var coverageScore = purchase.ExcessPercentage switch
        {
            <= 5m => 35m,
            <= 10m => 32m,
            <= 15m => 28m,
            <= 25m => 22m,
            _ => 15m
        };

        var costScore = Math.Round(
            30m * minimumCost / purchase.TotalCost,
            2);

        var wasteScore = purchase.ExcessPercentage switch
        {
            <= 5m => 10m,
            <= 10m => 8m,
            <= 15m => 6m,
            <= 25m => 4m,
            _ => 2m
        };

        var resistanceScore = product.UseResistance switch
        {
            FlooringUseResistance.VeryHigh => 15m,
            FlooringUseResistance.High => 13m,
            FlooringUseResistance.Medium => 10m,
            FlooringUseResistance.Light => 6m,
            _ => 5m
        };

        var replacementScore = product.ReplacementEase switch
        {
            FlooringReplacementEase.High => 10m,
            FlooringReplacementEase.Medium => 7m,
            FlooringReplacementEase.Low => 4m,
            _ => 3m
        };

        var confidence =
            product.UseResistance != FlooringUseResistance.Unknown &&
            product.ReplacementEase != FlooringReplacementEase.Unknown
                ? ConfidenceLevel.High
                : ConfidenceLevel.Medium;

        return new FlooringProductScoreResultDto
        {
            ProductName = product.Name,
            TotalScore = (int)Math.Round(
                coverageScore +
                costScore +
                wasteScore +
                resistanceScore +
                replacementScore,
                MidpointRounding.AwayFromZero),
            CoverageStatus = coverageStatus,
            ConfidenceLevel = confidence,
            BoxesRequired = purchase.Boxes,
            RequiredAreaSquareMeters = purchase.RequiredAreaSquareMeters,
            PurchasedAreaSquareMeters = purchase.PurchasedAreaSquareMeters,
            ExcessAreaSquareMeters = purchase.ExcessAreaSquareMeters,
            ExcessPercentage = purchase.ExcessPercentage,
            TotalCost = purchase.TotalCost,
            Factors =
            [
                new FlooringScoreFactorDto
                {
                    FactorType = FlooringScoreFactorType.ActualCoverage,
                    Score = coverageScore,
                    MaximumScore = 35m,
                    Explanation =
                        $"Se necesitan {purchase.RequiredAreaSquareMeters:N2} m² y se comprarían {purchase.PurchasedAreaSquareMeters:N2} m²."
                },
                new FlooringScoreFactorDto
                {
                    FactorType = FlooringScoreFactorType.TotalCost,
                    Score = costScore,
                    MaximumScore = 30m,
                    Explanation =
                        $"Costo total estimado: ${purchase.TotalCost:N2}."
                },
                new FlooringScoreFactorDto
                {
                    FactorType = FlooringScoreFactorType.EstimatedWaste,
                    Score = wasteScore,
                    MaximumScore = 10m,
                    Explanation =
                        $"Excedente por cajas completas: {purchase.ExcessAreaSquareMeters:N2} m² ({purchase.ExcessPercentage:N2} %)."
                },
                new FlooringScoreFactorDto
                {
                    FactorType = FlooringScoreFactorType.UseResistance,
                    Score = resistanceScore,
                    MaximumScore = 15m,
                    Explanation =
                        $"Resistencia de uso informada: {GetResistanceText(product.UseResistance)}."
                },
                new FlooringScoreFactorDto
                {
                    FactorType = FlooringScoreFactorType.ReplacementEase,
                    Score = replacementScore,
                    MaximumScore = 10m,
                    Explanation =
                        $"Facilidad de reposición informada: {GetReplacementText(product.ReplacementEase)}."
                }
            ]
        };
    }

    private static void Validate(FlooringProductAlternativeDto product)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (string.IsNullOrWhiteSpace(product.Name))
        {
            throw new ArgumentException(
                "El nombre es obligatorio.",
                nameof(product));
        }

        if (product.CoverageSquareMetersPerBox <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(product.CoverageSquareMetersPerBox));
        }

        if (product.PricePerBox <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(product.PricePerBox));
        }
    }

    private static string GetResistanceText(FlooringUseResistance resistance)
    {
        return resistance switch
        {
            FlooringUseResistance.VeryHigh => "Muy alta",
            FlooringUseResistance.High => "Alta",
            FlooringUseResistance.Medium => "Media",
            FlooringUseResistance.Light => "Liviana",
            _ => "No informada"
        };
    }

    private static string GetReplacementText(FlooringReplacementEase replacementEase)
    {
        return replacementEase switch
        {
            FlooringReplacementEase.High => "Alta",
            FlooringReplacementEase.Medium => "Media",
            FlooringReplacementEase.Low => "Baja",
            _ => "No informada"
        };
    }

    private sealed record PurchaseResult(
        int Boxes,
        decimal PurchasedAreaSquareMeters,
        decimal ExcessAreaSquareMeters,
        decimal ExcessPercentage,
        decimal TotalCost)
    {
        public decimal RequiredAreaSquareMeters =>
            PurchasedAreaSquareMeters - ExcessAreaSquareMeters;
    }
}
