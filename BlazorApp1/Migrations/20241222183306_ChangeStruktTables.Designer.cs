﻿// <auto-generated />
using BlazorApp1.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BlazorApp1.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241222183306_ChangeStruktTables")]
    partial class ChangeStruktTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BlazorApp1.Infrastructure.Product", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("BlazorApp1.Infrastructure.ProductWarehouse", b =>
                {
                    b.Property<int>("WarehouseId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.HasKey("WarehouseId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductsWarehouses", (string)null);
                });

            modelBuilder.Entity("BlazorApp1.Infrastructure.Warehouse", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("BlazorApp1.Infrastructure.ProductWarehouse", b =>
                {
                    b.HasOne("BlazorApp1.Infrastructure.Product", "Product")
                        .WithMany("ProductsWarehouses")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlazorApp1.Infrastructure.Warehouse", "Warehouse")
                        .WithMany("ProductsWarehouses")
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("BlazorApp1.Infrastructure.Product", b =>
                {
                    b.Navigation("ProductsWarehouses");
                });

            modelBuilder.Entity("BlazorApp1.Infrastructure.Warehouse", b =>
                {
                    b.Navigation("ProductsWarehouses");
                });
#pragma warning restore 612, 618
        }
    }
}
