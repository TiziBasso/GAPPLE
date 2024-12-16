using GAPPLE.Client;
using GAPPLE.Client.Helpers;
using GAPPLE.Client.Services;
using GAPPLE.Client.Tools;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress), Timeout = TimeSpan.FromDays(2) });

Services(builder.Services);

builder.Services.AddRadzenComponents();
//builder.Services.AddBlazoredSessionStorage();
//builder.Services.AddAuthorizationCore();
//builder.Services.AddScoped<AuthenticationService>();
//builder.Services.AddScoped<AuthenticationStateProvider>(x => x.GetService<AuthenticationService>());
//builder.Services.AddScoped<UsuariosService>();

await builder.Build().RunAsync();

static void Services(IServiceCollection services)
{
    services.AddSingleton<ParametrosDeConsulta>();
    services.AddScoped<OfertasService>();
    services.AddScoped<SeguridadService>();
    services.AddScoped<DialogCustom>();
    services.AddScoped<DialogService>();
    services.AddScoped<ProductosService>();
    services.AddScoped<ToolsHelpers>();
    services.AddScoped<ClientesService>();
}