﻿// <auto-generated />
using System;
using GeeekHouseAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeeekHouseAPI.Migrations
{
    [DbContext(typeof(GeekHouseContext))]
    partial class GeekHouseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CategoryProduct", b =>
                {
                    b.Property<int>("CategoriesId")
                        .HasColumnType("int");

                    b.Property<int>("ProductsId")
                        .HasColumnType("int");

                    b.HasKey("CategoriesId", "ProductsId");

                    b.HasIndex("ProductsId");

                    b.ToTable("CategoryProduct");
                });

            modelBuilder.Entity("GeeekHouseAPI.Data.Availability", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Availability");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "DISPONIBLE"
                        },
                        new
                        {
                            Id = 2,
                            Description = "PREORDEN"
                        },
                        new
                        {
                            Id = 3,
                            Description = "AGOTADO"
                        });
                });

            modelBuilder.Entity("GeeekHouseAPI.Data.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Category");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            name = "Funko"
                        },
                        new
                        {
                            Id = 2,
                            name = "Videogame"
                        },
                        new
                        {
                            Id = 3,
                            name = "Console"
                        });
                });

            modelBuilder.Entity("GeeekHouseAPI.Data.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Mime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("productId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("productId");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("GeeekHouseAPI.Data.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AvailabilityId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.Property<DateTime>("insertDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AvailabilityId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("CategoryProduct", b =>
                {
                    b.HasOne("GeeekHouseAPI.Data.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GeeekHouseAPI.Data.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GeeekHouseAPI.Data.Image", b =>
                {
                    b.HasOne("GeeekHouseAPI.Data.Product", "product")
                        .WithMany("Image")
                        .HasForeignKey("productId");

                    b.Navigation("product");
                });

            modelBuilder.Entity("GeeekHouseAPI.Data.Product", b =>
                {
                    b.HasOne("GeeekHouseAPI.Data.Availability", "Availability")
                        .WithMany()
                        .HasForeignKey("AvailabilityId");

                    b.Navigation("Availability");
                });

            modelBuilder.Entity("GeeekHouseAPI.Data.Product", b =>
                {
                    b.Navigation("Image");
                });
#pragma warning restore 612, 618
        }
    }
}
