using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Infrastructure;

public class StatusMonitorDbContext : DbContext
{
    public StatusMonitorDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data source = SensorMonitor.db");
    }
    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<SensorReading> SensorReadings { get; set; }
}
