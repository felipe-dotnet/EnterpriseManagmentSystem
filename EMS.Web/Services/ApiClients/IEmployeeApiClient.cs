using EMS.Application.Features.Employees.DTOs;
using EMS.Web.Models;

namespace EMS.Web.Services.ApiClients;

public interface IEmployeeApiClient
{
    Task<ApiResponse<PagedResult<EmployeeDto>>?> GetEmployeesAsync(int page = 1, int pageSize = 10, string? search = null);
    Task<ApiResponse<EmployeeDto>?> GetEmployeeByIdAsync(int id);
    Task<ApiResponse<EmployeeDto>?> CreateEmployeeAsync(CreateEmployeeDto createDto);
    Task<ApiResponse<EmployeeDto>?> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateDto);
    Task<ApiResponse<object>?> DeleteEmployeeAsync(int id);
    Task<ApiResponse<List<EmployeeDto>>?> SearchEmployeesAsync(string searchTerm);
}
