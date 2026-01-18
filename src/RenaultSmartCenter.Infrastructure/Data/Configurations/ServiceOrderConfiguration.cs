using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenaultSmartCenter.Domain.Entities;

namespace RenaultSmartCenter.Infrastructure.Data.Configurations;

public class ServiceOrderConfiguration : IEntityTypeConfiguration<ServiceOrder>
{
    public void Configure(EntityTypeBuilder<ServiceOrder> builder)
    {
        builder.ToTable("ServiceOrders");

        builder.HasKey(so => so.Id);

        builder.Property(so => so.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(so => so.LaborCost)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0);

        builder.Property(so => so.PartsCost)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0);

        builder.Property(so => so.Discount)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0);

        builder.Property(so => so.VAT)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0);

        builder.Property(so => so.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0);

        builder.HasOne(so => so.Branch)
            .WithMany(b => b.ServiceOrders)
            .HasForeignKey(so => so.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(so => so.Customer)
            .WithMany(c => c.ServiceOrders)
            .HasForeignKey(so => so.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(so => so.Vehicle)
            .WithMany(v => v.ServiceOrders)
            .HasForeignKey(so => so.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(so => so.AssignedMechanic)
            .WithMany(u => u.AssignedServiceOrders)
            .HasForeignKey(so => so.AssignedMechanicId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(so => so.OrderNumber)
            .IsUnique();
        builder.HasIndex(so => so.BranchId);
        builder.HasIndex(so => so.CustomerId);
        builder.HasIndex(so => so.VehicleId);
        builder.HasIndex(so => so.Status);
        builder.HasIndex(so => so.ScheduledDate);
        builder.HasIndex(so => so.CreatedAt);
        builder.HasIndex(so => so.IsDeleted);
    }
}
