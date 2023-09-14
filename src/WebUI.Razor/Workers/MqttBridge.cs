
using Application;
using Application.Dtos;
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
    public IEnumerable<SensorResponse> Sensors { get; set; } = new List<SensorResponse>();

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
            Sensors  = await sensorService.GetAllSensorsAsync();
        }

        var mqttFactory = new MqttFactory();

        var mqttClient = mqttFactory.CreateManagedMqttClient();

        var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(_projectSettings.MqttBroker)
                .WithCredentials(_projectSettings.MqttUser, _projectSettings.MqttPassword)
                .Build();

        var managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
                .WithClientOptions(mqttClientOptions)
                .Build();

        var mqttSubscribeOptionsBuilder = mqttFactory.CreateSubscribeOptionsBuilder();


        foreach (var sensor in Sensors)
        {
            mqttSubscribeOptionsBuilder.WithTopicFilter(f => f.WithTopic(sensor.MqttTopic));
            _logger.LogInformation($"{FormattedDateTime} : Subscribing to {_projectSettings.MqttBroker}/{sensor.MqttTopic}\n");
        }
        var mqttSubscribeOptions = mqttSubscribeOptionsBuilder.Build();

        await mqttClient.SubscribeAsync(mqttSubscribeOptions.TopicFilters);

        mqttClient.ApplicationMessageReceivedAsync += HandleMessages;

        await mqttClient.StartAsync(managedMqttClientOptions);
    }

    private async Task HandleMessages(MqttApplicationMessageReceivedEventArgs e)
    {
        var clientId = e.ClientId;
        var topic = e.ApplicationMessage.Topic;
        var message = e.ApplicationMessage.ConvertPayloadToString();

        _logger.LogInformation($"{FormattedDateTime} : Received mqtt-message\n      ClientID: {clientId}\n      Topic: {topic}\n      Message: {message}\n");

        var validationResult = ValidateMessage(topic, message);

        if (validationResult != null)
        {
            var (sensor, value) = validationResult.Value;
            
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var sensorReadingService = scope.ServiceProvider.GetRequiredService<ISensorReadingService>();
                if (!await sensorReadingService.AddReadingAsync(new CreateSensorReading { SensorId = sensor!.Id, Value = value }))
                {
                    _logger.LogError($"{FormattedDateTime} : Unable to add sensor reading to database");
                    return;
                }
                else
                    _logger.LogInformation($"{FormattedDateTime} : Successfully added reading for {sensor.Name} ({sensor.Id}) : {value} {sensor.Measurement}");
            }
        }
    }



    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }

    private (SensorResponse? sensor, double value)? ValidateMessage(string topic, string message)
    {
        var sensor = Sensors.FirstOrDefault(s => s.MqttTopic == topic);

        if (sensor == null)
        {
            _logger.LogWarning($"{FormattedDateTime} : No sensor with MQTT-topic {topic}");
            return null;
        }
        if (!double.TryParse(message, out double value))
        {
            _logger.LogWarning($"{FormattedDateTime} : Message is not parsable to a double");
            return null;
        }
        if (value > sensor.MaxReading || value < sensor.MinReading)
        {
            _logger.LogWarning($"{FormattedDateTime} : Message is parsable to a double, but it's not within the sensors min/max value range");
            return null;
        }
        return (sensor, value);
    }

    private static string FormattedDateTime => $"{DateTime.Now.ToShortDateString()} - {DateTime.Now.ToShortTimeString()}";
}
