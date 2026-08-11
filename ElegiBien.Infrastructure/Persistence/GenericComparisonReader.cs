using System.Globalization;
using System.Text.Json;
using ElegiBien.Domain.Entities;
using ElegiBien.Domain.Enums;
using ElegiBien.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElegiBien.Infrastructure.Persistence;

internal static class GenericComparisonReader
{
    public static Task<List<ComparisonAlternative>> LoadAsync(
        ElegiBienDbContext dbContext,
        Guid analysisId,
        CategoryCode categoryCode,
        CancellationToken cancellationToken)
    {
        return dbContext.ComparisonAlternatives
            .AsNoTracking()
            .Where(x =>
                x.AnalysisId == analysisId &&
                x.CategoryCode == categoryCode)
            .Include(x => x.Score!)
                .ThenInclude(x => x.Factors)
            .OrderBy(x => x.Position)
            .ToListAsync(cancellationToken);
    }

    public static TEnum GetStatus<TEnum>(
        ComparisonScore score,
        TEnum fallback)
        where TEnum : struct, Enum
    {
        if (int.TryParse(
                score.StatusCode,
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out var numericValue) &&
            Enum.IsDefined(typeof(TEnum), numericValue))
        {
            return (TEnum)Enum.ToObject(typeof(TEnum), numericValue);
        }

        return fallback;
    }

    public static TEnum GetEnum<TEnum>(
        string json,
        string propertyName,
        TEnum fallback)
        where TEnum : struct, Enum
    {
        using var document = Parse(json);
        if (!TryGetProperty(document.RootElement, propertyName, out var value))
        {
            return fallback;
        }

        if (value.ValueKind == JsonValueKind.Number &&
            value.TryGetInt32(out var numericValue) &&
            Enum.IsDefined(typeof(TEnum), numericValue))
        {
            return (TEnum)Enum.ToObject(typeof(TEnum), numericValue);
        }

        if (value.ValueKind == JsonValueKind.String)
        {
            var text = value.GetString();
            if (Enum.TryParse<TEnum>(text, true, out var parsed))
            {
                return parsed;
            }
        }

        return fallback;
    }

    public static int GetInt(
        string json,
        string propertyName,
        int fallback = 0)
    {
        using var document = Parse(json);
        return TryGetProperty(document.RootElement, propertyName, out var value) &&
               value.TryGetInt32(out var result)
            ? result
            : fallback;
    }

    public static decimal GetDecimal(
        string json,
        string propertyName,
        decimal fallback = 0m)
    {
        using var document = Parse(json);
        return TryGetProperty(document.RootElement, propertyName, out var value) &&
               value.TryGetDecimal(out var result)
            ? result
            : fallback;
    }

    private static JsonDocument Parse(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return JsonDocument.Parse("{}");
        }

        try
        {
            return JsonDocument.Parse(json);
        }
        catch (JsonException)
        {
            return JsonDocument.Parse("{}");
        }
    }

    private static bool TryGetProperty(
        JsonElement element,
        string propertyName,
        out JsonElement value)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in element.EnumerateObject())
            {
                if (string.Equals(
                        property.Name,
                        propertyName,
                        StringComparison.OrdinalIgnoreCase))
                {
                    value = property.Value;
                    return true;
                }
            }
        }

        value = default;
        return false;
    }
}
