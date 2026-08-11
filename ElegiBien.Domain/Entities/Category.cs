using ElegiBien.Domain.Enums;

namespace ElegiBien.Domain.Entities;

public class Category
{
    public int CategoryId { get; set; }

    public CategoryCode Code { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public int DisplayOrder { get; set; }
}