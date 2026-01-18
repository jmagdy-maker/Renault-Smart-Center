using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenaultSmartCenter.Domain.Entities;

namespace RenaultSmartCenter.Infrastructure.Data.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Make)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Renault");

        builder.Property(v => v.Model)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.Year)
            .IsRequired();

        builder.Property(v => v.VIN)
            .HasMaxLength(50);

        builder.Property(v => v.PlateNumber)
            .HasMaxLength(20);

        builder.Property(v => v.Color)
            .HasMaxLength(50);

        builder.Property(v => v.CurrentMileage)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(v => v.Customer)
            .WithMany(c => c.Vehicles)
            .HasForeignKey(v => v.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(v => v.CustomerId);
        builder.HasIndex(v => v.VIN);
        builder.HasIndex(v => v.PlateNumber);
        builder.HasIndex(v => v.Make);
        builder.HasIndex(v => v.Model);
        builder.HasIndex(v => v.IsDeleted);
    }
}
