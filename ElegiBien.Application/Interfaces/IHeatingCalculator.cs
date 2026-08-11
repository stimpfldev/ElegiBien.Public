using ElegiBien.Domain.Entities;

namespace ElegiBien.Application.Interfaces;

public interface IHeatingCalculator
{
    HeatingCalculationResult Calculate(HeatingInput input);
}
