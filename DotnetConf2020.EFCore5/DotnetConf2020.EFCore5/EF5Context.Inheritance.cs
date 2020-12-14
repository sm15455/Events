using Microsoft.EntityFrameworkCore;

namespace EFCore5
{
    public partial class EF5Context
    {
        public DbSet<Device> Devices { get; set; }

        partial void OnModelCreatingInheritance(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Device>()
            //    .HasDiscriminator<string>("device")
            //    .HasValue<Laptop>("Laptop")
            //    .HasValue<Desktop>("Desktop");

            modelBuilder.Entity<Device>()
                .ToTable("Device");
            modelBuilder.Entity<Laptop>()
                .ToTable("Laptop");
            modelBuilder.Entity<Desktop>()
                .ToTable("Desktop");

        }

    }

    public abstract class Device
    {
        public int Id { get; set; }
        public int Name { get; set; }
    }

    public class Laptop : Device
    {
        public int BatteryLifetime { get; set; }
    }

    public class Desktop : Device
    {
        public int CaseHeight { get; set; }
        public int CaseWidth { get; set; }
        public int CaseDepth { get; set; }
    }
}