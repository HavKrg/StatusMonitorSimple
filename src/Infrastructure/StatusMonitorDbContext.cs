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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alarm>().Property(a => a.Status).HasDefaultValue(null);

        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<Alarm> Alarms { get; set; }
    public DbSet<SensorReading> SensorReadings { get; set; }
    public DbSet<ModbusStatus> ModbusStatuses { get; set; }
}
