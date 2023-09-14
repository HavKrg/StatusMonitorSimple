using System.Data.Common;
using System.Text.Json;
using Application;
using Application.Dtos;
using Application.Interfaces.Services;
using Application.Services;
using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.SeedData;
using Microsoft.Extensions.Configuration;
using WebUI.Razor;


var builder = WebApplication.CreateBuilder(args);

// Parse configurartion files from argument
if (args.Length == 0)
{
    System.Console.WriteLine("missing argument for config file");
    Environment.Exit(1);
}
else
    builder.Configuration.AddJsonFile(args[0]);

// Extract sensor-list from configuration
var sensors = builder.Configuration.GetSection("Sensors").Get<List<CreateSensor>>();

if (sensors == null)
    System.Console.WriteLine("no sensors in configuration");

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
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<ISensorReadingRepository, SensorReadingRepository>();
builder.Services.AddScoped<ISensorReadingService, SensorReadingService>();
builder.Services.AddScoped<ISensorService, SensorService>();
builder.Services.AddHostedService<MqttBridge>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    var sensorJson = System.Text.Json.JsonSerializer.Serialize(sensors);
    SeedDataProd.Initialize(sensorJson, args.Length > 1 ? args[1] : null);
}
if (app.Environment.IsDevelopment())
{
    SeedDataDev.Initialize();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
