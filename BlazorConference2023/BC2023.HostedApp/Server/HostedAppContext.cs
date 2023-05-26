using Microsoft.EntityFrameworkCore;

namespace BC2023.HostedApp.Server
{
    public class HostedAppContext : DbContext
    {
        public HostedAppContext(DbContextOptions<HostedAppContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
