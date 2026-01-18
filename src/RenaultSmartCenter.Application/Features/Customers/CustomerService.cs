using AutoMapper;
using RenaultSmartCenter.Application.Common.DTOs;
using RenaultSmartCenter.Application.Interfaces;
using RenaultSmartCenter.Domain.Entities;

namespace RenaultSmartCenter.Application.Features.Customers;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllAsync(Guid branchId);
    Task<CustomerDto?> GetByIdAsync(Guid id);
    Task<CustomerDto> CreateAsync(CreateCustomerDto dto, string createdBy);
    Task<CustomerDto?> UpdateAsync(Guid id, UpdateCustomerDto dto, string updatedBy);
    Task<bool> DeleteAsync(Guid id);
}

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync(Guid branchId)
    {
        var customers = await _unitOfWork.Repository<Customer>().FindAsync(c => c.BranchId == branchId && !c.IsDeleted);
        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }

    public async Task<CustomerDto?> GetByIdAsync(Guid id)
    {
        var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(id);
        return customer != null ? _mapper.Map<CustomerDto>(customer) : null;
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto, string createdBy)
    {
        var customer = _mapper.Map<Customer>(dto);
        customer.CreatedBy = createdBy;
        
        await _unitOfWork.Repository<Customer>().AddAsync(customer);
        await _unitOfWork.SaveChangesAsync();
        
        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<CustomerDto?> UpdateAsync(Guid id, UpdateCustomerDto dto, string updatedBy)
    {
        var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(id);
        if (customer == null || customer.IsDeleted)
            return null;

        _mapper.Map(dto, customer);
        customer.UpdatedBy = updatedBy;
        customer.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Repository<Customer>().UpdateAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(id);
        if (customer == null || customer.IsDeleted)
            return false;

        customer.IsDeleted = true;
        customer.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Repository<Customer>().UpdateAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
