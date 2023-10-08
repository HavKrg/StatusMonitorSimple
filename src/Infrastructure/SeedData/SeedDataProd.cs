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
        if (string.IsNullOrWhiteSpace(requestMessage))
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
                context.RemoveRange(await context.Alarms.ToListAsync());
                context.RemoveRange(await context.Sensors.ToListAsync());
                context.RemoveRange(await context.SensorReadings.ToListAsync());
            }
            else if (requestMessage == "clear readings")
            {
                await context.Database.EnsureCreatedAsync();

                System.Console.WriteLine("deleting readings");
                context.RemoveRange(await context.SensorReadings.ToListAsync());
            }
            else
            {
                System.Console.WriteLine("creating database");
                await context.Database.EnsureCreatedAsync();
            }


            await context.SaveChangesAsync();


            var locations = JsonSerializer.Deserialize<List<Location>>(locationsJson);
            if (locations == null || locations.Count == 0)
            {
                System.Console.WriteLine("No location data provided");
                return;
            }


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
            }
            await context.SaveChangesAsync();
        }

    }
}