using System.Runtime.CompilerServices;
using System.Text.Json;
using Domain.Models;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedData;

public static class SeedDataProd
{
    public static async void Initialize(string sensorsJson, string? deleteMessage)
    {
        var sensors = JsonSerializer.Deserialize<IEnumerable<Sensor>>(sensorsJson);

        if(sensors == null || !sensors.Any())
        {
            System.Console.WriteLine("No sensor data provided");
            return;
        }

        using (var context = new StatusMonitorDbContext())
        {
            if(deleteMessage == "clear sensors")
                context.Sensors.RemoveRange(context.Sensors);
            if(deleteMessage == "clear readings")
                context.SensorReadings.RemoveRange(context.SensorReadings);
            if(deleteMessage == "clear database")
            {
                context.Sensors.RemoveRange(context.Sensors);
                context.SensorReadings.RemoveRange(context.SensorReadings);
            }

            await context.SaveChangesAsync();


            foreach (var sensor in sensors)
            {
                if(await context.Sensors.FindAsync(sensor.Id) == null)
                    await context.AddAsync(sensor);
            }

            await context.SaveChangesAsync();
        }

    }
}