using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Data
{
    public class GeekHouseContext : DbContext
    {
        public GeekHouseContext(DbContextOptions<GeekHouseContext> options) : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }

       
    }
}
