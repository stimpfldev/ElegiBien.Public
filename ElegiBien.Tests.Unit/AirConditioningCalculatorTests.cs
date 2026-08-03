using ElegiBien.Application.Interfaces;
using ElegiBien.Application.Services;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Tests.Unit;

public class AirConditioningCalculatorTests
{
    private readonly AirConditioningCalculator _calculator = new();

    [Fact]
    public void Calculate_NormalRoom_ReturnsExpectedRange()
    {
        var input = CreateDefaultInput();

        var result = _calculator.Calculate(input);

        Assert.Equal(52m, result.VolumeCubicMeters);
        Assert.Equal(2600m, result.BaseFrigories);
        Assert.Equal(2600m, result.EstimatedFrigories);
        Assert.Equal(2600m, result.RecommendedMinimumFrigories);
        Assert.Equal(2860m, result.RecommendedMaximumFrigories);
        Assert.Equal(2730m, result.IdealFrigories);
        Assert.Equal(
            ConfidenceLevel.Medium,
            result.ConfidenceLevel);
        Assert.False(result.RequiresProfessionalReview);
    }

    [Fact]
    public void Calculate_HighSun_IncreasesCapacity()
    {
        var input = CreateDefaultInput();
        input.SunExposure = SunExposure.High;

        var result = _calculator.Calculate(input);

        Assert.Equal(2860m, result.EstimatedFrigories);
        Assert.Equal(2860m, result.RecommendedMinimumFrigories);
        Assert.Equal(3146m, result.RecommendedMaximumFrigories);
    }

    [Fact]
    public void Calculate_FourPeople_AddsExtraCapacity()
    {
        var input = CreateDefaultInput();
        input.PeopleCount = 4;

        var result = _calculator.Calculate(input);

        Assert.Equal(2900m, result.EstimatedFrigories);
        Assert.Equal(3190m, result.RecommendedMaximumFrigories);
    }

    [Fact]
    public void Calculate_OpenRoom_RequiresProfessionalReview()
    {
        var input = CreateDefaultInput();
        input.IsOpenToAnotherSpace = true;

        var result = _calculator.Calculate(input);

        Assert.True(result.RequiresProfessionalReview);
        Assert.Equal(
            ConfidenceLevel.Low,
            result.ConfidenceLevel);
    }

    [Fact]
    public void Calculate_InvalidLength_ThrowsException()
    {
        var input = CreateDefaultInput();
        input.LengthMeters = 0;

        Assert.Throws<ArgumentOutOfRangeException>(
            () => _calculator.Calculate(input));
    }

    private static AirConditioningInput CreateDefaultInput()
    {
        return new AirConditioningInput
        {
            AnalysisId = Guid.NewGuid(),
            LengthMeters = 5m,
            WidthMeters = 4m,
            HeightMeters = 2.60m,
            IsHeightAssumed = true,
            PeopleCount = 2,
            SunExposure = SunExposure.Medium,
            ClimateZone = ClimateZone.Temperate,
            InsulationLevel = InsulationLevel.Normal,
            WindowExposure = WindowExposure.Normal
        };
    }
}