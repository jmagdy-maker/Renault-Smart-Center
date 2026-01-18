using RenaultSmartCenter.Domain.Enums;

namespace RenaultSmartCenter.Application.Common.DTOs;

public class AppointmentDto
{
    public Guid Id { get; set; }
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public Guid? VehicleId { get; set; }
    public string? VehicleInfo { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    public AppointmentStatus Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public string? ServiceDescription { get; set; }
    public string? Notes { get; set; }
    public Guid? AssignedMechanicId { get; set; }
    public string? AssignedMechanicName { get; set; }
}

public class CreateAppointmentDto
{
    public Guid BranchId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid? VehicleId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public string? ServiceDescription { get; set; }
    public string? Notes { get; set; }
    public Guid? AssignedMechanicId { get; set; }
}

public class UpdateAppointmentDto
{
    public DateTime? AppointmentDate { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public AppointmentStatus? Status { get; set; }
    public string? ServiceDescription { get; set; }
    public string? Notes { get; set; }
    public Guid? AssignedMechanicId { get; set; }
}
