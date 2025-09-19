using EMS.Domain.Entities;

namespace EMS.Application.Commond.Interfaces;
public interface IEmployeeRepository:IRepository<Employee>
{
    Task<IReadOnlyList<Employee>> GetActiveEmployeesAsync();
    Task<IReadOnlyList<Employee>> GetByDepartmentAsync(string department);
    Task<IReadOnlyList<Employee>> SearchAsync(string searchTerm);
    Task<Dictionary<string, int>> GetDepartmentStatsAsync();
    Task<decimal> GetAverageSalaryByDepartmentAsync(string department);
}
