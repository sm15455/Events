using BC2023.ProxiedApp.Client.Logic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BC2023.ProxiedApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<bool> Login(LoginRequest model)
        {
            var http = new HttpClient();
            var result = await http.PostAsJsonAsync("https://localhost:5001/users/authenticate", new { model.Username, model.Password });

            var tokenData = await result.Content.ReadAsStringAsync();
            if (tokenData is null)
            {
                return false;
            }

            var claims = new List<Claim>
            {
                new Claim("token", tokenData),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties { };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return true;
        }

        [HttpPost("logout")]
        public Task Logout()
        {
            return HttpContext.SignOutAsync();
        }
    }
}
