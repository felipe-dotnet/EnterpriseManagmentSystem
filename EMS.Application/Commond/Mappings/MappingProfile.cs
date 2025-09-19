using AutoMapper;
using EMS.Application.Features.Customers.DTOs;
using EMS.Application.Features.Employees.DTOs;
using EMS.Application.Features.Orders.DTOs;
using EMS.Domain.Entities;

namespace EMS.Application.Commond.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<CreateEmployeeDto, Employee>();
        CreateMap<UpdateEmployeeDto, Employee>();

        // Customer mappings
        CreateMap<Customer, CustomerDto>()
            .ForMember(dest => dest.TotalOrders, opt => opt.MapFrom(src => src.Orders.Count));
        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>();

        // Order mappings
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.CompanyName))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()));
        CreateMap<CreateOrderDto, Order>();

    }
}
