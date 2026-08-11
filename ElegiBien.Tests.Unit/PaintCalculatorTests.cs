using ElegiBien.Application.Services;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Tests.Unit;

public class PaintCalculatorTests
{
    [Fact]
    public void Calculate_ReturnsExpectedReferenceLiters()
    {
        var calculator = new PaintCalculator();

        var result = calculator.Calculate(
            new PaintInput
            {
                AnalysisId = Guid.NewGuid(),
                LengthMeters = 5m,
                WidthMeters = 4m,
                HeightMeters = 2.6m,
                DoorCount = 1,
                WindowCount = 1,
                CoatCount = 2,
                SurfaceCondition = PaintSurfaceCondition.Good,
                WastePercentage = 10m
            });

        Assert.Equal(43.5m, result.NetAreaSquareMeters);
        Assert.Equal(9.57m, result.ReferenceLiters);
    }

    [Fact]
    public void Calculate_IncludingCeiling_IncreasesNetArea()
    {
        var calculator = new PaintCalculator();

        var withoutCeiling = calculator.Calculate(
            CreateInput(includeCeiling: false));

        var withCeiling = calculator.Calculate(
            CreateInput(includeCeiling: true));

        Assert.True(
            withCeiling.NetAreaSquareMeters >
            withoutCeiling.NetAreaSquareMeters);
    }

    [Fact]
    public void Calculate_NewOrPorousSurface_IncreasesReferenceLiters()
    {
        var calculator = new PaintCalculator();

        var goodSurface = calculator.Calculate(
            CreateInput(PaintSurfaceCondition.Good));

        var porousSurface = calculator.Calculate(
            CreateInput(PaintSurfaceCondition.NewOrPorous));

        Assert.True(
            porousSurface.ReferenceLiters >
            goodSurface.ReferenceLiters);
    }

    [Fact]
    public void Calculate_DamagedSurface_RequiresProfessionalReview()
    {
        var result = new PaintCalculator().Calculate(
            CreateInput(PaintSurfaceCondition.Damaged));

        Assert.True(result.RequiresProfessionalReview);
    }

    [Fact]
    public void Calculate_InvalidDimensions_ThrowsException()
    {
        var input = CreateInput();
        input.LengthMeters = 0m;

        Assert.Throws<ArgumentOutOfRangeException>(
            () => new PaintCalculator().Calculate(input));
    }

    private static PaintInput CreateInput(
        PaintSurfaceCondition surfaceCondition =
            PaintSurfaceCondition.Good,
        bool includeCeiling = false)
    {
        return new PaintInput
        {
            AnalysisId = Guid.NewGuid(),
            LengthMeters = 4m,
            WidthMeters = 4m,
            HeightMeters = 2.6m,
            IncludeCeiling = includeCeiling,
            DoorCount = 1,
            WindowCount = 1,
            CoatCount = 2,
            SurfaceCondition = surfaceCondition,
            WastePercentage = 10m
        };
    }
}
