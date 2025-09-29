using EMS.Application.Commond.Interfaces;
using EMS.Domain.Entities;
using EMS.Domain.Enums;
using EMS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;


namespace EMS.Infrastructure.Repositories;
public class EmployeeRepository(ApplicationDbContext context) : BaseRepository<Employee>(context), IEmployeeRepository
{
    public async Task<IReadOnlyList<Employee>> GetActiveEmployeesAsync()
    {
        return await _dbSet
            .Where(e => e.Status == EmployeeStatus.Active)
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ToListAsync();
    }

    public async Task<decimal> GetAverageSalaryByDepartmentAsync(string department)
    {
        var employees = await _dbSet
            .Where(e => e.Department.ToLower() == department.ToLower()
                    && e.Status == EmployeeStatus.Active).ToListAsync();

        return employees.Count != 0 ? employees.Average(e => e.Salary) : 0m;

    }

    public async Task<IReadOnlyList<Employee>> GetByDepartmentAsync(string department)
    {
        return await _dbSet
            .Where(e => e.Department==department)
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ToListAsync();
    }

    public async Task<Dictionary<string, int>> GetDepartmentStatsAsync()
    {
        return await _dbSet
            .Where(e => e.Status == EmployeeStatus.Active)
            .GroupBy(e => e.Department)
            .Select(g => new { Department = g.Key, Count = g.Count() })
            .ToDictionaryAsync(g => g.Department, g => g.Count);
    }

    public async Task<IReadOnlyList<Employee>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetActiveEmployeesAsync();

        var term = searchTerm.ToLower().Trim();

        return await _dbSet
            .Where(e =>
                e.FirstName==term ||
                e.LastName==term ||
                e.Email==term ||
                e.Position==term ||
                e.Department==term)
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ToListAsync();
    }

    public override async Task<Employee> AddAsync(Employee employee)
    {
        var emailExists = await _dbSet.AnyAsync(e => e.Email==employee.Email);
        if (emailExists)
            throw new InvalidOperationException($"An employee with the email '{employee.Email}' already exists.");
        employee.CreatedAt = DateTime.UtcNow;
        return await base.AddAsync(employee);
    }

    public override async Task UpdateAsync(Employee employee)
    {
        // Validar email único (excluyendo el empleado actual)
        var emailExists = await _dbSet
            .AnyAsync(e => e.Email==employee.Email && e.Id != employee.Id);

        if (emailExists)
            throw new InvalidOperationException($"Ya existe otro empleado con el email: {employee.Email}");

        await base.UpdateAsync(employee);
    }
}
