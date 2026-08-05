using ElegiBien.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ElegiBien.Tests.Integration;

public sealed class ElegiBienWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName =
        $"ElegiBienIntegration_{Guid.NewGuid():N}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

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
        });
    }
}
