using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenaultSmartCenter.Domain.Entities;

namespace RenaultSmartCenter.Infrastructure.Data.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.ServiceDescription)
            .HasMaxLength(1000);

        builder.HasOne(a => a.Branch)
            .WithMany()
            .HasForeignKey(a => a.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Customer)
            .WithMany(c => c.Appointments)
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Vehicle)
            .WithMany(v => v.Appointments)
            .HasForeignKey(a => a.VehicleId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(a => a.AssignedMechanic)
            .WithMany(u => u.AssignedAppointments)
            .HasForeignKey(a => a.AssignedMechanicId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(a => a.BranchId);
        builder.HasIndex(a => a.CustomerId);
        builder.HasIndex(a => a.AppointmentDate);
        builder.HasIndex(a => a.Status);
        builder.HasIndex(a => new { a.AppointmentDate, a.StartTime });
        builder.HasIndex(a => a.IsDeleted);
    }
}
