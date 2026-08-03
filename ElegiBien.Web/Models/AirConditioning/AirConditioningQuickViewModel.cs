using ElegiBien.Application.DTOs;

namespace ElegiBien.Web.Models.AirConditioning;

public class AirConditioningQuickViewModel
{
    public AirConditioningQuickInputDto Input { get; set; } = new();

    public bool AllowAnonymousAnalytics { get; set; }

    public bool AllowRadarData { get; set; }

    public AirConditioningResultDto? Result { get; set; }
}