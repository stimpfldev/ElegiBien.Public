using ElegiBien.Domain.Entities;

namespace ElegiBien.Application.Interfaces;

public interface IPaintCalculator
{
    PaintCalculationResult Calculate(PaintInput input);
}
