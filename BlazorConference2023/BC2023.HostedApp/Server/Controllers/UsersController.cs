using BC2023.HostedApp.Server.Models.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BC2023.HostedApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HostedAppContext _context;

        public UsersController(IConfiguration configuration, HostedAppContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("authenticate")]
        public async Task<bool> Authenticate(LoginRequest request)
        {
            var user = await Authenticate(request.Username, request.Password);
            if (user is null)
                return false;

            var claims = new List<Claim>
            {
                new Claim("sub", user.Id.ToString()),
                new Claim("username", user.Username),
                new Claim(ClaimTypes.Role, "Administrator"),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties { };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return true;
        }

        [HttpGet("getprofile")]
        [Authorize]
        public async Task<ActionResult<UserModel>> GetProfile()
        {
            var id = Convert.ToInt32(User.FindFirst("sub").Value);
            var user = await _context.Users.FirstAsync(c => c.Id == id);

            if (user is not null)
            {
                return new UserModel()
                {
                    FirstName = user.FirstName,
                    Id = user.Id,
                    LastName = user.LastName,
                    Username = user.Username
                };
            }
            return BadRequest();
        }

        [HttpPost("Logout")]
        [Authorize]
        public Task Logout()
        {
            return HttpContext.SignOutAsync();
        }

        private Task<User?> Authenticate(string username, string password)
        {
            return _context.Users.FirstOrDefaultAsync(c => c.Username == username && c.Password == password);
        }
    }
}