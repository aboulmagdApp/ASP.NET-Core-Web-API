using cityInfo.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace cityInfo.Api.DbContexts
{
    public class CityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest { get; set; }
        public CityInfoContext(DbContextOptions<CityInfoContext> options)
           : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                 .HasData(
                new City("New York City")
                {
                    Id = new Guid("5A89BA5D-76C9-45F2-84BB-A8A2DAD0D484"),
                    Description = "The one with that big park."
                },
                new City("Antwerp")
                {
                    Id = new Guid("CA203AF8-7127-433E-AE28-9C255A04ED84"),
                    Description = "The one with the cathedral that was never really finished."
                },
                new City("Paris")
                {   Id = new Guid("FFE7E114-C1D3-41E3-B294-F49E2AD00F0B"),
                    Description = "The one with that big tower."
                });
            modelBuilder.Entity<PointOfInterest>()
             .HasData(
               new PointOfInterest("Central Park")
               {
                   Id = 1,
                   CityId = new Guid("5A89BA5D-76C9-45F2-84BB-A8A2DAD0D484"),
                   Description = "The most visited urban park in the United States."

               },
               new PointOfInterest("Empire State Building")
               {
                   Id = 2,
                   CityId = new Guid("5A89BA5D-76C9-45F2-84BB-A8A2DAD0D484"),
                   Description = "A 102-story skyscraper located in Midtown Manhattan."
               },
               new PointOfInterest("Cathedral")
                 {
                     Id = 3,
                     CityId = new Guid("CA203AF8-7127-433E-AE28-9C255A04ED84"),
                     Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans."
                 },
               new PointOfInterest("Antwerp Central Station")
               {
                   Id = 4,
                   CityId = new Guid("CA203AF8-7127-433E-AE28-9C255A04ED84"),
                   Description = "The the finest example of railway architecture in Belgium."
               },
               new PointOfInterest("Eiffel Tower")
               {
                   Id = 5,
                   CityId = new Guid("FFE7E114-C1D3-41E3-B294-F49E2AD00F0B"),
                   Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel."
               },
               new PointOfInterest("The Louvre")
               {
                   Id = 6,
                   CityId = new Guid("FFE7E114-C1D3-41E3-B294-F49E2AD00F0B"),
                   Description = "The world's largest museum."
               }
               );
            base.OnModelCreating(modelBuilder);
        }
    }
}
