using EMS.Application.Features.Employees.DTOs;
using EMS.Web.Models;
using Microsoft.Extensions.Options;

namespace EMS.Web.Services.ApiClients;

public class EmployeeApiClient : IEmployeeApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<EmployeeApiClient> _logger;
    private readonly string _baseEndPoint = "api/v1/Employees";

    public EmployeeApiClient(HttpClient httpClient, IOptions<ApiSettings> apiSettings, ILogger<EmployeeApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.BaseAddress = new Uri(apiSettings.Value.BaseUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(apiSettings.Value.TimeOutSeconds);
    }

    public async Task<ApiResponse<PagedResult<EmployeeDto>>?> GetEmployeesAsync(
            int page = 1,
            int pageSize = 10,
            string? search = null)
    {
        try
        {
            var queryParams = $"?page={page}&pageSize={pageSize}";
            if (!string.IsNullOrWhiteSpace(search))
            {
                queryParams += $"&search={Uri.EscapeDataString(search)}";
            }

            _logger.LogInformation("Obteniendo empleados: {Endpoint}", $"{_baseEndPoint}{queryParams}");

            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResult<EmployeeDto>>>(
                $"{_baseEndPoint}{queryParams}");

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener empleados");
            return new ApiResponse<PagedResult<EmployeeDto>>
            {
                Success = false,
                Message = "Error al conectar con el servidor",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<EmployeeDto>?> GetEmployeeByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Obteniendo empleado ID: {EmployeeId}", id);

            var response = await _httpClient.GetFromJsonAsync<ApiResponse<EmployeeDto>>(
                $"{_baseEndPoint}/{id}");

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener empleado ID: {EmployeeId}", id);
            return new ApiResponse<EmployeeDto>
            {
                Success = false,
                Message = "Error al obtener el empleado",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<EmployeeDto>?> CreateEmployeeAsync(CreateEmployeeDto createDto)
    {
        try
        {
            _logger.LogInformation("Creando empleado: {@Employee}", createDto);

            var response = await _httpClient.PostAsJsonAsync(_baseEndPoint, createDto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ApiResponse<EmployeeDto>>();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Error al crear empleado: {StatusCode} - {Content}",
                response.StatusCode, errorContent);

            // Intentar parsear el error como ApiResponse
            var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<EmployeeDto>>();
            return errorResponse ?? new ApiResponse<EmployeeDto>
            {
                Success = false,
                Message = $"Error del servidor: {response.StatusCode}",
                Errors = new List<string> { errorContent }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear empleado");
            return new ApiResponse<EmployeeDto>
            {
                Success = false,
                Message = "Error al crear el empleado",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<EmployeeDto>?> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateDto)
    {
        try
        {
            _logger.LogInformation("Actualizando empleado ID: {EmployeeId}", id);

            var response = await _httpClient.PutAsJsonAsync($"{_baseEndPoint}/{id}", updateDto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ApiResponse<EmployeeDto>>();
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<EmployeeDto>>();
            return errorResponse ?? new ApiResponse<EmployeeDto>
            {
                Success = false,
                Message = $"Error del servidor: {response.StatusCode}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar empleado ID: {EmployeeId}", id);
            return new ApiResponse<EmployeeDto>
            {
                Success = false,
                Message = "Error al actualizar el empleado",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<object>?> DeleteEmployeeAsync(int id)
    {
        try
        {
            _logger.LogInformation("Eliminando empleado ID: {EmployeeId}", id);

            var response = await _httpClient.DeleteAsync($"{_baseEndPoint}/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
            return errorResponse ?? new ApiResponse<object>
            {
                Success = false,
                Message = $"Error del servidor: {response.StatusCode}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar empleado ID: {EmployeeId}", id);
            return new ApiResponse<object>
            {
                Success = false,
                Message = "Error al eliminar el empleado",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<List<EmployeeDto>>?> SearchEmployeesAsync(string searchTerm)
    {
        try
        {
            _logger.LogInformation("Buscando empleados: {SearchTerm}", searchTerm);

            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<EmployeeDto>>>(
                $"{_baseEndPoint}/search?q={Uri.EscapeDataString(searchTerm)}");

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar empleados");
            return new ApiResponse<List<EmployeeDto>>
            {
                Success = false,
                Message = "Error al buscar empleados",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}
