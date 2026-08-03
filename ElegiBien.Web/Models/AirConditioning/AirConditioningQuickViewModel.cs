using ElegiBien.Application.DTOs;

namespace ElegiBien.Web.Models.AirConditioning;

public class AirConditioningQuickViewModel
{
    public AirConditioningQuickInputDto Input { get; set; } = new();

    public AirConditioningResultDto? Result { get; set; }
}