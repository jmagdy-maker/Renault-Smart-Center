namespace RenaultSmartCenter.Application.Common.DTOs;

public class SparePartDto
{
    public Guid Id { get; set; }
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string PartNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? OEMNumber { get; set; }
    public string? Category { get; set; }
    public decimal UnitPrice { get; set; }
    public int StockQuantity { get; set; }
    public int MinimumStock { get; set; }
    public string? Unit { get; set; }
    public Guid? SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public bool IsActive { get; set; }
    public bool IsLowStock { get; set; }
    public decimal InventoryValue { get; set; }
}

public class CreateSparePartDto
{
    public Guid BranchId { get; set; }
    public string PartNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? OEMNumber { get; set; }
    public string? Category { get; set; }
    public decimal UnitPrice { get; set; }
    public int StockQuantity { get; set; }
    public int MinimumStock { get; set; }
    public string? Unit { get; set; }
    public Guid? SupplierId { get; set; }
}

public class UpdateSparePartDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? OEMNumber { get; set; }
    public string? Category { get; set; }
    public decimal UnitPrice { get; set; }
    public int MinimumStock { get; set; }
    public string? Unit { get; set; }
    public Guid? SupplierId { get; set; }
    public bool IsActive { get; set; }
}
