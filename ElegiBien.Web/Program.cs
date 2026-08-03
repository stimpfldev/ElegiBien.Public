using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
// Servicios de aplicación
builder.Services.AddScoped<
    ElegiBien.Application.Interfaces.IAirConditioningCalculator,
    ElegiBien.Application.Services.AirConditioningCalculator>();

builder.Services.AddScoped<
    ElegiBien.Application.Interfaces.IAirConditioningAnalysisStore,
    ElegiBien.Infrastructure.Persistence.AirConditioningAnalysisStore>();

builder.Services.AddScoped<
    ElegiBien.Application.Interfaces.IAirConditioningProductComparer,
    ElegiBien.Application.Services.AirConditioningProductComparer>();

builder.Services.AddScoped<
    ElegiBien.Application.Interfaces.IAirConditioningAnalysisReader,
    ElegiBien.Infrastructure.Persistence.AirConditioningAnalysisReader>();

builder.Services.AddScoped<
    ElegiBien.Application.Interfaces.IAirConditioningComparisonStore,
    ElegiBien.Infrastructure.Persistence.AirConditioningComparisonStore>();

builder.Services.AddScoped<
    ElegiBien.Application.Interfaces.ISharedResultService,
    ElegiBien.Infrastructure.Persistence.SharedResultService>();

builder.Services.AddScoped<
    ElegiBien.Application.Interfaces.ISharedAirConditioningResultReader,
    ElegiBien.Infrastructure.Persistence.SharedAirConditioningResultReader>();

builder.Services.AddScoped<
    ElegiBien.Application.Interfaces.IAnonymousAnalyticsService,
    ElegiBien.Infrastructure.Persistence.AnonymousAnalyticsService>();








var connectionString =
    builder.Configuration.GetConnectionString("ElegiBienDb")
    ?? throw new InvalidOperationException(
        "No se encontró la cadena de conexión ElegiBienDb.");

builder.Services.AddDbContext<ElegiBienDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext =
        scope.ServiceProvider.GetRequiredService<ElegiBienDbContext>();

    await ElegiBienDbSeeder.SeedAsync(dbContext);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=AirConditioning}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

public partial class Program;