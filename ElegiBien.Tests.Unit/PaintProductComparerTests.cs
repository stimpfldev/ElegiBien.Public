using ElegiBien.Application.DTOs;
using ElegiBien.Application.Services;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Tests.Unit;

public class PaintProductComparerTests
{
    [Fact]
    public void Compare_RecommendsBetterOverallOption()
    {
        var result = new PaintProductComparer().Compare(
            CreateInput(),
            CreateCalculation(),
            CreateFirstProduct(),
            CreateSecondProduct());

        Assert.Equal(
            "Pintura A",
            result.RecommendedProductName);
    }

    [Fact]
    public void Compare_CalculatesCompleteContainers()
    {
        var result = new PaintProductComparer().Compare(
            CreateInput(),
            CreateCalculation(),
            CreateFirstProduct(),
            CreateSecondProduct());

        Assert.True(
            result.FirstProduct.ContainersRequired >= 1);

        Assert.True(
            result.FirstProduct.LitersPurchased >=
            result.FirstProduct.LitersRequired);
    }

    [Fact]
    public void Compare_UsesFourScoreFactorsPerProduct()
    {
        var result = new PaintProductComparer().Compare(
            CreateInput(),
            CreateCalculation(),
            CreateFirstProduct(),
            CreateSecondProduct());

        Assert.Equal(
            4,
            result.FirstProduct.Factors.Count);

        Assert.Equal(
            4,
            result.SecondProduct.Factors.Count);
    }

    [Fact]
    public void Compare_InvalidContainerSize_ThrowsException()
    {
        var invalidProduct = CreateFirstProduct();
        invalidProduct.ContainerLiters = 0m;

        Assert.Throws<ArgumentOutOfRangeException>(
            () => new PaintProductComparer().Compare(
                CreateInput(),
                CreateCalculation(),
                invalidProduct,
                CreateSecondProduct()));
    }

    private static PaintInput CreateInput()
    {
        return new PaintInput
        {
            CoatCount = 2
        };
    }

    private static PaintCalculationResult CreateCalculation()
    {
        return new PaintCalculationResult
        {
            AdjustedAreaSquareMeters = 90m
        };
    }

    private static PaintProductAlternativeDto CreateFirstProduct()
    {
        return new PaintProductAlternativeDto
        {
            Name = "Pintura A",
            ContainerLiters = 10m,
            PricePerContainer = 100000m,
            CoverageSquareMetersPerLiterPerCoat = 10m,
            Washability = PaintWashability.High,
            DryingHours = 2m
        };
    }

    private static PaintProductAlternativeDto CreateSecondProduct()
    {
        return new PaintProductAlternativeDto
        {
            Name = "Pintura B",
            ContainerLiters = 10m,
            PricePerContainer = 90000m,
            CoverageSquareMetersPerLiterPerCoat = 8m,
            Washability = PaintWashability.Low,
            DryingHours = 8m
        };
    }
}
