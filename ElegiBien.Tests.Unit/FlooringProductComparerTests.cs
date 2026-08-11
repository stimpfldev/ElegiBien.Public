using ElegiBien.Application.DTOs;
using ElegiBien.Application.Services;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Tests.Unit;

public class FlooringProductComparerTests
{
    [Fact]
    public void Compare_RecommendsBetterOverallOption()
    {
        var result = new FlooringProductComparer().Compare(
            CreateCalculation(),
            CreateFirstProduct(),
            CreateSecondProduct());

        Assert.Equal(
            "Piso A",
            result.RecommendedProductName);
    }

    [Fact]
    public void Compare_CalculatesCompleteBoxesAndExcess()
    {
        var result = new FlooringProductComparer().Compare(
            CreateCalculation(),
            CreateFirstProduct(),
            CreateSecondProduct());

        Assert.Equal(10, result.FirstProduct.BoxesRequired);
        Assert.Equal(22m, result.FirstProduct.PurchasedAreaSquareMeters);
        Assert.Equal(0m, result.FirstProduct.ExcessAreaSquareMeters);
    }

    [Fact]
    public void Compare_UsesFiveScoreFactorsPerProduct()
    {
        var result = new FlooringProductComparer().Compare(
            CreateCalculation(),
            CreateFirstProduct(),
            CreateSecondProduct());

        Assert.Equal(5, result.FirstProduct.Factors.Count);
        Assert.Equal(5, result.SecondProduct.Factors.Count);
    }

    [Fact]
    public void Compare_InvalidBoxCoverage_ThrowsException()
    {
        var invalidProduct = CreateFirstProduct();
        invalidProduct.CoverageSquareMetersPerBox = 0m;

        Assert.Throws<ArgumentOutOfRangeException>(
            () => new FlooringProductComparer().Compare(
                CreateCalculation(),
                invalidProduct,
                CreateSecondProduct()));
    }

    private static FlooringCalculationResult CreateCalculation()
    {
        return new FlooringCalculationResult
        {
            RequiredAreaSquareMeters = 22m
        };
    }

    private static FlooringProductAlternativeDto CreateFirstProduct()
    {
        return new FlooringProductAlternativeDto
        {
            Name = "Piso A",
            CoverageSquareMetersPerBox = 2.20m,
            PricePerBox = 30000m,
            UseResistance = FlooringUseResistance.High,
            ReplacementEase = FlooringReplacementEase.High
        };
    }

    private static FlooringProductAlternativeDto CreateSecondProduct()
    {
        return new FlooringProductAlternativeDto
        {
            Name = "Piso B",
            CoverageSquareMetersPerBox = 2m,
            PricePerBox = 26000m,
            UseResistance = FlooringUseResistance.Medium,
            ReplacementEase = FlooringReplacementEase.Low
        };
    }
}
