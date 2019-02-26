using Microsoft.EntityFrameworkCore;
using pumps.Models;

namespace pumps.Database
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Pump> Pumps { get; set; }
        public DbSet<SensorLog> SensorLogs { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
    }
}