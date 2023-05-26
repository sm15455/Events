using BC2023.HostedApp.Client;
using BC2023.HostedApp.Client.Logic;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();
var baseAddress = builder.HostEnvironment.BaseAddress + "api/";
builder.Services.AddHttpClient<AccountService>(c => c.BaseAddress = new Uri(baseAddress + "users/"));
builder.Services.AddHttpClient<DataService>(c => c.BaseAddress = new Uri(baseAddress + "data/"));

builder.Services.AddApiAuthorization();

await builder.Build().RunAsync();
