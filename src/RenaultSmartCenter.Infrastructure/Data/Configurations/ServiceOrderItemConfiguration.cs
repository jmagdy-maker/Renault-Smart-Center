using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenaultSmartCenter.Domain.Entities;

namespace RenaultSmartCenter.Infrastructure.Data.Configurations;

public class ServiceOrderItemConfiguration : IEntityTypeConfiguration<ServiceOrderItem>
{
    public void Configure(EntityTypeBuilder<ServiceOrderItem> builder)
    {
        builder.ToTable("ServiceOrderItems");

        builder.HasKey(soi => soi.Id);

        builder.Property(soi => soi.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(soi => soi.Quantity)
            .HasDefaultValue(1);

        builder.Property(soi => soi.UnitPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(soi => soi.TotalPrice)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(soi => soi.ServiceOrder)
            .WithMany(so => so.ServiceOrderItems)
            .HasForeignKey(soi => soi.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(soi => soi.SparePart)
            .WithMany(sp => sp.ServiceOrderItems)
            .HasForeignKey(soi => soi.SparePartId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(soi => soi.ServiceOrderId);
        builder.HasIndex(soi => soi.SparePartId);
        builder.HasIndex(soi => soi.IsDeleted);
    }
}
