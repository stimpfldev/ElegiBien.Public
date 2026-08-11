using ElegiBien.Application.DTOs;
using ElegiBien.Domain.Entities;

namespace ElegiBien.Application.Interfaces;

public interface IAirConditioningProductComparer
{
    ProductComparisonResultDto Compare(
        DimensioningResult dimensioningResult,
        ProductAlternativeDto firstProduct,
        ProductAlternativeDto secondProduct);
}