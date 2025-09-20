using AutoMapper;
using EMS.Application.Commond.Interfaces;
using EMS.Application.Features.Customers.DTOs;
using EMS.Application.Servicesñ;
using EMS.Domain.Entities;

namespace EMS.Application.Services;
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<CustomerDto>> GetAllAsync()
    {
        var customers = await _repository.GetActiveCustomersAsync();
        return _mapper.Map<List<CustomerDto>>(customers);
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var customer = await _repository.GetByIdAsync(id);
        return customer != null ? _mapper.Map<CustomerDto>(customer) : null;
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto createDto)
    {
        var customer = _mapper.Map<Customer>(createDto);
        var createdCustomer = await _repository.AddAsync(customer);
        return _mapper.Map<CustomerDto>(createdCustomer);
    }

    public async Task<CustomerDto> UpdateAsync(UpdateCustomerDto updateDto)
    {
        var customer = _mapper.Map<Customer>(updateDto);
        await _repository.UpdateAsync(customer);
        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await _repository.GetByIdAsync(id);
        if (customer != null)
        {
            customer.IsDeleted = true; // Soft delete
            await _repository.UpdateAsync(customer);
        }
    }

    public async Task<List<CustomerDto>> SearchAsync(string searchTerm)
    {
        var customers = await _repository.SearchAsync(searchTerm);
        return _mapper.Map<List<CustomerDto>>(customers);
    }

    public async Task<CustomerDto?> GetByEmailAsync(string email)
    {
        var customer = await _repository.GetByEmailAsync(email);
        return customer != null ? _mapper.Map<CustomerDto>(customer) : null;
    }
}
