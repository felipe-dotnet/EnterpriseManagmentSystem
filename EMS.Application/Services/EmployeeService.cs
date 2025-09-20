using AutoMapper;
using EMS.Application.Commond.Interfaces;
using EMS.Application.Features.Employees.DTOs;
using EMS.Domain.Entities;

namespace EMS.Application.Services;
public class EmployeeService: IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    public EmployeeService(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<EmployeeDto>> GetAllAsync()
    {
        var employees = await _repository.GetActiveEmployeesAsync();
        return _mapper.Map<List<EmployeeDto>>(employees);
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id);
        return employee != null ? _mapper.Map<EmployeeDto>(employee) : null;
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto createDto)
    {
        var employee = _mapper.Map<Employee>(createDto);
        var createdEmployee = await _repository.AddAsync(employee);
        return _mapper.Map<EmployeeDto>(createdEmployee);
    }

    public async Task<EmployeeDto> UpdateAsync(UpdateEmployeeDto updateDto)
    {
        var employee = _mapper.Map<Employee>(updateDto);
        await _repository.UpdateAsync(employee);
        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task DeleteAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id);
        if (employee != null)
        {
            await _repository.DeleteAsync(employee);
        }
    }

    public async Task<List<EmployeeDto>> SearchAsync(string searchTerm)
    {
        var employees = await _repository.SearchAsync(searchTerm);
        return _mapper.Map<List<EmployeeDto>>(employees);
    }
}   
