using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EFCore5
{
    public partial class EF5Context
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }

        partial void OnModelCreatingManyToMany(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>();

            modelBuilder.Entity<Tag>(entity =>
                entity
                    .HasMany(e => e.Products)
                    .WithMany(e => e.Tags)
                    .UsingEntity<ProductTag>(
                                j => j
                                    .HasOne(pt => pt.Product)
                                    .WithMany(pt => pt.ProductTags)
                                    .HasForeignKey(pt => pt.ProductId),
                                j => j
                                    .HasOne(pt => pt.Tag)
                                    .WithMany(pt => pt.ProductTags)
                                    .HasForeignKey(pt => pt.TagId),
                                j =>
                                {
                                    j.Property(pt => pt.AssociationDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
                                    j.Property(pt => pt.User).IsRequired();
                                }));
        }

    }

    public partial class Product
    {
        public Product()
        {
            Tags = new HashSet<Tag>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<ProductTag> ProductTags { get; set; }
    }

    public partial class Tag
    {
        public Tag()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<ProductTag> ProductTags { get; set; }
    }

    public partial class ProductTag
    {
        public int TagId { get; set; }
        public int ProductId { get; set; }
        public Tag Tag { get; set; }
        public Product Product { get; set; }

        public DateTime AssociationDate { get; set; }
        public string User { get; set; }
    }

}