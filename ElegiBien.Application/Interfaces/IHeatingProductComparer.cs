using ElegiBien.Application.DTOs;
using ElegiBien.Domain.Entities;

namespace ElegiBien.Application.Interfaces;

public interface IHeatingProductComparer
{
    HeatingComparisonResultDto Compare(
        HeatingCalculationResult calculationResult,
        HeatingProductAlternativeDto firstProduct,
        HeatingProductAlternativeDto secondProduct);
}
