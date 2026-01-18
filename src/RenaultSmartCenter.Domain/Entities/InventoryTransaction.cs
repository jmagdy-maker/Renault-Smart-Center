using RenaultSmartCenter.Domain.Enums;
using RenaultSmartCenter.Domain.Interfaces;

namespace RenaultSmartCenter.Domain.Entities;

public class InventoryTransaction : IAuditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SparePartId { get; set; }
    public Guid BranchId { get; set; }
    public TransactionType TransactionType { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public string? ReferenceNumber { get; set; } // PO number, SO number, etc.
    public string? Notes { get; set; }
    public Guid? SupplierId { get; set; }
    public Guid? ServiceOrderId { get; set; }

    // Navigation Properties
    public virtual SparePart SparePart { get; set; } = null!;
    public virtual Branch Branch { get; set; } = null!;
    public virtual Supplier? Supplier { get; set; }
    public virtual ServiceOrder? ServiceOrder { get; set; }

    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
}
