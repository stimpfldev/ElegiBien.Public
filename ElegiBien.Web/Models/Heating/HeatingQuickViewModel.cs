using ElegiBien.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace ElegiBien.Web.Models.Heating;

public class HeatingQuickViewModel
{
    public HeatingQuickInputDto Input { get; set; } = new();

    public HeatingCalculationResultDto? Result { get; set; }

    [Display(Name = "Permitir analítica anónima para mejorar ElegíBien")]
    public bool AllowAnonymousAnalytics { get; set; }

    [Display(Name = "Permitir el uso anónimo de este resultado en estadísticas agregadas")]
    public bool AllowRadarData { get; set; }
}
