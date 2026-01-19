using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RenaultSmartCenter.Application.Common.DTOs;
using RenaultSmartCenter.Application.Interfaces;
using RenaultSmartCenter.Domain.Entities;

namespace RenaultSmartCenter.Application.Features.Inventory;

public interface ISparePartService
{
    Task<IEnumerable<SparePartDto>> GetAllAsync(Guid branchId, bool? lowStockOnly = null);
    Task<SparePartDto?> GetByIdAsync(Guid id);
    Task<SparePartDto> CreateAsync(CreateSparePartDto dto, string createdBy);
    Task<SparePartDto?> UpdateAsync(Guid id, UpdateSparePartDto dto, string updatedBy);
    Task<bool> DeleteAsync(Guid id);
}

public class SparePartService : ISparePartService
{
    private readonly IApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SparePartService(IApplicationDbContext context, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SparePartDto>> GetAllAsync(Guid branchId, bool? lowStockOnly = null)
    {
        var query = _context.SpareParts
            .Include(sp => sp.Branch)
            .Include(sp => sp.Supplier)
            .Where(sp => sp.BranchId == branchId && !sp.IsDeleted);

        if (lowStockOnly == true)
        {
            query = query.Where(sp => sp.StockQuantity <= sp.MinimumStock);
        }

        var parts = await query
            .OrderBy(sp => sp.Name)
            .ToListAsync();

        return _mapper.Map<IEnumerable<SparePartDto>>(parts);
    }

    public async Task<SparePartDto?> GetByIdAsync(Guid id)
    {
        var part = await _context.SpareParts
            .Include(sp => sp.Branch)
            .Include(sp => sp.Supplier)
            .FirstOrDefaultAsync(sp => sp.Id == id && !sp.IsDeleted);

        return part != null ? _mapper.Map<SparePartDto>(part) : null;
    }

    public async Task<SparePartDto> CreateAsync(CreateSparePartDto dto, string createdBy)
    {
        var part = _mapper.Map<SparePart>(dto);
        part.CreatedBy = createdBy;

        await _unitOfWork.Repository<SparePart>().AddAsync(part);
        await _unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(part.Id) ?? throw new Exception("Failed to create spare part");
    }

    public async Task<SparePartDto?> UpdateAsync(Guid id, UpdateSparePartDto dto, string updatedBy)
    {
        var part = await _unitOfWork.Repository<SparePart>().GetByIdAsync(id);
        if (part == null || part.IsDeleted)
            return null;

        _mapper.Map(dto, part);
        part.UpdatedBy = updatedBy;
        part.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Repository<SparePart>().UpdateAsync(part);
        await _unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var part = await _unitOfWork.Repository<SparePart>().GetByIdAsync(id);
        if (part == null || part.IsDeleted)
            return false;

        part.IsDeleted = true;
        part.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Repository<SparePart>().UpdateAsync(part);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
