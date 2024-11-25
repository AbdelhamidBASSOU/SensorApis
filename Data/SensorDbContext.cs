using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SensorApis.Models;

namespace SensorApis.Data
{
    public class SensorDbContext : IdentityDbContext<User>
    {
        public SensorDbContext(DbContextOptions<SensorDbContext> options)
            : base(options)
        {
        }

        // Add this to avoid the warning
        public DbSet<Sensor> Sensors { get; set; } // Add DbSet for your sensors
    }
}
