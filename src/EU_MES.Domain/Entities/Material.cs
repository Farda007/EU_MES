using EU_MES.Domain.Common;

namespace EU_MES.Domain.Entities;

/// <summary>Represents a raw material or component.</summary>
public class Material : BaseEntity
{
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Unit { get; private set; } = string.Empty;
    public decimal StockQuantity { get; private set; }
    public decimal ReorderPoint { get; private set; }
    public string Category { get; private set; } = string.Empty;

    private Material() { }

    public static Material Create(string code, string name, string unit, decimal stockQuantity,
        decimal reorderPoint, string category)
    {
        return new Material
        {
            Code = code,
            Name = name,
            Unit = unit,
            StockQuantity = stockQuantity,
            ReorderPoint = reorderPoint,
            Category = category
        };
    }

    public void UpdateStock(decimal quantity)
    {
        StockQuantity = quantity;
        SetUpdatedAt();
    }

    public void AddStock(decimal quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
        StockQuantity += quantity;
        SetUpdatedAt();
    }

    public void ConsumeStock(decimal quantity)
    {
        if (quantity > StockQuantity) throw new InvalidOperationException("Insufficient stock.");
        StockQuantity -= quantity;
        SetUpdatedAt();
    }

    public bool IsBelowReorderPoint => StockQuantity <= ReorderPoint;
}
