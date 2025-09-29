using Serilog;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using EMS.Application;
using EMS.Infrastructure;
using EMS.API.Middleware;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.RateLimiting;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/api-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "EMS.API")
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Starting up the API...");

    builder.Services.AddApplication();
    builder.Services.AddInfraestructure(builder.Configuration);

    builder.Services.AddValidatorsFromAssemblyContaining<Program>();


    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.WriteIndented = true;
        });

    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    });

    builder.Services.AddVersionedApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });


    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Enterprise Management System API",
            Version = "v1",
            Description = "API RESTful para gestión empresarial con arquitectura limpia",
            Contact = new OpenApiContact
            {
                Name = "Felipe .NET Developer",
                Email = "felipe.dotnet@empresa.com",
                Url = new Uri("https://github.com/felipe-dotnet")
            },
            License = new OpenApiLicense
            {
                Name = "MIT License",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        });

        // Incluir comentarios XML
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            options.IncludeXmlComments(xmlPath);
        }

        // Configuración de seguridad JWT (para futuro)
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: 'Bearer {token}'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    //CONFIGURAR CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });

        options.AddPolicy("ProductionPolicy", policy =>
        {
            policy.WithOrigins("https://tu-dominio.com", "https://app.tu-dominio.com")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
    });

    ////HEALTH CHECKS
    //builder.Services.AddHealthChecks()
    //    .AddSqlServer(
    //        builder.Configuration.GetConnectionString("DefaultConnection")!,
    //        name: "database",
    //        tags: new[] { "db", "sql", "sqlserver" });

    // RATE LIMITING(Prevención de abuso)
    builder.Services.AddRateLimiter(options =>
    {
        options.AddFixedWindowLimiter("fixed", limiterOptions =>
        {
            limiterOptions.PermitLimit = 100;
            limiterOptions.Window = TimeSpan.FromMinutes(1);
            limiterOptions.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
            limiterOptions.QueueLimit = 5;
        });
    });

    //CONFIGURACIONES ADICIONALES
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true; // Manejamos validaciones manualmente
    });

    var app = builder.Build();

    app.UseMiddleware<ValidationMiddleware>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "EMS API v1");
            options.RoutePrefix = string.Empty; // Swagger en la raíz
            options.DisplayRequestDuration();
            options.EnableDeepLinking();
            options.EnableFilter();
            options.ShowExtensions();
        });

        app.MapGet("/swagger", () => Results.Redirect("/"));

    }

    // HTTPS Redirection
    app.UseHttpsRedirection();

    // CORS
    app.UseCors(app.Environment.IsDevelopment() ? "AllowAll" : "ProductionPolicy");

    // Rate Limiting
    app.UseRateLimiter();

    // Logging de requests
    app.UseSerilogRequestLogging(options =>
    {
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
        };
    });

    // Authentication & Authorization (para futuro)
    // app.UseAuthentication();
    // app.UseAuthorization();

    // Health Checks Endpoint
    //app.MapHealthChecks("/health");

    // Controllers
    app.MapControllers();

    Log.Information("EMS API configurada correctamente");
    Log.Information("Swagger UI disponible en: {SwaggerUrl}", app.Environment.IsDevelopment() ? "https://localhost:7001" : "N/A");

    app.Run();
}
catch (Exception ex)
{

    Log.Fatal(ex, "La aplicación falló al iniciar");
    throw;
}
finally
{
    Log.CloseAndFlush();
}


