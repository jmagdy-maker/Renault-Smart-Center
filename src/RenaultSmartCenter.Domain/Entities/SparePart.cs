using RenaultSmartCenter.Domain.Interfaces;

namespace RenaultSmartCenter.Domain.Entities;

public class SparePart : IAuditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BranchId { get; set; }
    public string PartNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? OEMNumber { get; set; } // Renault OEM part number
    public string? Category { get; set; }
    public decimal UnitPrice { get; set; }
    public int StockQuantity { get; set; } = 0;
    public int MinimumStock { get; set; } = 0;
    public string? Unit { get; set; } = "PCS";
    public Guid? SupplierId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public virtual Branch Branch { get; set; } = null!;
    public virtual Supplier? Supplier { get; set; }
    public virtual ICollection<ServiceOrderItem> ServiceOrderItems { get; set; } = new List<ServiceOrderItem>();
    public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();

    // Computed Properties
    public bool IsLowStock => StockQuantity <= MinimumStock;
    public decimal InventoryValue => StockQuantity * UnitPrice;

    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
}
