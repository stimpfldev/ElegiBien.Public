using ElegiBien.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ElegiBien.Tests.Integration;

public sealed class ElegiBienProductionWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string ConnectionStringVariable =
        "ConnectionStrings__ElegiBienDb";

    private readonly string? _previousConnectionString;

    private readonly string _databaseName =
        $"ElegiBienProductionIntegration_{Guid.NewGuid():N}";

    public ElegiBienProductionWebApplicationFactory()
    {
        _previousConnectionString =
            Environment.GetEnvironmentVariable(ConnectionStringVariable);

        Environment.SetEnvironmentVariable(
            ConnectionStringVariable,
            "Server=(localdb)\\MSSQLLocalDB;Database=ElegiBien_ProductionTest;Trusted_Connection=True;TrustServerCertificate=True");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Production");

        builder.ConfigureServices(services =>
        {
            var descriptorsToRemove = services
                .Where(descriptor =>
                    descriptor.ServiceType == typeof(ElegiBienDbContext) ||
                    descriptor.ServiceType == typeof(DbContextOptions) ||
                    descriptor.ServiceType == typeof(DbContextOptions<ElegiBienDbContext>) ||
                    descriptor.ServiceType.FullName?.Contains(
                        "IDbContextOptionsConfiguration",
                        StringComparison.Ordinal) == true)
                .ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ElegiBienDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));

            services.AddControllers()
                .AddApplicationPart(typeof(IntegrationFailureController).Assembly);
        });
    }

    protected override void Dispose(bool disposing)
    {
        try
        {
            if (disposing)
            {
                Environment.SetEnvironmentVariable(
                    ConnectionStringVariable,
                    _previousConnectionString);
            }
        }
        finally
        {
            base.Dispose(disposing);
        }
    }
}
