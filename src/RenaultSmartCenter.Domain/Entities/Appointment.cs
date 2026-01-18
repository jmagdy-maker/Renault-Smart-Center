using RenaultSmartCenter.Domain.Enums;
using RenaultSmartCenter.Domain.Interfaces;

namespace RenaultSmartCenter.Domain.Entities;

public class Appointment : IAuditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BranchId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid? VehicleId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    public string? ServiceDescription { get; set; }
    public string? Notes { get; set; }
    public Guid? AssignedMechanicId { get; set; }
    public string? ReminderSent { get; set; } // JSON placeholder for reminder tracking

    // Navigation Properties
    public virtual Branch Branch { get; set; } = null!;
    public virtual Customer Customer { get; set; } = null!;
    public virtual Vehicle? Vehicle { get; set; }
    public virtual User? AssignedMechanic { get; set; }

    // Computed Properties
    public DateTime AppointmentDateTime => AppointmentDate.Date.Add(StartTime);

    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
}
