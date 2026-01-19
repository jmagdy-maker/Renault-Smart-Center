using Microsoft.EntityFrameworkCore;
using RenaultSmartCenter.Domain.Entities;

namespace RenaultSmartCenter.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Branch> Branches { get; }
    DbSet<User> Users { get; }
    DbSet<Customer> Customers { get; }
    DbSet<Vehicle> Vehicles { get; }
    DbSet<ServiceOrder> ServiceOrders { get; }
    DbSet<ServiceOrderItem> ServiceOrderItems { get; }
    DbSet<Appointment> Appointments { get; }
    DbSet<SparePart> SpareParts { get; }
    DbSet<InventoryTransaction> InventoryTransactions { get; }
    DbSet<Supplier> Suppliers { get; }
    DbSet<Invoice> Invoices { get; }
    DbSet<InvoiceItem> InvoiceItems { get; }
    DbSet<Payment> Payments { get; }
    DbSet<AuditLog> AuditLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
