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
                   new Category() { Id = 1,name="Funko" },
                   new Category() { Id = 2,name="Videogame"},
                   new Category() { Id=3,name="Console"}
            

            );
        }
    }
}