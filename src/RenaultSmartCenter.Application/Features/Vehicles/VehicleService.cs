using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RenaultSmartCenter.Application.Common.DTOs;
using RenaultSmartCenter.Application.Interfaces;
using RenaultSmartCenter.Domain.Entities;
using RenaultSmartCenter.Infrastructure.Data;

namespace RenaultSmartCenter.Application.Features.Vehicles;

public interface IVehicleService
{
    Task<IEnumerable<VehicleDto>> GetAllAsync(Guid? customerId = null);
    Task<VehicleDto?> GetByIdAsync(Guid id);
    Task<VehicleDto> CreateAsync(CreateVehicleDto dto, string createdBy);
    Task<VehicleDto?> UpdateAsync(Guid id, UpdateVehicleDto dto, string updatedBy);
    Task<bool> DeleteAsync(Guid id);
}

public class VehicleService : IVehicleService
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VehicleService(ApplicationDbContext context, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VehicleDto>> GetAllAsync(Guid? customerId = null)
    {
        var query = _context.Vehicles
            .Include(v => v.Customer)
            .Where(v => !v.IsDeleted);

        if (customerId.HasValue)
        {
            query = query.Where(v => v.CustomerId == customerId.Value);
        }

        var vehicles = await query.ToListAsync();
        return _mapper.Map<IEnumerable<VehicleDto>>(vehicles);
    }

    public async Task<VehicleDto?> GetByIdAsync(Guid id)
    {
        var vehicle = await _context.Vehicles
            .Include(v => v.Customer)
            .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted);

        return vehicle != null ? _mapper.Map<VehicleDto>(vehicle) : null;
    }

    public async Task<VehicleDto> CreateAsync(CreateVehicleDto dto, string createdBy)
    {
        var vehicle = _mapper.Map<Vehicle>(dto);
        vehicle.CreatedBy = createdBy;

        await _unitOfWork.Repository<Vehicle>().AddAsync(vehicle);
        await _unitOfWork.SaveChangesAsync();

        var createdVehicle = await _context.Vehicles
            .Include(v => v.Customer)
            .FirstOrDefaultAsync(v => v.Id == vehicle.Id);

        return _mapper.Map<VehicleDto>(createdVehicle!);
    }

    public async Task<VehicleDto?> UpdateAsync(Guid id, UpdateVehicleDto dto, string updatedBy)
    {
        var vehicle = await _unitOfWork.Repository<Vehicle>().GetByIdAsync(id);
        if (vehicle == null || vehicle.IsDeleted)
            return null;

        _mapper.Map(dto, vehicle);
        vehicle.UpdatedBy = updatedBy;
        vehicle.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Repository<Vehicle>().UpdateAsync(vehicle);
        await _unitOfWork.SaveChangesAsync();

        var updatedVehicle = await _context.Vehicles
            .Include(v => v.Customer)
            .FirstOrDefaultAsync(v => v.Id == id);

        return _mapper.Map<VehicleDto>(updatedVehicle!);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var vehicle = await _unitOfWork.Repository<Vehicle>().GetByIdAsync(id);
        if (vehicle == null || vehicle.IsDeleted)
            return false;

        vehicle.IsDeleted = true;
        vehicle.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Repository<Vehicle>().UpdateAsync(vehicle);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
