using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RenaultSmartCenter.Application.Common.DTOs;
using RenaultSmartCenter.Application.Interfaces;
using RenaultSmartCenter.Domain.Entities;
using RenaultSmartCenter.Domain.Enums;

namespace RenaultSmartCenter.Application.Features.Appointments;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDto>> GetAllAsync(Guid branchId, DateTime? date = null);
    Task<AppointmentDto?> GetByIdAsync(Guid id);
    Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto, string createdBy);
    Task<AppointmentDto?> UpdateAsync(Guid id, UpdateAppointmentDto dto, string updatedBy);
    Task<bool> DeleteAsync(Guid id);
}

public class AppointmentService : IAppointmentService
{
    private readonly IApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AppointmentService(IApplicationDbContext context, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAsync(Guid branchId, DateTime? date = null)
    {
        var query = _context.Appointments
            .Include(a => a.Branch)
            .Include(a => a.Customer)
            .Include(a => a.Vehicle)
            .Include(a => a.AssignedMechanic)
            .Where(a => a.BranchId == branchId && !a.IsDeleted);

        if (date.HasValue)
        {
            query = query.Where(a => a.AppointmentDate.Date == date.Value.Date);
        }

        var appointments = await query
            .OrderBy(a => a.AppointmentDate)
            .ThenBy(a => a.StartTime)
            .ToListAsync();

        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }

    public async Task<AppointmentDto?> GetByIdAsync(Guid id)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Branch)
            .Include(a => a.Customer)
            .Include(a => a.Vehicle)
            .Include(a => a.AssignedMechanic)
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

        return appointment != null ? _mapper.Map<AppointmentDto>(appointment) : null;
    }

    public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto, string createdBy)
    {
        var appointment = _mapper.Map<Appointment>(dto);
        appointment.CreatedBy = createdBy;

        await _unitOfWork.Repository<Appointment>().AddAsync(appointment);
        await _unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(appointment.Id) ?? throw new Exception("Failed to create appointment");
    }

    public async Task<AppointmentDto?> UpdateAsync(Guid id, UpdateAppointmentDto dto, string updatedBy)
    {
        var appointment = await _unitOfWork.Repository<Appointment>().GetByIdAsync(id);
        if (appointment == null || appointment.IsDeleted)
            return null;

        if (dto.AppointmentDate.HasValue)
            appointment.AppointmentDate = dto.AppointmentDate.Value;
        if (dto.StartTime.HasValue)
            appointment.StartTime = dto.StartTime.Value;
        if (dto.EndTime.HasValue)
            appointment.EndTime = dto.EndTime.Value;
        if (dto.Status.HasValue)
            appointment.Status = dto.Status.Value;

        appointment.ServiceDescription = dto.ServiceDescription ?? appointment.ServiceDescription;
        appointment.Notes = dto.Notes ?? appointment.Notes;
        appointment.AssignedMechanicId = dto.AssignedMechanicId ?? appointment.AssignedMechanicId;

        appointment.UpdatedBy = updatedBy;
        appointment.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Repository<Appointment>().UpdateAsync(appointment);
        await _unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var appointment = await _unitOfWork.Repository<Appointment>().GetByIdAsync(id);
        if (appointment == null || appointment.IsDeleted)
            return false;

        appointment.IsDeleted = true;
        appointment.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Repository<Appointment>().UpdateAsync(appointment);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
