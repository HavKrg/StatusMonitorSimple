using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        var sensors = new List<Sensor>();
        sensors.Add(new Sensor("Sensor 1", "", "sensors/sensor1", 0, 550, "cm"));
        sensors.Add(new Sensor("Sensor 2", "", "sensors/sensor2", 0, 550, "cm"));
        sensors.Add(new Sensor("Sensor 3", "", "sensors/sensor3", 0, 550, "cm"));
        sensors.Add(new Sensor("Sensor 4", "", "sensors/sensor4", 0, 550, "cm"));

        var optionsBuilder = new DbContextOptionsBuilder<StatusMonitorDbContext>();
        optionsBuilder.UseSqlite("Data source = SensorMonitor-test.db");

        using (var context = new StatusMonitorDbContext(optionsBuilder.Options))
        {
            if(!context.Sensors.Any())
            {
                context.Sensors.AddRange(sensors);
                context.SaveChanges();
            }
            
        }

        // using (var context = new StatusMonitorDbContext(serviceProvider.GetRequiredService<DbContextOptions<StatusMonitorDbContext>>()))
        // {
        //     if (!context.Sensors.Any())
        //     {
        //         context.Sensors.AddRange(sensors);
        //     }
        //     context.SaveChangesAsync();
        // }
    }
}
