using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RenaultSmartCenter.Application.Common.DTOs;
using RenaultSmartCenter.Application.Interfaces;
using RenaultSmartCenter.Domain.Entities;
using RenaultSmartCenter.Domain.Enums;
using RenaultSmartCenter.Infrastructure.Data;

namespace RenaultSmartCenter.Application.Features.ServiceOrders;

public interface IServiceOrderService
{
    Task<IEnumerable<ServiceOrderDto>> GetAllAsync(Guid branchId);
    Task<ServiceOrderDto?> GetByIdAsync(Guid id);
    Task<ServiceOrderDto> CreateAsync(CreateServiceOrderDto dto, string createdBy);
    Task<ServiceOrderDto?> UpdateAsync(Guid id, UpdateServiceOrderDto dto, string updatedBy);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> UpdateStatusAsync(Guid id, ServiceOrderStatus status, string updatedBy);
}

public class ServiceOrderService : IServiceOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ServiceOrderService(ApplicationDbContext context, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ServiceOrderDto>> GetAllAsync(Guid branchId)
    {
        var orders = await _context.ServiceOrders
            .Include(so => so.Branch)
            .Include(so => so.Customer)
            .Include(so => so.Vehicle)
            .Include(so => so.AssignedMechanic)
            .Include(so => so.ServiceOrderItems)
                .ThenInclude(soi => soi.SparePart)
            .Where(so => so.BranchId == branchId && !so.IsDeleted)
            .OrderByDescending(so => so.CreatedAt)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ServiceOrderDto>>(orders);
    }

    public async Task<ServiceOrderDto?> GetByIdAsync(Guid id)
    {
        var order = await _context.ServiceOrders
            .Include(so => so.Branch)
            .Include(so => so.Customer)
            .Include(so => so.Vehicle)
            .Include(so => so.AssignedMechanic)
            .Include(so => so.ServiceOrderItems)
                .ThenInclude(soi => soi.SparePart)
            .FirstOrDefaultAsync(so => so.Id == id && !so.IsDeleted);

        return order != null ? _mapper.Map<ServiceOrderDto>(order) : null;
    }

    public async Task<ServiceOrderDto> CreateAsync(CreateServiceOrderDto dto, string createdBy)
    {
        var order = _mapper.Map<ServiceOrder>(dto);
        order.OrderNumber = GenerateOrderNumber();
        order.CreatedBy = createdBy;
        order.Status = ServiceOrderStatus.Created;

        // Calculate costs
        foreach (var itemDto in dto.Items)
        {
            var item = _mapper.Map<ServiceOrderItem>(itemDto);
            item.ServiceOrderId = order.Id;
            order.ServiceOrderItems.Add(item);

            if (item.IsLabor)
                order.LaborCost += item.TotalPrice;
            else
                order.PartsCost += item.TotalPrice;
        }

        order.TotalAmount = order.SubTotal - order.Discount + order.VAT;

        await _unitOfWork.Repository<ServiceOrder>().AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(order.Id) ?? throw new Exception("Failed to create service order");
    }

    public async Task<ServiceOrderDto?> UpdateAsync(Guid id, UpdateServiceOrderDto dto, string updatedBy)
    {
        var order = await _context.ServiceOrders
            .Include(so => so.ServiceOrderItems)
            .FirstOrDefaultAsync(so => so.Id == id && !so.IsDeleted);

        if (order == null)
            return null;

        _mapper.Map(dto, order);
        order.UpdatedBy = updatedBy;
        order.UpdatedAt = DateTime.UtcNow;

        // Update status dates
        if (dto.Status == ServiceOrderStatus.InProgress && !order.StartedDate.HasValue)
            order.StartedDate = DateTime.UtcNow;
        else if (dto.Status == ServiceOrderStatus.Completed && !order.CompletedDate.HasValue)
            order.CompletedDate = DateTime.UtcNow;
        else if (dto.Status == ServiceOrderStatus.Delivered && !order.DeliveredDate.HasValue)
            order.DeliveredDate = DateTime.UtcNow;

        // Recalculate totals
        order.PartsCost = order.ServiceOrderItems
            .Where(i => !i.IsLabor)
            .Sum(i => i.TotalPrice);
        order.LaborCost = order.ServiceOrderItems
            .Where(i => i.IsLabor)
            .Sum(i => i.TotalPrice);
        order.TotalAmount = order.SubTotal - order.Discount + order.VAT;

        await _unitOfWork.Repository<ServiceOrder>().UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task<bool> UpdateStatusAsync(Guid id, ServiceOrderStatus status, string updatedBy)
    {
        var order = await _unitOfWork.Repository<ServiceOrder>().GetByIdAsync(id);
        if (order == null || order.IsDeleted)
            return false;

        order.Status = status;
        order.UpdatedBy = updatedBy;
        order.UpdatedAt = DateTime.UtcNow;

        if (status == ServiceOrderStatus.InProgress && !order.StartedDate.HasValue)
            order.StartedDate = DateTime.UtcNow;
        else if (status == ServiceOrderStatus.Completed && !order.CompletedDate.HasValue)
            order.CompletedDate = DateTime.UtcNow;
        else if (status == ServiceOrderStatus.Delivered && !order.DeliveredDate.HasValue)
            order.DeliveredDate = DateTime.UtcNow;

        await _unitOfWork.Repository<ServiceOrder>().UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var order = await _unitOfWork.Repository<ServiceOrder>().GetByIdAsync(id);
        if (order == null || order.IsDeleted)
            return false;

        order.IsDeleted = true;
        order.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Repository<ServiceOrder>().UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    private string GenerateOrderNumber()
    {
        return $"SO-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }
}
