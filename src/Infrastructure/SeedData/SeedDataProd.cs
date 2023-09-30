using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.Json;
using Domain.Models;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.SeedData;

public static class SeedDataProd
{
    public static async void Initialize(string locationsJson, string? requestMessage)
    {
        var locations = JsonSerializer.Deserialize<List<Location>>(locationsJson);
        if (locations == null || locations.Count == 0)
        {
            System.Console.WriteLine("No location data provided");
            return;
        }

        if(string.IsNullOrWhiteSpace(requestMessage))
            requestMessage = "no request message";
        Console.WriteLine(requestMessage);
        var optionsBuilder = new DbContextOptionsBuilder<StatusMonitorDbContext>();

        using (var context = new StatusMonitorDbContext(optionsBuilder.Options))
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
                System.Console.WriteLine("creating database");
                await context.Database.EnsureCreatedAsync();
            }
            

            await context.SaveChangesAsync();


            foreach (var location in locations)
            {
                var dBLocation = await context.Locations.FindAsync(location.Id);
                if (dBLocation == null)
                {
                    System.Console.WriteLine($"adding location: {location.Name}");
                    await context.Locations.AddAsync(location);
                    var modbusStatus = new ModbusStatus(location.Id, false, 0, 0, 0, string.Empty, DateTime.MinValue);
                    await context.ModbusStatuses.AddAsync(modbusStatus);
                }
                else
                {
                    System.Console.WriteLine($"updating location: {location.Name}");
                    dBLocation.Update(location);
                }

                foreach (var sensor in location.Sensors)
                {
                    sensor.LocationId = location.Id;
                    var dbSensor = await context.Sensors.FindAsync(sensor.Id);
                    if(dbSensor == null)
                    {
                        System.Console.WriteLine($"adding sensor: {sensor.Name}");
                        await context.Sensors.AddAsync(sensor);
                    }
                    else
                    {
                        System.Console.WriteLine($"updating location: {sensor.Name}");
                        dbSensor.Update(sensor);
                    }
                }
                foreach (var alarm in location.Alarms)
                {
                    alarm.LocationId = location.Id;
                    var dbAlarm = await context.Alarms.FindAsync(alarm.Id);
                    if(dbAlarm == null)
                    {
                        System.Console.WriteLine($"adding sensor: {alarm.Name}");
                        await context.Alarms.AddAsync(alarm);
                    }
                    else
                    {
                        System.Console.WriteLine($"updating location: {alarm.Name}");
                        dbAlarm.Update(alarm);
                    }
                }
            }

            await context.SaveChangesAsync();
        }

    }
}