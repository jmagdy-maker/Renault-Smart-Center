using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenaultSmartCenter.Domain.Entities;

namespace RenaultSmartCenter.Infrastructure.Data.Configurations;

public class SparePartConfiguration : IEntityTypeConfiguration<SparePart>
{
    public void Configure(EntityTypeBuilder<SparePart> builder)
    {
        builder.ToTable("SpareParts");

        builder.HasKey(sp => sp.Id);

        builder.Property(sp => sp.PartNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(sp => sp.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(sp => sp.OEMNumber)
            .HasMaxLength(100);

        builder.Property(sp => sp.Category)
            .HasMaxLength(100);

        builder.Property(sp => sp.UnitPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(sp => sp.Unit)
            .HasMaxLength(20)
            .HasDefaultValue("PCS");

        builder.HasOne(sp => sp.Branch)
            .WithMany(b => b.SpareParts)
            .HasForeignKey(sp => sp.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sp => sp.Supplier)
            .WithMany(s => s.SpareParts)
            .HasForeignKey(sp => sp.SupplierId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(sp => new { sp.PartNumber, sp.BranchId })
            .IsUnique();
        builder.HasIndex(sp => sp.BranchId);
        builder.HasIndex(sp => sp.OEMNumber);
        builder.HasIndex(sp => sp.Category);
        builder.HasIndex(sp => sp.IsActive);
        builder.HasIndex(sp => sp.IsDeleted);
    }
}
