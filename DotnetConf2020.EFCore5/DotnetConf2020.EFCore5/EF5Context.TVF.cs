using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFCore5
{
    public partial class EF5Context
    {
        partial void OnModelCreatingTVF(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDbFunction(() => GetCustomerView()).HasName("GetCustomers");

            modelBuilder.Entity<CustomerView>();
        }
        public IQueryable<CustomerView> GetCustomerView() => FromExpression(() => GetCustomerView());
    }

    public class CustomerView
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}