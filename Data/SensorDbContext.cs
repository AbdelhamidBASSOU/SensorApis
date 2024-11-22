using Microsoft.EntityFrameworkCore;
using SensorApis.Models;
namespace SensorApis.Data
{
    public class SensorDbContext : DbContext
    {
        public SensorDbContext(DbContextOptions<SensorDbContext> options) : base(options) { }

        public DbSet<Sensor> Sensors { get; set; }
    }
}
