using RenaultSmartCenter.Domain.Interfaces;

namespace RenaultSmartCenter.Domain.Entities;

public class Vehicle : IAuditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public string Make { get; set; } = "Renault";
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string? VIN { get; set; }
    public string? PlateNumber { get; set; }
    public string? Color { get; set; }
    public decimal CurrentMileage { get; set; }
    public DateTime? LastServiceDate { get; set; }
    public string? Notes { get; set; }

    // Navigation Properties
    public virtual Customer Customer { get; set; } = null!;
    public virtual ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    // Computed Properties
    public string FullVehicleInfo => $"{Year} {Make} {Model} - {PlateNumber ?? VIN ?? "N/A"}";

    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
}
