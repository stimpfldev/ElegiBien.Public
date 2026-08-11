using ElegiBien.Application.Services;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Tests.Unit;

public class FlooringCalculatorTests
{
    [Theory]
    [InlineData(FlooringInstallationPattern.Straight, 10)]
    [InlineData(FlooringInstallationPattern.Staggered, 12)]
    [InlineData(FlooringInstallationPattern.Diagonal, 15)]
    public void GetRecommendedWastePercentage_ReturnsValueByInstallationPattern(
        FlooringInstallationPattern installationPattern,
        decimal expectedPercentage)
    {
        var result = new FlooringCalculator()
            .GetRecommendedWastePercentage(installationPattern);

        Assert.Equal(expectedPercentage, result);
    }

    [Fact]
    public void Calculate_ReturnsExpectedAreaAndWaste()
    {
        var result = new FlooringCalculator().Calculate(
            CreateInput(
                lengthMeters: 5m,
                widthMeters: 4m,
                wastePercentage: 10m));

        Assert.Equal(20m, result.TotalAreaSquareMeters);
        Assert.Equal(2m, result.WasteAreaSquareMeters);
        Assert.Equal(22m, result.RequiredAreaSquareMeters);
    }

    [Fact]
    public void Calculate_DiagonalInstallation_RequiresProfessionalReview()
    {
        var input = CreateInput();
        input.InstallationPattern = FlooringInstallationPattern.Diagonal;
        input.WastePercentage = 15m;

        var result = new FlooringCalculator().Calculate(input);

        Assert.True(result.RequiresProfessionalReview);
        Assert.Equal(ConfidenceLevel.Medium, result.ConfidenceLevel);
    }

    [Fact]
    public void Calculate_StraightInstallation_ReturnsHighConfidence()
    {
        var result = new FlooringCalculator().Calculate(CreateInput());

        Assert.Equal(ConfidenceLevel.High, result.ConfidenceLevel);
    }

    [Fact]
    public void Calculate_InvalidDimensions_ThrowsException()
    {
        var input = CreateInput();
        input.LengthMeters = 0m;

        Assert.Throws<ArgumentOutOfRangeException>(
            () => new FlooringCalculator().Calculate(input));
    }

    [Fact]
    public void Calculate_InvalidWastePercentage_ThrowsException()
    {
        var input = CreateInput();
        input.WastePercentage = 31m;

        Assert.Throws<ArgumentOutOfRangeException>(
            () => new FlooringCalculator().Calculate(input));
    }

    private static FlooringInput CreateInput(
        decimal lengthMeters = 4m,
        decimal widthMeters = 4m,
        decimal wastePercentage = 10m)
    {
        return new FlooringInput
        {
            AnalysisId = Guid.NewGuid(),
            LengthMeters = lengthMeters,
            WidthMeters = widthMeters,
            InstallationPattern = FlooringInstallationPattern.Straight,
            WastePercentage = wastePercentage
        };
    }
}
