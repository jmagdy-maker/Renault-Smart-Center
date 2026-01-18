using RenaultSmartCenter.Domain.Enums;
using RenaultSmartCenter.Domain.Interfaces;

namespace RenaultSmartCenter.Domain.Entities;

public class ServiceOrder : IAuditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BranchId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid VehicleId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public ServiceType ServiceType { get; set; }
    public ServiceOrderStatus Status { get; set; } = ServiceOrderStatus.Created;
    public DateTime? ScheduledDate { get; set; }
    public DateTime? StartedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public decimal LaborCost { get; set; }
    public decimal PartsCost { get; set; }
    public decimal Discount { get; set; }
    public decimal VAT { get; set; }
    public decimal TotalAmount { get; set; }
    public string? InternalNotes { get; set; }
    public string? CustomerNotes { get; set; }
    public Guid? AssignedMechanicId { get; set; }

    // Navigation Properties
    public virtual Branch Branch { get; set; } = null!;
    public virtual Customer Customer { get; set; } = null!;
    public virtual Vehicle Vehicle { get; set; } = null!;
    public virtual User? AssignedMechanic { get; set; }
    public virtual ICollection<ServiceOrderItem> ServiceOrderItems { get; set; } = new List<ServiceOrderItem>();
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    // Computed Properties
    public decimal SubTotal => LaborCost + PartsCost;
    public decimal TotalAfterDiscount => SubTotal - Discount;
    public decimal TotalWithVAT => TotalAfterDiscount + VAT;

    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
}
