using ElegiBien.Application.Services;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Tests.Unit;

public class HeatingCalculatorTests
{
    private readonly HeatingCalculator _calculator = new();

    [Fact]
    public void Calculate_WithStandardRoom_ReturnsExpectedRange()
    {
        var input = CreateInput();

        var result = _calculator.Calculate(input);

        Assert.Equal(20m, result.SurfaceSquareMeters);
        Assert.Equal(52m, result.VolumeCubicMeters);
        Assert.Equal(2600m, result.BasePowerWatts);
        Assert.Equal(2470m, result.RecommendedMinimumWatts);
        Assert.Equal(2860m, result.RecommendedMaximumWatts);
        Assert.Equal(2665m, result.IdealPowerWatts);
        Assert.Equal(2292m, result.IdealPowerKcalPerHour);
        Assert.Equal(ConfidenceLevel.High, result.ConfidenceLevel);
        Assert.False(result.RequiresProfessionalReview);
    }

    [Fact]
    public void Calculate_WithPoorInsulationAndColdClimate_IncreasesRequiredPower()
    {
        var input = CreateInput();
        input.ClimateZone = HeatingClimateZone.Cold;
        input.InsulationLevel = InsulationLevel.Poor;

        var result = _calculator.Calculate(input);

        Assert.True(result.IdealPowerWatts > 2665m);
    }

    [Fact]
    public void Calculate_WithVeryColdZone_RequiresProfessionalReview()
    {
        var input = CreateInput();
        input.ClimateZone = HeatingClimateZone.VeryCold;

        var result = _calculator.Calculate(input);

        Assert.True(result.RequiresProfessionalReview);
        Assert.Equal(ConfidenceLevel.Low, result.ConfidenceLevel);
    }

    [Fact]
    public void Calculate_WithInvalidLength_Throws()
    {
        var input = CreateInput();
        input.LengthMeters = 0m;

        Assert.Throws<ArgumentOutOfRangeException>(() => _calculator.Calculate(input));
    }

    [Fact]
    public void Calculate_WithInvalidExteriorWallsCount_Throws()
    {
        var input = CreateInput();
        input.ExteriorWallsCount = 5;

        Assert.Throws<ArgumentOutOfRangeException>(() => _calculator.Calculate(input));
    }

    private static HeatingInput CreateInput()
    {
        return new HeatingInput
        {
            AnalysisId = Guid.NewGuid(),
            LengthMeters = 5m,
            WidthMeters = 4m,
            HeightMeters = 2.60m,
            ClimateZone = HeatingClimateZone.TemperateCold,
            InsulationLevel = InsulationLevel.Normal,
            ExteriorWallsCount = 1,
            WindowExposure = WindowExposure.Normal,
            IsOpenToAnotherSpace = false
        };
    }
}
