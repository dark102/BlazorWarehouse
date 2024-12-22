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
    [Migration("20241218141800_СhangingRelationshipsTable")]
    partial class СhangingRelationshipsTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BlazorApp1.Infrastructure.Product", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<int>("count")
                        .HasColumnType("integer");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Products");
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

            modelBuilder.Entity("ProductWarehouse", b =>
                {
                    b.Property<int>("productsListid")
                        .HasColumnType("integer");

                    b.Property<int>("warehouseListid")
                        .HasColumnType("integer");

                    b.HasKey("productsListid", "warehouseListid");

                    b.HasIndex("warehouseListid");

                    b.ToTable("ProductsWarehouses", (string)null);
                });

            modelBuilder.Entity("ProductWarehouse", b =>
                {
                    b.HasOne("BlazorApp1.Infrastructure.Product", null)
                        .WithMany()
                        .HasForeignKey("productsListid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlazorApp1.Infrastructure.Warehouse", null)
                        .WithMany()
                        .HasForeignKey("warehouseListid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
