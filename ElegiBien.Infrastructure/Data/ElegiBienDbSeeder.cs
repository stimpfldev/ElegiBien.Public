using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Infrastructure.Data;

public static class ElegiBienDbSeeder
{
    public static async Task SeedAsync(ElegiBienDbContext dbContext)
    {
        await EnsureCategoryAsync(
            dbContext,
            CategoryCode.AirConditioning,
            "Aire acondicionado",
            "aire-acondicionado",
            true,
            1);

        await EnsureCategoryAsync(
            dbContext,
            CategoryCode.Paint,
            "Pintura",
            "pintura",
            true,
            2);

        await EnsureCategoryAsync(
            dbContext,
            CategoryCode.CeramicAndFlooring,
            "Cerámicos y pisos",
            "ceramicos-y-pisos",
            true,
            3);

        await EnsureCategoryAsync(
            dbContext,
            CategoryCode.WaterHeater,
            "Termotanque",
            "termotanque",
            false,
            4);

        await EnsureCategoryAsync(
            dbContext,
            CategoryCode.Heating,
            "Calefacción",
            "calefaccion",
            true,
            5);

        await EnsureMethodologyAsync(
            dbContext,
            CategoryCode.AirConditioning,
            "1.0.0",
            "Metodología inicial de dimensionamiento de aire acondicionado.");

        await EnsureMethodologyAsync(
            dbContext,
            CategoryCode.Paint,
            "1.0.0",
            "Metodología inicial para cálculo de superficie, litros y comparación de pinturas.");

        await EnsureMethodologyAsync(
            dbContext,
            CategoryCode.CeramicAndFlooring,
            "1.0.0",
            "Metodología inicial para cálculo de superficie, material adicional, cajas y comparación de cerámicos y pisos.");

        await EnsureMethodologyAsync(
            dbContext,
            CategoryCode.Heating,
            "1.0.0",
            "Metodología inicial para cálculo de potencia térmica y comparación de sistemas de calefacción.");
    }

    private static async Task EnsureCategoryAsync(
        ElegiBienDbContext dbContext,
        CategoryCode code,
        string name,
        string slug,
        bool isActive,
        int displayOrder)
    {
        var category = await dbContext.Categories
            .SingleOrDefaultAsync(x => x.Code == code);

        if (category is null)
        {
            dbContext.Categories.Add(new Category
            {
                Code = code,
                Name = name,
                Slug = slug,
                IsActive = isActive,
                DisplayOrder = displayOrder
            });

            await dbContext.SaveChangesAsync();
            return;
        }

        var changed = false;

        if (category.Name != name)
        {
            category.Name = name;
            changed = true;
        }

        if (category.Slug != slug)
        {
            category.Slug = slug;
            changed = true;
        }

        if (category.IsActive != isActive)
        {
            category.IsActive = isActive;
            changed = true;
        }

        if (category.DisplayOrder != displayOrder)
        {
            category.DisplayOrder = displayOrder;
            changed = true;
        }

        if (changed)
        {
            await dbContext.SaveChangesAsync();
        }
    }

    private static async Task EnsureMethodologyAsync(
        ElegiBienDbContext dbContext,
        CategoryCode code,
        string version,
        string description)
    {
        var category = await dbContext.Categories.SingleAsync(
            x => x.Code == code);

        var methodology = await dbContext.MethodologyVersions
            .SingleOrDefaultAsync(
                x => x.CategoryId == category.CategoryId &&
                     x.Version == version);

        if (methodology is null)
        {
            dbContext.MethodologyVersions.Add(new MethodologyVersion
            {
                CategoryId = category.CategoryId,
                Version = version,
                Description = description,
                EffectiveFromUtc = DateTime.UtcNow,
                IsActive = true
            });

            await dbContext.SaveChangesAsync();
            return;
        }

        var changed = false;

        if (methodology.Description != description)
        {
            methodology.Description = description;
            changed = true;
        }

        if (!methodology.IsActive)
        {
            methodology.IsActive = true;
            changed = true;
        }

        if (changed)
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
