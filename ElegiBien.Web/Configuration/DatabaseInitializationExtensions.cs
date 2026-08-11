using ElegiBien.Infrastructure.Data;

namespace ElegiBien.Web.Configuration;

public static class DatabaseInitializationExtensions
{
    public static async Task<bool> RunDatabaseCommandAsync(
        this WebApplication app,
        string[] args)
    {
        if (!args.Contains("--seed", StringComparer.OrdinalIgnoreCase))
        {
            return false;
        }

        var allowSeedCommand = app.Configuration
            .GetValue("DatabaseInitialization:AllowSeedCommand", false);

        if (!allowSeedCommand)
        {
            throw new InvalidOperationException(
                "El comando de inicialización de datos está deshabilitado.");
        }

        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider
            .GetRequiredService<ElegiBienDbContext>();

        await ElegiBienDbSeeder.SeedAsync(dbContext);
        return true;
    }
}
