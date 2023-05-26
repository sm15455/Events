using BC2023.ProxiedApp.Api.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BC2023.ProxiedApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApiContext _context;

        public UsersController(IConfiguration configuration, ApiContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("authenticate")]
        public async Task<TokenResponse?> Authenticate(LoginRequest request)
        {
            var user = await Authenticate(request.Username, request.Password);
            if (user is null)
                return null;

            var response = new TokenResponse()
            {
                AccessToken = GenerateJwtToken(user),
            };
            return response!;
        }

        [HttpGet("getprofile")]
        [Authorize]
        public async Task<ActionResult<UserModel>> GetProfile()
        {
            var id = Convert.ToInt32(User.FindFirst("sub")!.Value);
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

        private Task<User?> Authenticate(string username, string password)
        {
            return _context.Users.FirstOrDefaultAsync(c => c.Username == username && c.Password == password);
        }

        private string GenerateJwtToken(User user)
        {
            //getting the secret key
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretKey")!);

            //create claims
            var subClaim = new Claim("sub", user.Id.ToString());
            var usernameClaim = new Claim("username", user.Username);

            //create claimsIdentity
            var claimsIdentity = new ClaimsIdentity(new[] { subClaim, usernameClaim }, "serverAuth");

            // generate token that is valid for 7 days
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            //creating a token handler
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //returning the token back
            return tokenHandler.WriteToken(token);
        }
    }
}