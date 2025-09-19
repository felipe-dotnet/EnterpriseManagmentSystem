using EMS.Application.Features.Employees.DTOs;

namespace EMS.Application.Services;

public interface IEmployeeService
{
    Task<List<EmployeeDto>> GetAllAsync();
    Task<EmployeeDto?> GetByIdAsync(int id);
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto createDto);
    Task<EmployeeDto> UpdateAsync(UpdateEmployeeDto updateDto);
    Task DeleteAsync(int id);
    Task<List<EmployeeDto>> SearchAsync(string searchTerm);
}
