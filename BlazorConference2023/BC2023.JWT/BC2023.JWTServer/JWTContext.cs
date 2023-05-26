using Microsoft.EntityFrameworkCore;

namespace BC2023.JWTServer
{
    public class JWTContext : DbContext
    {
        public JWTContext(DbContextOptions<JWTContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string? RefreshToken { get; set; } = String.Empty;
        public DateTime? RefreshTokenExpirationDate { get; set; }
    }
}
