using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Infrastructure.Data;

public static class ElegiBienDbSeeder
{
    public static async Task SeedAsync(ElegiBienDbContext dbContext)
    {
        if (!await dbContext.Categories.AnyAsync())
        {
            dbContext.Categories.AddRange(
                new Category
                {
                    Code = CategoryCode.AirConditioning,
                    Name = "Aire acondicionado",
                    Slug = "aire-acondicionado",
                    IsActive = true,
                    DisplayOrder = 1
                },
                new Category
                {
                    Code = CategoryCode.Paint,
                    Name = "Pintura",
                    Slug = "pintura",
                    IsActive = false,
                    DisplayOrder = 2
                },
                new Category
                {
                    Code = CategoryCode.CeramicAndFlooring,
                    Name = "Cerámicos y pisos",
                    Slug = "ceramicos-y-pisos",
                    IsActive = false,
                    DisplayOrder = 3
                },
                new Category
                {
                    Code = CategoryCode.WaterHeater,
                    Name = "Termotanque",
                    Slug = "termotanque",
                    IsActive = false,
                    DisplayOrder = 4
                });

            await dbContext.SaveChangesAsync();
        }

        var airConditioningCategory = await dbContext.Categories
            .SingleAsync(x => x.Code == CategoryCode.AirConditioning);

        var methodologyExists = await dbContext.MethodologyVersions
            .AnyAsync(x =>
                x.CategoryId == airConditioningCategory.CategoryId &&
                x.Version == "1.0.0");

        if (!methodologyExists)
        {
            dbContext.MethodologyVersions.Add(
                new MethodologyVersion
                {
                    CategoryId = airConditioningCategory.CategoryId,
                    Version = "1.0.0",
                    Description =
                        "Metodología inicial de dimensionamiento de aire acondicionado.",
                    EffectiveFromUtc = DateTime.UtcNow,
                    IsActive = true
                });

            await dbContext.SaveChangesAsync();
        }
    }
}