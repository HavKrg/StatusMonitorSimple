using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.Json;
using Domain.Models;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedData;

public static class SeedDataProd
{
    public static async void Initialize(string sensorsJson, string? requestMessage)
    {
        var sensors = JsonSerializer.Deserialize<IEnumerable<Sensor>>(sensorsJson);
        System.Console.WriteLine("seeding data");
        if (sensors == null || !sensors.Any())
        {
            System.Console.WriteLine("No sensor data provided");
            return;
        }
        Console.WriteLine(requestMessage);
        using (var context = new StatusMonitorDbContext())
        {
            if (requestMessage == "fresh database")
            {
                System.Console.WriteLine("deleting database");
                await context.Database.EnsureDeletedAsync();
                System.Console.WriteLine("creating database");
                await context.Database.EnsureCreatedAsync();
                System.Console.WriteLine("ensure created complete");
            }
            else if (requestMessage == "clear readings")
            {
                await context.Database.EnsureCreatedAsync();
                
                System.Console.WriteLine("deleting readings");
                context.SensorReadings.RemoveRange(context.SensorReadings);
            }
            else
            {
                await context.Database.EnsureCreatedAsync();
            }
            

            await context.SaveChangesAsync();


            foreach (var sensor in sensors)
            {
                var dBSensor = await context.Sensors.FindAsync(sensor.Id);
                if (dBSensor == null)
                    await context.Sensors.AddAsync(sensor);
                else
                {
                    dBSensor.Update(sensor);
                }
            }

            await context.SaveChangesAsync();
        }

    }
}