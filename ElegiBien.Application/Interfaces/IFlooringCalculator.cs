using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;

namespace ElegiBien.Application.Interfaces;

public interface IFlooringCalculator
{
    decimal GetRecommendedWastePercentage(
        FlooringInstallationPattern installationPattern);

    FlooringCalculationResult Calculate(FlooringInput input);
}
