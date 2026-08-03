using ElegiBien.Domain.Entities;

namespace ElegiBien.Application.Interfaces;

public interface IAirConditioningCalculator
{
    DimensioningResult Calculate(
        AirConditioningInput input);
}