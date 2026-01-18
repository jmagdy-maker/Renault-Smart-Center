using System.Linq;
using RenaultSmartCenter.Domain.Interfaces;

namespace RenaultSmartCenter.Domain.Entities;

public class Invoice : IAuditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BranchId { get; set; }
    public Guid ServiceOrderId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Discount { get; set; }
    public decimal VAT { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsPaid { get; set; } = false;
    public DateTime? PaidDate { get; set; }
    public string? Notes { get; set; }

    // Navigation Properties
    public virtual Branch Branch { get; set; } = null!;
    public virtual ServiceOrder ServiceOrder { get; set; } = null!;
    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    // Computed Properties (calculated in application layer)
    public decimal RemainingBalance => TotalAmount - (Payments?.Sum(p => p.Amount) ?? 0);

    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
}
