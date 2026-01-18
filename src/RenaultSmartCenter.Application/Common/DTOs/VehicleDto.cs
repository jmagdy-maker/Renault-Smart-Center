namespace RenaultSmartCenter.Application.Common.DTOs;

public class VehicleDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string? VIN { get; set; }
    public string? PlateNumber { get; set; }
    public string? Color { get; set; }
    public decimal CurrentMileage { get; set; }
    public DateTime? LastServiceDate { get; set; }
    public string? Notes { get; set; }
    public string FullVehicleInfo { get; set; } = string.Empty;
}

public class CreateVehicleDto
{
    public Guid CustomerId { get; set; }
    public string Make { get; set; } = "Renault";
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string? VIN { get; set; }
    public string? PlateNumber { get; set; }
    public string? Color { get; set; }
    public decimal CurrentMileage { get; set; }
    public string? Notes { get; set; }
}

public class UpdateVehicleDto
{
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string? VIN { get; set; }
    public string? PlateNumber { get; set; }
    public string? Color { get; set; }
    public decimal CurrentMileage { get; set; }
    public DateTime? LastServiceDate { get; set; }
    public string? Notes { get; set; }
}
