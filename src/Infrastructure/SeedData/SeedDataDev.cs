using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.SeedData;

public static class SeedDataDev
{
    public static void Initialize()
    {
        var sensors = new List<Sensor>();
        sensors.Add(new Sensor(Guid.NewGuid(), "Sensor 1", "", "sensors/sensor1", 0, 550, "cm"));
        sensors.Add(new Sensor(Guid.NewGuid(), "Sensor 2", "", "sensors/sensor2", 0, 550, "cm"));
        sensors.Add(new Sensor(Guid.NewGuid(), "Sensor 3", "", "sensors/sensor3", 0, 550, "cm"));
        sensors.Add(new Sensor(Guid.NewGuid(), "Sensor 4", "", "sensors/sensor4", 0, 550, "cm"));

        var optionsBuilder = new DbContextOptionsBuilder<StatusMonitorDbContext>();
        optionsBuilder.UseSqlite("Data source = SensorMonitor-test.db");

        using (var context = new StatusMonitorDbContext(optionsBuilder.Options))
        {
            context.Sensors.RemoveRange(context.Sensors);
            context.SensorReadings.RemoveRange(context.SensorReadings);
            context.SaveChanges();
            
            context.Sensors.AddRange(sensors);
            context.SaveChanges();
            
            
        }
    }
}
