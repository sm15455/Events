using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EF8;

class EF8Context : DbContext
{
    public DbSet<Person> People => Set<Person>();
    public DbSet<Halfling> Halflings => Set<Halfling>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        optionsBuilder.UseSqlServer(
                       @$"Server=(local);Database=EF8;integrated security=true;trust server certificate=true", x =>
                       {
                           x.UseHierarchyId();
                       })
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, new[] { RelationalEventId.CommandExecuting });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Person>().OwnsOne(c => c.Contacts, b =>
        {
            b.ToJson();
            b.OwnsMany(m => m.Addresses);
        });
    }
}

class Person
{
    public int Id { get; set; }
    public DateOnly BirthDate { get; set; }
    public string Name { get; set; } = String.Empty;
    public Contact? Contacts { get; set; }
    public List<string>? Positions { get; set; }
}

public class Contact
{
    public List<Address> Addresses { get; set; } = new();
    public List<string> Emails { get; set; } = new();
    public List<string> Phones { get; set; } = new();
}

public class Address
{
    public string Street { get; set; } = String.Empty;
    public string City { get; set; } = String.Empty;
    public string State { get; set; } = String.Empty;
    public string ZipCode { get; set; } = String.Empty;
}

public class Halfling
{
    public Halfling(HierarchyId pathFromPatriarch, string name, int? yearOfBirth = null)
    {
        PathFromPatriarch = pathFromPatriarch;
        Name = name;
        YearOfBirth = yearOfBirth;
    }

    public int Id { get; private set; }
    public HierarchyId PathFromPatriarch { get; set; }
    public string Name { get; set; }
    public int? YearOfBirth { get; set; }
}