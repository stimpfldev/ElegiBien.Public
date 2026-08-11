using ElegiBien.Application.Interfaces;
using ElegiBien.Application.Services;
using ElegiBien.Application.UseCases;
using ElegiBien.Infrastructure.Data;
using ElegiBien.Infrastructure.Persistence;
using ElegiBien.Web.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

namespace ElegiBien.Web.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddElegiBienServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllersWithViews();

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.AddPolicy(
                "public-forms",
                httpContext => RateLimitPartition.GetFixedWindowLimiter(
                    httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 30,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0,
                        AutoReplenishment = true
                    }));
        });

        var connectionString = configuration.GetConnectionString("ElegiBienDb");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "No se encontró la cadena de conexión ElegiBienDb. " +
                "Configurala mediante ConnectionStrings:ElegiBienDb.");
        }

        services.AddDbContext<ElegiBienDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IAirConditioningCalculator, AirConditioningCalculator>();
        services.AddScoped<IAirConditioningAnalysisStore, AirConditioningAnalysisStore>();
        services.AddScoped<IAirConditioningProductComparer, AirConditioningProductComparer>();
        services.AddScoped<IAirConditioningAnalysisReader, AirConditioningAnalysisReader>();
        services.AddScoped<IAirConditioningComparisonStore, AirConditioningComparisonStore>();
        services.AddScoped<ISharedAirConditioningResultReader, SharedAirConditioningResultReader>();

        services.AddScoped<IPaintCalculator, PaintCalculator>();
        services.AddScoped<IPaintAnalysisStore, PaintAnalysisStore>();
        services.AddScoped<IPaintAnalysisReader, PaintAnalysisReader>();
        services.AddScoped<IPaintProductComparer, PaintProductComparer>();
        services.AddScoped<IPaintComparisonStore, PaintComparisonStore>();
        services.AddScoped<ISharedPaintResultReader, SharedPaintResultReader>();

        services.AddScoped<IFlooringCalculator, FlooringCalculator>();
        services.AddScoped<IFlooringProductComparer, FlooringProductComparer>();
        services.AddScoped<IFlooringAnalysisStore, FlooringAnalysisStore>();
        services.AddScoped<IFlooringAnalysisReader, FlooringAnalysisReader>();
        services.AddScoped<IFlooringComparisonStore, FlooringComparisonStore>();
        services.AddScoped<ISharedFlooringResultReader, SharedFlooringResultReader>();

        services.AddScoped<IHeatingCalculator, HeatingCalculator>();
        services.AddScoped<IHeatingAnalysisStore, HeatingAnalysisStore>();
        services.AddScoped<IHeatingAnalysisReader, HeatingAnalysisReader>();
        services.AddScoped<IHeatingProductComparer, HeatingProductComparer>();
        services.AddScoped<IHeatingComparisonStore, HeatingComparisonStore>();
        services.AddScoped<ISharedHeatingResultReader, SharedHeatingResultReader>();

        services.AddScoped<ISharedResultService, SharedResultService>();
        services.AddScoped<IExpiredSharedResultCleanupService, ExpiredSharedResultCleanupService>();
        services.AddScoped<IAnonymousAnalyticsService, AnonymousAnalyticsService>();
        services.AddHostedService<ExpiredSharedResultsHostedService>();

        services.AddScoped<IAirConditioningUseCase, AirConditioningUseCase>();
        services.AddScoped<IPaintUseCase, PaintUseCase>();
        services.AddScoped<IFlooringUseCase, FlooringUseCase>();
        services.AddScoped<IHeatingUseCase, HeatingUseCase>();

        return services;
    }
}
