using Microsoft.EntityFrameworkCore;

namespace EFCoreMultiThread
{
    public class AppDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=backendb;User=sa;Password=Pass123*;");
        }
    }
}