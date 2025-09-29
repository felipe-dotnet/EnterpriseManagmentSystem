using FluentValidation;
using EMS.API.Commond;
using System.Net;
using System.Text.Json;

namespace EMS.API.Middleware;

public class ValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ValidationMiddleware> _logger;
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ValidationMiddleware(RequestDelegate next, ILogger<ValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException validationException)
        {
            _logger.LogWarning("Validation failed: {@ValidationErrors}", validationException.Errors);
            await HandleValidationExceptionAsync(context, validationException);
        }
    }

    private static async Task HandleValidationExceptionAsync(HttpContext context, ValidationException validationException)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var errors = validationException.Errors
            .Select(error => error.ErrorMessage)
            .ToList();

        var response = ApiResponse<object>.ErrorResponse("Datos de entrada no válidos", errors);
        response.Meta.RequestId = context.TraceIdentifier;

        var jsonResponse = JsonSerializer.Serialize(response, options: _jsonSerializerOptions);

        await context.Response.WriteAsync(jsonResponse);
    }
}
