using BC2023.JWTApp;
using BC2023.JWTApp.Logic;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddApiAuthorization();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<AuthenticationStateProvider, JWTAuthenticationStateProvider>();
builder.Services
    .AddHttpClient<AccountService>(c => c.BaseAddress = new Uri("https://localhost:5011/users/"));

builder.Services
    .AddHttpClient<ProfileService>(c => c.BaseAddress = new Uri("https://localhost:5011/users/"))
    .AddHttpMessageHandler<JWTAuthorizationHandler>();

builder.Services
    .AddHttpClient<DataService>(c => c.BaseAddress = new Uri("https://localhost:5011/data/"))
    .AddHttpMessageHandler<JWTAuthorizationHandler>();

builder.Services.AddScoped<JWTAuthorizationHandler>();
builder.Services.AddScoped<StorageService>();

await builder.Build().RunAsync();
