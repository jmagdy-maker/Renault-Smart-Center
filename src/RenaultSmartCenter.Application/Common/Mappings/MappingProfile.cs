using AutoMapper;
using RenaultSmartCenter.Application.Common.DTOs;
using RenaultSmartCenter.Domain.Entities;
using RenaultSmartCenter.Domain.Enums;

namespace RenaultSmartCenter.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Customer mappings
        CreateMap<Customer, CustomerDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Vehicle mappings
        CreateMap<Vehicle, VehicleDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName))
            .ForMember(dest => dest.FullVehicleInfo, opt => opt.MapFrom(src => src.FullVehicleInfo));

        CreateMap<CreateVehicleDto, Vehicle>();
        CreateMap<UpdateVehicleDto, Vehicle>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // ServiceOrder mappings
        CreateMap<ServiceOrder, ServiceOrderDto>()
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName))
            .ForMember(dest => dest.VehicleInfo, opt => opt.MapFrom(src => src.Vehicle.FullVehicleInfo))
            .ForMember(dest => dest.ServiceTypeName, opt => opt.MapFrom(src => src.ServiceType.ToString()))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.AssignedMechanicName, opt => opt.MapFrom(src => src.AssignedMechanic != null ? src.AssignedMechanic.FullName : null));

        CreateMap<ServiceOrderItem, ServiceOrderItemDto>()
            .ForMember(dest => dest.SparePartName, opt => opt.MapFrom(src => src.SparePart != null ? src.SparePart.Name : null));

        CreateMap<CreateServiceOrderDto, ServiceOrder>()
            .ForMember(dest => dest.OrderNumber, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ServiceOrderStatus.Created))
            .ForMember(dest => dest.PartsCost, opt => opt.Ignore())
            .ForMember(dest => dest.LaborCost, opt => opt.Ignore())
            .ForMember(dest => dest.TotalAmount, opt => opt.Ignore());

        CreateMap<CreateServiceOrderItemDto, ServiceOrderItem>()
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Quantity * src.UnitPrice));

        CreateMap<UpdateServiceOrderDto, ServiceOrder>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // SparePart mappings
        CreateMap<SparePart, SparePartDto>()
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : null));

        CreateMap<CreateSparePartDto, SparePart>();
        CreateMap<UpdateSparePartDto, SparePart>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Appointment mappings
        CreateMap<Appointment, AppointmentDto>()
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName))
            .ForMember(dest => dest.VehicleInfo, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.FullVehicleInfo : null))
            .ForMember(dest => dest.AssignedMechanicName, opt => opt.MapFrom(src => src.AssignedMechanic != null ? src.AssignedMechanic.FullName : null))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<CreateAppointmentDto, Appointment>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AppointmentStatus.Scheduled));

        CreateMap<UpdateAppointmentDto, Appointment>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
