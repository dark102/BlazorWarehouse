using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Infrastructure
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Warehouse> Warehouses => Set<Warehouse>();
        public DbSet<ProductWarehouse> ProductsWarehouses => Set<ProductWarehouse>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseAsyncSeeding(async (context, _, cancellationToken) =>
                {
                    await SeedDefaultDataAsync(context, cancellationToken);
                })
                .UseSeeding((context, _) =>
                    SeedDefaultDataAsync(context, new CancellationToken()).Wait()
                );
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Warehouse>()
            .HasMany(u => u.productsList)
            .WithMany(c => c.warehouseList)
           .UsingEntity<ProductWarehouse>(
                   j => j
                    .HasOne(pt => pt.Product)
                    .WithMany(t => t.ProductsWarehouses)
                    .HasForeignKey(pt => pt.ProductId),
                   j => j
                    .HasOne(pt => pt.Warehouse)
                    .WithMany(p => p.ProductsWarehouses)
                    .HasForeignKey(pt => pt.WarehouseId),
                j =>
                {
                    
                    j.Property(pt => pt.ProductCount).HasDefaultValue(0);
                    j.HasKey(t => new { t.WarehouseId, t.ProductId });
                    j.ToTable("ProductsWarehouses");
                });
        }
        private async Task SeedDefaultDataAsync(DbContext context, CancellationToken cancellationToken)
        {

            if (!await context.Set<Product>().AnyAsync())
            {
                var prodList = new List<Product>(){
                            new Product
                            {
                                name = "Продукт 1"
                            },
                            new Product
                            {
                                name = "Продукт 2"
                            },
                            new Product
                            {
                                name = "Продукт 3"
                            },
                            new Product
                            {
                                name = "Продукт 4"
                            },
                            new Product
                            {
                                name = "Продукт 5"
                            },
                        };
                var data =  new List<Warehouse>()
                {
                    new Warehouse()
                    {
                        name = "Склад №1",
                        productsList =prodList
                    },
                    new Warehouse()
                    {
                        name = "Склад №2",
                        productsList =prodList
                    }

                };

                context.Set<Warehouse>().AddRange(data);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

    }
}
