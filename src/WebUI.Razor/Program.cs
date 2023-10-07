using System.Net.WebSockets;
using Application;
using Application.Dtos;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Models;
using Application.Services;
using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.SeedData;


var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Parse configurartion files from argument
if (args.Length == 0)
{
    System.Console.WriteLine("missing argument for config file");
    Environment.Exit(1);
}
else
{
    System.Console.WriteLine($"Getting config-files from {args[0]}");
    builder.Configuration.AddJsonFile(args[0] + "projectSettings.config", optional: true, reloadOnChange: true);
    builder.Configuration.AddJsonFile(args[0] + "loggSettings.config", optional: true, reloadOnChange: true);
    builder.Configuration.AddJsonFile(args[0] + "sensors.json", optional: true, reloadOnChange: true);
}



// Extract sensor-list from configuration
var locations = builder.Configuration.GetSection("Locations").Get<List<CreateLocation>>();

if (locations == null)
    System.Console.WriteLine("no locations in configuration");

// Extract project-settings from configuration
var projectSettings = builder.Configuration.GetSection("ProjectSettings").Get<ProjectSettings>();

if (projectSettings == null)
{
    System.Console.WriteLine("no project-settings in configuration");
    Environment.Exit(1);
}
else
    builder.Services.AddSingleton(projectSettings);


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<StatusMonitorDbContext>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IAlarmRepository, AlarmRepository>();
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<ISensorReadingRepository, SensorReadingRepository>();
builder.Services.AddScoped<IModbusStatusRepository, ModbusStatusRepository>();

builder.Services.AddScoped<ISensorReadingService, SensorReadingService>();
builder.Services.AddScoped<ISensorService, SensorService>();
builder.Services.AddScoped<IAlarmService, AlarmService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IModbusStatusService, ModbusStatusService>();
builder.Services.AddSingleton<IMqttClientService, MqttClientService>();
// builder.Services.AddHostedService<MqttBridge>();


var app = builder.Build();

// Configure the HTTP request pipeline.

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    System.Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");

    var locationsJson = System.Text.Json.JsonSerializer.Serialize(locations);
    SeedDataProd.Initialize(locationsJson, args.Length > 1 ? args[1] : null);
}
if (app.Environment.IsDevelopment())
{
    System.Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
    // var sensorJson = System.Text.Json.JsonSerializer.Serialize(sensors);
    // SeedDataProd.Initialize(sensorJson, args.Length > 1 ? args[1] : null);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
