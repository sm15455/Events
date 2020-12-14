using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EFCore5
{
    public partial class EF5Context
    {
        public DbSet<Lookup> Genders => Set<Lookup>("Gender");
        public DbSet<Lookup> Countries => Set<Lookup>("Country");
        public DbSet<Person> People { get; set; }

        partial void OnModelCreatingSEMD(ModelBuilder modelBuilder)
        {
            modelBuilder.SharedTypeEntity<Lookup>("Gender", c => c.HasMany<Person>().WithOne(p => p.Gender));
            modelBuilder.SharedTypeEntity<Lookup>("Country");
        }

    }

    public class Lookup
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Lookup Gender { get; set; }
    }
}