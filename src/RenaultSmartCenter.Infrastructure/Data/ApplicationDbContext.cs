using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RenaultSmartCenter.Domain.Entities;
using RenaultSmartCenter.Application.Interfaces;
using System.Linq.Expressions;

namespace RenaultSmartCenter.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<Branch> Branches { get; set; }
    public new DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<ServiceOrder> ServiceOrders { get; set; }
    public DbSet<ServiceOrderItem> ServiceOrderItems { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<SparePart> SpareParts { get; set; }
    public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Global query filter for soft deletes
        ApplyGlobalQueryFilter<Branch>(modelBuilder);
        ApplyGlobalQueryFilter<User>(modelBuilder);
        ApplyGlobalQueryFilter<Customer>(modelBuilder);
        ApplyGlobalQueryFilter<Vehicle>(modelBuilder);
        ApplyGlobalQueryFilter<ServiceOrder>(modelBuilder);
        ApplyGlobalQueryFilter<ServiceOrderItem>(modelBuilder);
        ApplyGlobalQueryFilter<Appointment>(modelBuilder);
        ApplyGlobalQueryFilter<SparePart>(modelBuilder);
        ApplyGlobalQueryFilter<InventoryTransaction>(modelBuilder);
        ApplyGlobalQueryFilter<Supplier>(modelBuilder);
        ApplyGlobalQueryFilter<Invoice>(modelBuilder);
        ApplyGlobalQueryFilter<InvoiceItem>(modelBuilder);
        ApplyGlobalQueryFilter<Payment>(modelBuilder);
    }

    private void ApplyGlobalQueryFilter<T>(ModelBuilder modelBuilder) where T : class
    {
        var parameter = Expression.Parameter(typeof(T), "e");
        var property = Expression.Property(parameter, "IsDeleted");
        var condition = Expression.Equal(property, Expression.Constant(false));
        var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
        modelBuilder.Entity<T>().HasQueryFilter(lambda);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Domain.Interfaces.IAuditable && 
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            var auditable = (Domain.Interfaces.IAuditable)entityEntry.Entity;

            if (entityEntry.State == EntityState.Added)
            {
                auditable.CreatedAt = DateTime.UtcNow;
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                auditable.UpdatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
