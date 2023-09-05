
using Application.Interfaces.Services;
using Domain.Models;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace WebUI.Razor;

public class MqttBridge : BackgroundService
{
    private readonly ILogger<MqttBridge> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ProjectSettings _projectSettings;
    public IEnumerable<Sensor> Sensors { get; set; }

    public MqttBridge(ILogger<MqttBridge> logger, ProjectSettings projectSettings, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _projectSettings = projectSettings;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("starting MqttBridge");
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var sensorService = scope.ServiceProvider.GetRequiredService<ISensorService>();
            Sensors = await sensorService.GetAllSensorsAsync();
        }

        var mqttFactory = new MqttFactory();

        var mqttClient = mqttFactory.CreateManagedMqttClient();

        var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(_projectSettings.MqttBroker)
                .Build();

        var managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
                .WithClientOptions(mqttClientOptions)
                .Build();

        var mqttSubscribeOptionsBuilder = mqttFactory.CreateSubscribeOptionsBuilder();

    


        foreach (var sensor in Sensors)
        {
                mqttSubscribeOptionsBuilder.WithTopicFilter(f => f.WithTopic(sensor.MqttTopic));
                _logger.LogInformation($"{DateTime.Now.ToShortDateString()} - {DateTime.Now.ToShortTimeString()} : Subscribing to {_projectSettings.MqttBroker}/{sensor.MqttTopic}\n");
        }
        var mqttSubscribeOptions = mqttSubscribeOptionsBuilder.Build();

        await mqttClient.SubscribeAsync(mqttSubscribeOptions.TopicFilters);

        mqttClient.ApplicationMessageReceivedAsync += HandleMessages;

        await mqttClient.StartAsync(managedMqttClientOptions);
    }

    private async Task HandleMessages(MqttApplicationMessageReceivedEventArgs e)
    {
            _logger.LogInformation($"{DateTime.Now.ToShortDateString()} - {DateTime.Now.ToShortTimeString()} : Received mqtt-message\n      ClientID: {e.ClientId}\n      Topic: {e.ApplicationMessage.Topic}\n      Message: {e.ApplicationMessage.ConvertPayloadToString()}\n");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}
