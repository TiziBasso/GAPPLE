using Blazored.SessionStorage;
using GAPPLE.Client;
using GAPPLE.Client.Extensiones;
using GAPPLE.Client.Helpers;
using GAPPLE.Client.Services;
using GAPPLE.Client.Tools;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress), Timeout = TimeSpan.FromDays(2) });

Services(builder.Services);

builder.Services.AddRadzenComponents();

await builder.Build().RunAsync();

static void Services(IServiceCollection services)
{
    services.AddBlazoredSessionStorage();
    services.AddScoped<AuthenticationStateProvider, AutenticacionExtension>();
    services.AddAuthorizationCore();
    services.AddSingleton<ParametrosDeConsulta>();
    services.AddScoped<OfertasService>();
    services.AddScoped<SeguridadService>();
    services.AddScoped<DialogService>();
    services.AddScoped<DialogCustom>();
    services.AddScoped<ProductosService>();
    services.AddScoped<ToolsHelpers>();
    services.AddScoped<ClientesService>();
    services.AddScoped<RadzenCustom>();
    services.AddScoped<OrdenesService>();
    services.AddScoped<JSFunction>();
    services.AddScoped<IJSFunction, JSFunction>();
    services.AddSingleton<SesionDTO>();
}