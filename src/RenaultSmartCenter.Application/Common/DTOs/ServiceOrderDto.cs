using RenaultSmartCenter.Domain.Enums;

namespace RenaultSmartCenter.Application.Common.DTOs;

public class ServiceOrderDto
{
    public Guid Id { get; set; }
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public Guid VehicleId { get; set; }
    public string VehicleInfo { get; set; } = string.Empty;
    public string OrderNumber { get; set; } = string.Empty;
    public ServiceType ServiceType { get; set; }
    public string ServiceTypeName { get; set; } = string.Empty;
    public ServiceOrderStatus Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public DateTime? ScheduledDate { get; set; }
    public DateTime? StartedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public decimal LaborCost { get; set; }
    public decimal PartsCost { get; set; }
    public decimal Discount { get; set; }
    public decimal VAT { get; set; }
    public decimal TotalAmount { get; set; }
    public string? InternalNotes { get; set; }
    public string? CustomerNotes { get; set; }
    public Guid? AssignedMechanicId { get; set; }
    public string? AssignedMechanicName { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ServiceOrderItemDto> Items { get; set; } = new();
}

public class ServiceOrderItemDto
{
    public Guid Id { get; set; }
    public Guid? SparePartId { get; set; }
    public string? SparePartName { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsLabor { get; set; }
}

public class CreateServiceOrderDto
{
    public Guid BranchId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid VehicleId { get; set; }
    public ServiceType ServiceType { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public string? InternalNotes { get; set; }
    public string? CustomerNotes { get; set; }
    public Guid? AssignedMechanicId { get; set; }
    public List<CreateServiceOrderItemDto> Items { get; set; } = new();
}

public class CreateServiceOrderItemDto
{
    public Guid? SparePartId { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public bool IsLabor { get; set; }
}

public class UpdateServiceOrderDto
{
    public ServiceType ServiceType { get; set; }
    public ServiceOrderStatus Status { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public decimal LaborCost { get; set; }
    public decimal Discount { get; set; }
    public decimal VAT { get; set; }
    public string? InternalNotes { get; set; }
    public string? CustomerNotes { get; set; }
    public Guid? AssignedMechanicId { get; set; }
}
