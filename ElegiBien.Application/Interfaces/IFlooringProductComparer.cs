using ElegiBien.Application.DTOs;
using ElegiBien.Domain.Entities;

namespace ElegiBien.Application.Interfaces;

public interface IFlooringProductComparer
{
    FlooringComparisonResultDto Compare(
        FlooringCalculationResult calculation,
        FlooringProductAlternativeDto firstProduct,
        FlooringProductAlternativeDto secondProduct);
}
