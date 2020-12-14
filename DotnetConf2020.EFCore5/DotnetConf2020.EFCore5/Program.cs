using EFCore5;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

using (var ctx = new EF5Context())
{
    ctx.Database.EnsureDeleted();
    ctx.Database.EnsureCreated();
}

//ManyToMany();

//Inheritance();

//SEMD();

//FilteredInclude();

//TVF();

static void ManyToMany()
{
    using var ctx = new EF5Context();
    var p1 = new Product() { Name = "Brooks Glycerine" };
    var p2 = new Product() { Name = "Brooks Transcend" };

    var t1 = new Tag() { Name = "Scarpe neutre" };
    var t2 = new Tag() { Name = "Scarpe antipronanti" };
    ctx.Products.Add(p1);
    ctx.Products.Add(p2);
    ctx.Tags.Add(t1);
    ctx.Tags.Add(t2);
    ctx.SaveChanges();
    ctx.ChangeTracker.Clear();

    ctx.ProductTags.Add(new ProductTag { ProductId = 1, TagId = 1, User = "Myself" });
    ctx.ProductTags.Add(new ProductTag { ProductId = 2, TagId = 2, User = "another" });
    ctx.SaveChanges();
    ctx.ChangeTracker.Clear();

    var products1 = ctx.Products.Include(c => c.Tags).ToList();
    var products2 = ctx.Products.Include(c => c.Tags).Where(c => c.ProductTags.Any(c => c.User == "myself")).ToList();
}

static void Inheritance()
{
    using var ctx = new EF5Context();
    var devices = ctx.Devices.ToList();
}

static void SEMD()
{
    using var ctx = new EF5Context();
    var g1 = new Lookup { Description = "Male" };
    var g2 = new Lookup { Description = "Female" };

    ctx.Genders.Add(g1);
    ctx.Genders.Add(g2);

    ctx.People.Add(new Person { Name = "Stefano Mostarda", Gender = g1 });
    ctx.SaveChanges();
    ctx.ChangeTracker.Clear();

    var genders = ctx.Genders.ToList();
    ctx.ChangeTracker.Clear();
}

static void FilteredInclude()
{
    using var ctx = new EF5Context();
    ctx.Customers.Include(c => c.Orders.OrderByDescending(c => c.Date).Take(10)).ThenInclude(c => c.OrderDetails.OrderBy(c => c.Price * c.Amount).Take(1)).ToList();
}

static void TVF()
{
    using var ctx = new EF5Context();
    ctx.Database.ExecuteSqlRaw($"CREATE FUNCTION GetCustomers() " +
        "RETURNS TABLE " +
        "AS return select id, name from customers");
    var c = ctx.GetCustomerView().Where(c => c.Id == 1).FirstOrDefault();
}
