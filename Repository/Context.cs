using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class Context : DbContext
    {
        public DbSet<Population> Population { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=localhost,1433;Database=UnDataStore;user id=sa;pwd=<YourStrong!Passw0rd>;MultipleActiveResultSets=true;");
        }
    }
}