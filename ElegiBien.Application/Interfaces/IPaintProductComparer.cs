using ElegiBien.Application.DTOs;
using ElegiBien.Domain.Entities;

namespace ElegiBien.Application.Interfaces;

public interface IPaintProductComparer
{
    PaintComparisonResultDto Compare(PaintInput input, PaintCalculationResult calculation, PaintProductAlternativeDto firstProduct, PaintProductAlternativeDto secondProduct);
}
