﻿using GeeekHouseAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Data
{
    public class GeekHouseContext : IdentityDbContext<ApplicationUser>
    {
        public GeekHouseContext(DbContextOptions<GeekHouseContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new DbInitializer(modelBuilder).Seed();
        }
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Image> Image { get; set; }

        public DbSet<Availability> Availability { get; set; }

        public DbSet<Subcategory> Subcategory { get; set; }
    }
}
