using EMS.Application.Features.Customers.DTOs;

namespace EMS.Application.Servicesñ
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetAllAsync();
        Task<CustomerDto?> GetByIdAsync(int id);
        Task<CustomerDto> CreateAsync(CreateCustomerDto createDto);
        Task<CustomerDto> UpdateAsync(UpdateCustomerDto updateDto);
        Task DeleteAsync(int id);
        Task<List<CustomerDto>> SearchAsync(string searchTerm);
        Task<CustomerDto?> GetByEmailAsync(string email);
    }
}
