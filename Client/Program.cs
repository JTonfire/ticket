using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using ITTicketingProject.Client;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddRadzenComponents();
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "ITTicketingProjectTheme";
    options.Duration = TimeSpan.FromDays(365);
});
builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ITTicketingProject.Client.TicketingDBService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddHttpClient("ITTicketingProject.Server", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ITTicketingProject.Server"));
builder.Services.AddScoped<ITTicketingProject.Client.SecurityService>();
builder.Services.AddScoped<AuthenticationStateProvider, ITTicketingProject.Client.ApplicationAuthenticationStateProvider>();
var host = builder.Build();
await host.RunAsync();