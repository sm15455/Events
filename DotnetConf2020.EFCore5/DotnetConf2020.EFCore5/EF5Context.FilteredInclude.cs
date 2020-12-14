using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EFCore5
{
    public partial class EF5Context
    {
        public DbSet<Customer> Customers { get; set; }
        
        partial void OnModelCreatingFilteredInclude(ModelBuilder modelBuilder)
        {
        }

    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Customer Customer { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }

    public class OrderDetail
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public Order Order { get; set; }
    }
}