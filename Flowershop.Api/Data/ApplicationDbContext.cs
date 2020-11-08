using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Architecture_3IMD.Models.Domain;

namespace Architecture_3IMD.Data
{
    public class ApplicationDbContext : DbContext
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> ctx) : base(ctx) 
        {
            
        }

        public DbSet<Bouquet> Bouquets { get; set; }
        public DbSet<Stores> Stores { get; set; }
        public DbSet<Sale> Sales { get; set; }
    }

    
}
