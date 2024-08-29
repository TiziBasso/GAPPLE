using GAPPLE.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddRadzenComponents();
//builder.Services.AddBlazoredSessionStorage();
//builder.Services.AddAuthorizationCore();
//builder.Services.AddScoped<AuthenticationService>();
//builder.Services.AddScoped<AuthenticationStateProvider>(x => x.GetService<AuthenticationService>());
//builder.Services.AddScoped<UsuariosService>();

await builder.Build().RunAsync();
