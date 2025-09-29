using Blazored.Modal;
using Blazored.Toast;
using EMS.Web.Components;
using EMS.Web.Models;
using EMS.Web.Services.ApiClients;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// CONFIGURAR HTTP CLIENT CON POLLY (Retry Policies)
builder.Services.AddHttpClient<IEmployeeApiClient, EmployeeApiClient>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy());

// AGREGAR BLAZORED TOAST (Notificaciones)
builder.Services.AddBlazoredToast();
//builder.Services.AddBlazoredModal();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (outcome, timespan, retryAttempt, context) =>
            {
                Console.WriteLine($"Reintento {retryAttempt} después de {timespan.TotalSeconds}s debido a: {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}");
            });
}

// CIRCUIT BREAKER - Prevenir cascada de fallos
static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30),
            onBreak: (outcome, breakDelay) =>
            {
                Console.WriteLine($"Circuit breaker abierto por {breakDelay.TotalSeconds}s");
            },
            onReset: () =>
            {
                Console.WriteLine("Circuit breaker cerrado - conexión restaurada");
            });
}
