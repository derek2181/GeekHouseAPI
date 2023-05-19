using Microsoft.EntityFrameworkCore;

namespace GeeekHouseAPI.Data
{
    public class DbInitializer
    {
        private readonly ModelBuilder modelBuilder;

        public DbInitializer(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        public void Seed()
        {
            modelBuilder.Entity<Category>().HasData(
                   new Category() { Id = 1, Name = "Funko"},
                   new Category() { Id = 2, Name = "Videojuegos" },
                   new Category() { Id = 3, Name = "Consolas" }


            );

            modelBuilder.Entity<Availability>().HasData(
                    new Availability() { Id = 1, Description = "DISPONIBLE" },
                    new Availability() { Id = 2, Description = "PREVENTA" },
                    new Availability() { Id = 3, Description = "AGOTADO" }
            );
        }
    }
}