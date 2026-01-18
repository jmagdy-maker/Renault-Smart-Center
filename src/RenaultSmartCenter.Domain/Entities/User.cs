using RenaultSmartCenter.Domain.Interfaces;

namespace RenaultSmartCenter.Domain.Entities;

// This is a wrapper/extension entity for Identity User
// Actual Identity User will be in Infrastructure layer
// This provides domain-level user information
public class User : IAuditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string IdentityUserId { get; set; } = string.Empty; // Links to ASP.NET Identity User
    public Guid BranchId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? EmployeeNumber { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public virtual Branch Branch { get; set; } = null!;
    public virtual ICollection<ServiceOrder> AssignedServiceOrders { get; set; } = new List<ServiceOrder>();
    public virtual ICollection<Appointment> AssignedAppointments { get; set; } = new List<Appointment>();

    // Computed Properties
    public string FullName => $"{FirstName} {LastName}";

    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
}
