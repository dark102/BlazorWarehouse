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
            .UsingEntity(j => j.ToTable("ProductsWarehouses"));
        }
        private async Task SeedDefaultDataAsync(DbContext context, CancellationToken cancellationToken)
        {

            if (!await context.Set<Product>().AnyAsync())
            {
                var data = new Warehouse()
                {
                    name = "Склад №1",
                    productsList = new List<Product>(){
                            new Product
                            {
                                count = 1,
                                name = "Продукт 1"
                            },
                            new Product
                            {
                                count = 1,
                                name = "Продукт 2"
                            },
                            new Product
                            {
                                count = 1,
                                name = "Продукт 3"
                            },
                            new Product
                            {
                                count = 1,
                                name = "Продукт 4"
                            },
                            new Product
                            {
                                count = 1,
                                name = "Продукт 5"
                            },
                        }

                };
                context.Set<Warehouse>().Add(data);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

    }
}
