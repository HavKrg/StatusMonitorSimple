using System.Runtime.CompilerServices;
using System.Text.Json;
using Application;
using Application.Dtos;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Models;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace WebUI.Razor.Workers.MqttBridge;
public class MqttBridge : BackgroundService
{
    private readonly ILogger<MqttBridge> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private IMqttClientService _mqttClientService;
    
    public MqttBridge(ILogger<MqttBridge> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("starting MqttBridge");

        using (var scope = _serviceScopeFactory.CreateScope())
        {
            _mqttClientService = scope.ServiceProvider.GetRequiredService<IMqttClientService>();
            // You can now use _mqttClientService in your methods
            await _mqttClientService.InitializeAsync();
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}

