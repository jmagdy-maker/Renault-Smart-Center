using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenaultSmartCenter.Domain.Entities;

namespace RenaultSmartCenter.Infrastructure.Data.Configurations;

public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
{
    public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
    {
        builder.ToTable("InventoryTransactions");

        builder.HasKey(it => it.Id);

        builder.Property(it => it.UnitPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(it => it.TotalAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(it => it.ReferenceNumber)
            .HasMaxLength(100);

        builder.HasOne(it => it.SparePart)
            .WithMany(sp => sp.InventoryTransactions)
            .HasForeignKey(it => it.SparePartId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(it => it.Branch)
            .WithMany()
            .HasForeignKey(it => it.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(it => it.Supplier)
            .WithMany(s => s.InventoryTransactions)
            .HasForeignKey(it => it.SupplierId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(it => it.ServiceOrder)
            .WithMany()
            .HasForeignKey(it => it.ServiceOrderId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(it => it.SparePartId);
        builder.HasIndex(it => it.BranchId);
        builder.HasIndex(it => it.TransactionType);
        builder.HasIndex(it => it.CreatedAt);
        builder.HasIndex(it => it.IsDeleted);
    }
}
