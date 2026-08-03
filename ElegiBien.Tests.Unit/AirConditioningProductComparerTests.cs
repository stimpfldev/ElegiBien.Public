using ElegiBien.Application.DTOs;
using ElegiBien.Application.Services;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Tests.Unit;

public class AirConditioningProductComparerTests
{
    private readonly AirConditioningProductComparer _comparer = new();

    [Fact]
    public void Compare_CorrectProductAgainstCheaperInsufficientProduct_SelectsCorrectProduct()
    {
        var result = _comparer.Compare(
            CreateDimensioningResult(),
            CreateProduct(
                "Equipo correcto",
                2700,
                800000,
                AirConditionerTechnology.Inverter,
                900,
                36),
            CreateProduct(
                "Equipo insuficiente",
                2200,
                600000,
                AirConditionerTechnology.Inverter,
                750,
                36));

        Assert.True(result.HasRecommendedProduct);
        Assert.Equal(
            "Equipo correcto",
            result.RecommendedProductName);
        Assert.True(result.FirstProduct.IsEligible);
        Assert.False(result.SecondProduct.IsEligible);
    }

    [Fact]
    public void Compare_TwoCorrectProducts_ReturnsHigherScore()
    {
        var result = _comparer.Compare(
            CreateDimensioningResult(),
            CreateProduct(
                "Equipo A",
                2700,
                800000,
                AirConditionerTechnology.Inverter,
                850,
                36),
            CreateProduct(
                "Equipo B",
                2750,
                900000,
                AirConditionerTechnology.Conventional,
                1100,
                12));

        Assert.True(result.HasRecommendedProduct);
        Assert.Equal(
            "Equipo A",
            result.RecommendedProductName);
        Assert.True(
            result.FirstProduct.TotalScore >
            result.SecondProduct.TotalScore);
    }

    [Fact]
    public void Compare_TwoInvalidProducts_RecommendsNeither()
    {
        var result = _comparer.Compare(
            CreateDimensioningResult(),
            CreateProduct(
                "Equipo insuficiente",
                2000,
                500000,
                AirConditionerTechnology.Inverter,
                null,
                12),
            CreateProduct(
                "Equipo excesivo",
                4000,
                900000,
                AirConditionerTechnology.Inverter,
                null,
                36));

        Assert.False(result.HasRecommendedProduct);
        Assert.False(result.FirstProduct.IsEligible);
        Assert.False(result.SecondProduct.IsEligible);
    }

    [Fact]
    public void Compare_MissingConsumption_UsesMediumConfidence()
    {
        var result = _comparer.Compare(
            CreateDimensioningResult(),
            CreateProduct(
                "Equipo A",
                2700,
                800000,
                AirConditionerTechnology.Inverter,
                null,
                36),
            CreateProduct(
                "Equipo B",
                2750,
                850000,
                AirConditionerTechnology.Conventional,
                null,
                24));

        Assert.Equal(
            ConfidenceLevel.Medium,
            result.FirstProduct.ConfidenceLevel);

        Assert.Equal(
            ConfidenceLevel.Medium,
            result.SecondProduct.ConfidenceLevel);
    }

    private static DimensioningResult CreateDimensioningResult()
    {
        return new DimensioningResult
        {
            RecommendedMinimumFrigories = 2600,
            RecommendedMaximumFrigories = 2860,
            IdealFrigories = 2730,
            ConfidenceLevel = ConfidenceLevel.Medium
        };
    }

    private static ProductAlternativeDto CreateProduct(
        string name,
        decimal capacity,
        decimal price,
        AirConditionerTechnology technology,
        decimal? consumption,
        int warrantyMonths)
    {
        return new ProductAlternativeDto
        {
            Name = name,
            CapacityFrigories = capacity,
            Price = price,
            Technology = technology,
            NominalConsumptionWatts = consumption,
            WarrantyMonths = warrantyMonths
        };
    }
}