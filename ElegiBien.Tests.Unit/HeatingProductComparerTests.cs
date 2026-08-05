using ElegiBien.Application.DTOs;
using ElegiBien.Application.Services;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Tests.Unit;

public class HeatingProductComparerTests
{
    private readonly HeatingProductComparer _comparer = new();

    [Fact]
    public void Compare_WithTwoEligibleProducts_ReturnsFiveFactorsPerProduct()
    {
        var result = _comparer.Compare(
            CreateCalculationResult(),
            CreateProduct("Producto A", 2700m, 500000m, 250m),
            CreateProduct("Producto B", 2800m, 550000m, 220m));

        Assert.Equal(5, result.FirstProduct.Factors.Count);
        Assert.Equal(5, result.SecondProduct.Factors.Count);
        Assert.NotNull(result.RecommendedProductName);
    }

    [Fact]
    public void Compare_WithInsufficientProduct_MarksItAsNotEligible()
    {
        var result = _comparer.Compare(
            CreateCalculationResult(),
            CreateProduct("Insuficiente", 1800m, 300000m, 100m),
            CreateProduct("Adecuado", 2700m, 500000m, 250m));

        Assert.False(result.FirstProduct.IsEligible);
        Assert.Equal(HeatingCapacityStatus.Insufficient, result.FirstProduct.CapacityStatus);
        Assert.Equal("Adecuado", result.RecommendedProductName);
    }

    [Fact]
    public void Compare_WithEqualProducts_ReturnsTie()
    {
        var first = CreateProduct("Producto A", 2700m, 500000m, 250m);
        var second = CreateProduct("Producto B", 2700m, 500000m, 250m);

        var result = _comparer.Compare(CreateCalculationResult(), first, second);

        Assert.True(result.IsTie);
        Assert.Null(result.RecommendedProductName);
    }

    [Fact]
    public void Compare_WithInvalidCapacity_Throws()
    {
        var product = CreateProduct("Inválido", 0m, 500000m, 250m);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _comparer.Compare(CreateCalculationResult(), product, CreateProduct("Válido", 2700m, 500000m, 250m)));
    }

    private static HeatingCalculationResult CreateCalculationResult()
    {
        return new HeatingCalculationResult
        {
            RecommendedMinimumWatts = 2470m,
            RecommendedMaximumWatts = 2860m,
            IdealPowerWatts = 2665m
        };
    }

    private static HeatingProductAlternativeDto CreateProduct(
        string name,
        decimal capacityWatts,
        decimal purchasePrice,
        decimal hourlyCost)
    {
        return new HeatingProductAlternativeDto
        {
            Name = name,
            SystemType = HeatingSystemType.HeatPumpAirConditioner,
            HeatingCapacityWatts = capacityWatts,
            PurchasePrice = purchasePrice,
            EstimatedHourlyCost = hourlyCost,
            EfficiencyLevel = HeatingEfficiencyLevel.High,
            SafetyLevel = HeatingSafetyLevel.StandardInstallation
        };
    }
}
