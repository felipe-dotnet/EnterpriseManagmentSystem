using EMS.API.Commond;
using EMS.API.Models;
using EMS.Application.Features.Employees.DTOs;
using EMS.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    /// <summary>
    /// Gestión de empleados del sistema
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class EmployeesController : BaseApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeesController> _logger;
        private readonly IValidator<CreateEmployeeDto> _createValidator;
        private readonly IValidator<UpdateEmployeeDto> _updateValidator;
        private readonly IValidator<EmployeeQuery> _queryValidator;

        public EmployeesController(
            IEmployeeService employeeService,
            ILogger<EmployeesController> logger,
            IValidator<CreateEmployeeDto> createValidator,
            IValidator<UpdateEmployeeDto> updateValidator,
            IValidator<EmployeeQuery> queryValidator)
        {
            _employeeService = employeeService;
            _logger = logger;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _queryValidator = queryValidator;
        }

        /// <summary>
        /// Obtiene un empleado específico por su ID
        /// </summary>
        /// <param name="id">ID del empleado</param>
        /// <returns>Datos del empleado</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<ActionResult<ApiResponse<EmployeeDto>>> GetEmployee(int id)
        {
            try
            {
                _logger.LogInformation("Obteniendo empleado con ID: {EmployeeId}", id);

                var employee = await _employeeService.GetByIdAsync(id);

                if (employee == null)
                {
                    _logger.LogWarning("Empleado no encontrado con ID: {EmployeeId}", id);
                    return Error<EmployeeDto>($"No se encontró el empleado con ID: {id}", null, 404);
                }

                return Success(employee, "Empleado obtenido correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empleado con ID: {EmployeeId}", id);
                return Error<EmployeeDto>("Error al obtener el empleado", null, 500);
            }
        }

        /// <summary>
        /// Obtiene una lista paginada de empleados con filtros opcionales
        /// </summary>
        /// <param name="query">Parámetros de consulta y filtros</param>
        /// <returns>Lista paginada de empleados</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<EmployeeDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<ActionResult<ApiResponse<PagedResult<EmployeeDto>>>> GetEmployees([FromQuery] EmployeeQuery query)
        {
            try
            {
                var validationResult = await _queryValidator.ValidateAsync(query);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return Error<PagedResult<EmployeeDto>>("Parámetros de consulta no válidos", errors, 400);
                }

                _logger.LogInformation("Obteniendo empleados con filtros: {@Query}", query);

                var allEmployees = await _employeeService.GetAllAsync();
                var filteredEmployees = ApplyFilters(allEmployees, query);
                var totalItems = filteredEmployees.Count;
                var pagedEmployees = filteredEmployees
                    .Skip(query.Skip)
                    .Take(query.Take)
                    .ToList();

                var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";
                var pagedResult = CreatePagedResult(pagedEmployees, totalItems, query, baseUrl);

                return Success(pagedResult, $"Se obtuvieron {pagedEmployees.Count} empleados de {totalItems} total");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empleados con filtros: {@Query}", query);
                return Error<PagedResult<EmployeeDto>>("Error al obtener la lista de empleados", null, 500);
            }
        }

        /// <summary>
        /// Crea un nuevo empleado en el sistema
        /// </summary>
        /// <param name="createDto">Datos del nuevo empleado</param>
        /// <returns>Empleado creado con su ID generado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<EmployeeDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        public async Task<ActionResult<ApiResponse<EmployeeDto>>> CreateEmployee([FromBody] CreateEmployeeDto createDto)
        {
            try
            {
                var validationResult = await _createValidator.ValidateAsync(createDto);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return Error<EmployeeDto>("Datos de entrada no válidos", errors, 400);
                }

                _logger.LogInformation("Creando nuevo empleado: {@Employee}", createDto);

                var employee = await _employeeService.CreateAsync(createDto);

                // ✅ AHORA SÍ FUNCIONA - GetEmployee está definido arriba
                return CreatedAtAction(
                    nameof(GetEmployee),
                    new { id = employee.Id, version = "1.0" },
                    ApiResponse<EmployeeDto>.SuccessResponse(employee, "Empleado creado correctamente"));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error de validación de negocio al crear empleado: {Message}", ex.Message);
                return Error<EmployeeDto>(ex.Message, null, 409);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear empleado: {@Employee}", createDto);
                return Error<EmployeeDto>("Error al crear el empleado", null, 500);
            }
        }

        /// <summary>
        /// Actualiza un empleado existente completamente
        /// </summary>
        /// <param name="id">ID del empleado</param>
        /// <param name="updateDto">Datos actualizados del empleado</param>
        /// <returns>Empleado actualizado</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        public async Task<ActionResult<ApiResponse<EmployeeDto>>> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto updateDto)
        {
            try
            {
                if (id != updateDto.Id)
                {
                    return Error<EmployeeDto>("El ID de la URL no coincide con el ID del empleado", null, 400);
                }

                var validationResult = await _updateValidator.ValidateAsync(updateDto);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return Error<EmployeeDto>("Datos de entrada no válidos", errors, 400);
                }

                _logger.LogInformation("Actualizando empleado ID: {EmployeeId}", id);

                var existingEmployee = await _employeeService.GetByIdAsync(id);
                if (existingEmployee == null)
                {
                    return Error<EmployeeDto>($"No se encontró el empleado con ID: {id}", null, 404);
                }

                var updatedEmployee = await _employeeService.UpdateAsync(updateDto);

                return Success(updatedEmployee, "Empleado actualizado correctamente");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error de validación de negocio al actualizar empleado ID: {EmployeeId}", id);
                return Error<EmployeeDto>(ex.Message, null, 409);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar empleado ID: {EmployeeId}", id);
                return Error<EmployeeDto>("Error al actualizar el empleado", null, 500);
            }
        }

        /// <summary>
        /// Elimina un empleado del sistema (soft delete)
        /// </summary>
        /// <param name="id">ID del empleado a eliminar</param>
        /// <returns>Confirmación de eliminación</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteEmployee(int id)
        {
            try
            {
                _logger.LogInformation("Eliminando empleado ID: {EmployeeId}", id);

                var employee = await _employeeService.GetByIdAsync(id);
                if (employee == null)
                {
                    return Error<object>($"No se encontró el empleado con ID: {id}", null, 404);
                }

                await _employeeService.DeleteAsync(id);

                return Success<object>(null, "Empleado eliminado correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar empleado ID: {EmployeeId}", id);
                return Error<object>("Error al eliminar el empleado", null, 500);
            }
        }

        /// <summary>
        /// Busca empleados por término de búsqueda
        /// </summary>
        /// <param name="q">Término de búsqueda (nombre, email, puesto, departamento)</param>
        /// <returns>Lista de empleados que coinciden con la búsqueda</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiResponse<List<EmployeeDto>>), 200)]
        public async Task<ActionResult<ApiResponse<List<EmployeeDto>>>> SearchEmployees([FromQuery] string q)
        {
            try
            {
                _logger.LogInformation("Buscando empleados con término: '{SearchTerm}'", q);

                var employees = await _employeeService.SearchAsync(q ?? string.Empty);

                return Success(employees, $"Se encontraron {employees.Count} empleados");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar empleados con término: '{SearchTerm}'", q);
                return Error<List<EmployeeDto>>("Error al buscar empleados", null, 500);
            }
        }

        /// <summary>
        /// Aplica filtros a la lista de empleados
        /// </summary>
        private List<EmployeeDto> ApplyFilters(List<EmployeeDto> employees, EmployeeQuery query)
        {
            var filtered = employees.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(query.Department))
            {
                filtered = filtered.Where(e => e.Department.Contains(query.Department, StringComparison.OrdinalIgnoreCase));
            }

            if (query.Status.HasValue)
            {
                filtered = filtered.Where(e => e.Status == query.Status.Value);
            }

            if (query.SalaryMin.HasValue)
            {
                filtered = filtered.Where(e => e.Salary >= query.SalaryMin.Value);
            }

            if (query.SalaryMax.HasValue)
            {
                filtered = filtered.Where(e => e.Salary <= query.SalaryMax.Value);
            }

            if (query.HiredAfter.HasValue)
            {
                filtered = filtered.Where(e => e.HireDate >= query.HiredAfter.Value);
            }

            if (query.HiredBefore.HasValue)
            {
                filtered = filtered.Where(e => e.HireDate <= query.HiredBefore.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var searchTerm = query.Search.ToLowerInvariant();
                filtered = filtered.Where(e =>
                    e.FirstName.ToLowerInvariant().Contains(searchTerm) ||
                    e.LastName.ToLowerInvariant().Contains(searchTerm) ||
                    e.Email.ToLowerInvariant().Contains(searchTerm) ||
                    e.Position.ToLowerInvariant().Contains(searchTerm) ||
                    e.Department.ToLowerInvariant().Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                filtered = query.OrderBy.ToLowerInvariant() switch
                {
                    "firstname" => query.OrderDirection.ToLowerInvariant() == "desc"
                        ? filtered.OrderByDescending(e => e.FirstName)
                        : filtered.OrderBy(e => e.FirstName),
                    "lastname" => query.OrderDirection.ToLowerInvariant() == "desc"
                        ? filtered.OrderByDescending(e => e.LastName)
                        : filtered.OrderBy(e => e.LastName),
                    "email" => query.OrderDirection.ToLowerInvariant() == "desc"
                        ? filtered.OrderByDescending(e => e.Email)
                        : filtered.OrderBy(e => e.Email),
                    "department" => query.OrderDirection.ToLowerInvariant() == "desc"
                        ? filtered.OrderByDescending(e => e.Department)
                        : filtered.OrderBy(e => e.Department),
                    "position" => query.OrderDirection.ToLowerInvariant() == "desc"
                        ? filtered.OrderByDescending(e => e.Position)
                        : filtered.OrderBy(e => e.Position),
                    "salary" => query.OrderDirection.ToLowerInvariant() == "desc"
                        ? filtered.OrderByDescending(e => e.Salary)
                        : filtered.OrderBy(e => e.Salary),
                    "hiredate" => query.OrderDirection.ToLowerInvariant() == "desc"
                        ? filtered.OrderByDescending(e => e.HireDate)
                        : filtered.OrderBy(e => e.HireDate),
                    _ => filtered.OrderBy(e => e.LastName)
                };
            }
            else
            {
                filtered = filtered.OrderBy(e => e.LastName).ThenBy(e => e.FirstName);
            }

            return filtered.ToList();
        }
    }
}