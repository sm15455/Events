using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFCore5
{
    public partial class EF5Context : DbContext
    {
        public EF5Context()
        {
        }

        public EF5Context(DbContextOptions<EF5Context> options)
                : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=(local);");
            optionsBuilder.LogTo(c => Console.WriteLine(c), (e, l) => e == RelationalEventId.CommandExecuted);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            OnModelCreatingInheritance(modelBuilder);
            OnModelCreatingManyToMany(modelBuilder);
            OnModelCreatingSEMD(modelBuilder);
            OnModelCreatingFilteredInclude(modelBuilder);
            OnModelCreatingTVF(modelBuilder);
        }

        partial void OnModelCreatingInheritance(ModelBuilder modelBuilder);
        partial void OnModelCreatingManyToMany(ModelBuilder modelBuilder);
        partial void OnModelCreatingSEMD(ModelBuilder modelBuilder);
        partial void OnModelCreatingFilteredInclude(ModelBuilder modelBuilder);
        partial void OnModelCreatingTVF(ModelBuilder modelBuilder);
    }
}