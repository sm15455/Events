using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(c =>
    {
        c.Events.OnRedirectToLogin = ctx =>
        {
            ctx.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
        c.Events.OnRedirectToAccessDenied = ctx =>
        {
            ctx.Response.StatusCode = 403;
            return Task.CompletedTask;
        };
    });


var routes = new[]
{
    new RouteConfig()
    {
        RouteId = "routeApi",
        ClusterId = "clusterApi",
        Match = new RouteMatch
        {
            Path = "api/{**catch-all}"
        }
    }
};

var clusters = new[]
{
    new ClusterConfig()
    {
        ClusterId = "clusterApi",
        Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
        {
            { "destination1", new DestinationConfig() { Address = "https://localhost:5001" } }
        }
    }
};

builder.Services
    .AddReverseProxy()
    .LoadFromMemory(routes, clusters)
    .AddTransforms(builderContext =>
    {
        builderContext
            .AddPathRemovePrefix("/api")
            .AddRequestTransform(transformContext =>
            {
                var ctx = builderContext.Services.GetRequiredService<IHttpContextAccessor>();
                transformContext.ProxyRequest.Headers
                    .Where(c => c.Key.Contains("cookie", StringComparison.OrdinalIgnoreCase))
                    .ToList()
                    .ForEach(c => transformContext.ProxyRequest.Headers.Remove(c.Key));
                var token = ctx.HttpContext!.User.FindFirstValue("token");
                if (token is not null)
                {
                    var json = JsonDocument.Parse(token);
                    transformContext.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("bearer", json.RootElement.GetString("accessToken"));
                }
                return ValueTask.CompletedTask;
            });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.MapControllers();
app.MapFallbackToFile("index.html");
app.MapReverseProxy();

app.Run();
