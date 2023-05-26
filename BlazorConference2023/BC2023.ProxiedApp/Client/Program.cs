using BC2023.ProxiedApp.Client;
using BC2023.ProxiedApp.Client.Logic;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<AuthenticationStateProvider, CookieAuthenticationStateProvider>();
var baseAddress = builder.HostEnvironment.BaseAddress;
builder.Services.AddHttpClient<AccountService>(c => c.BaseAddress = new Uri(baseAddress));
builder.Services.AddHttpClient<ProfileService>(c => c.BaseAddress = new Uri(baseAddress + "api/users/"));
builder.Services.AddHttpClient<DataService>(c => c.BaseAddress = new Uri(baseAddress + "api/data/"));

builder.Services.AddApiAuthorization();

await builder.Build().RunAsync();
