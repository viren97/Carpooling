using Carpooling.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Carpooling.DataProvider {
    public class CarpoolingContext : DbContext {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rider> Riders { get; set; }
        public DbSet<Via> Vias { get; set; }
       


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            //var config = ConfigurationManager.ConnectionStrings["CarpoolingDatabase"].ConnectionString;
            optionsBuilder.UseSqlServer(@"Server=VIRENDRA-PC\MYSQL;Database=Carpool;Trusted_Connection=True;");
        }

    }
}
