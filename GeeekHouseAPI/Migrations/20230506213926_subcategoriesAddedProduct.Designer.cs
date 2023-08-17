﻿// <auto-generated />
using System;
using GeeekHouseAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeeekHouseAPI.Migrations
{
    [DbContext(typeof(GeekHouseContext))]
    [Migration("20230506213926_subcategoriesAddedProduct")]
    partial class subcategoriesAddedProduct
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
                            name = "Videojuegos"
                        },
                        new
                        {
                            Id = 3,
                            name = "Consolas"
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

                    b.Property<int?>("CategoryId")
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

                    b.HasIndex("CategoryId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("GeeekHouseAPI.Data.Subcategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<int?>("categoryId")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("categoryId");

                    b.ToTable("Subcategory");
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

                    b.HasOne("GeeekHouseAPI.Data.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Availability");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("GeeekHouseAPI.Data.Subcategory", b =>
                {
                    b.HasOne("GeeekHouseAPI.Data.Product", null)
                        .WithMany("Subcategories")
                        .HasForeignKey("ProductId");

                    b.HasOne("GeeekHouseAPI.Data.Category", "category")
                        .WithMany("subcategories")
                        .HasForeignKey("categoryId");

                    b.Navigation("category");
                });

            modelBuilder.Entity("GeeekHouseAPI.Data.Category", b =>
                {
                    b.Navigation("Products");

                    b.Navigation("subcategories");
                });

            modelBuilder.Entity("GeeekHouseAPI.Data.Product", b =>
                {
                    b.Navigation("Image");

                    b.Navigation("Subcategories");
                });
#pragma warning restore 612, 618
        }
    }
}