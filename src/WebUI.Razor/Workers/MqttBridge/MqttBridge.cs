using Application;
using Application.Dtos;
using Application.Interfaces.Services;
using Domain.Models;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace WebUI.Razor.Workers.MqttBridge;

public class MqttBridge : BackgroundService
{
    private readonly ILogger<MqttBridge> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ProjectSettings _projectSettings;
    public IEnumerable<SensorResponse> Sensors { get; set; } = new List<SensorResponse>();
    private IManagedMqttClient _mqttClient { get; set; }

    public MqttBridge(ILogger<MqttBridge> logger, ProjectSettings projectSettings, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _projectSettings = projectSettings;
        _serviceScopeFactory = serviceScopeFactory;
        _mqttClient = new MqttFactory().CreateManagedMqttClient();

    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("starting MqttBridge");
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var sensorService = scope.ServiceProvider.GetRequiredService<ISensorService>();
            var sensors = await sensorService.GetAllSensorsAsync();
            Sensors = sensors.OrderBy(s => s.Id);
        }

        
        _mqttClient.ApplicationMessageReceivedAsync += HandleMessages;
        _mqttClient.ConnectingFailedAsync += ConnectionFailed;
        _mqttClient.ConnectionStateChangedAsync += ConnectionChanged;


        var tlsOptions = new MqttClientTlsOptionsBuilder()
            .UseTls()
            .Build();


        var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(_projectSettings.MqttBroker, _projectSettings.MqttPort)
                .WithClientId(_projectSettings.MqttClientId)
                .WithCredentials(_projectSettings.MqttUser, _projectSettings.MqttPassword)
                .WithCleanSession()
                .WithTlsOptions(tlsOptions)
                .WithKeepAlivePeriod(TimeSpan.FromMinutes(600))
                .Build();

        var managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
                .WithClientOptions(mqttClientOptions)
                .Build();

        try
        {
            foreach (var sensor in Sensors)
            {
                await _mqttClient.SubscribeAsync(sensor.MqttTopic);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Something went wrong when subscribing. {ex.Message}");
            throw;
        }

        try
        {
            await _mqttClient.StartAsync(managedMqttClientOptions);
        }
        catch (System.Exception ex)
        {
            _logger.LogInformation(ex.Message);
            throw;
        }
    }

    private async Task ConnectionChanged(EventArgs args)
    {
        if (_mqttClient.IsConnected)
            _logger.LogInformation($"Connected to MQTT-broker '{_projectSettings.MqttBroker}' with clientId '{_projectSettings.MqttClientId}'");
        else
            _logger.LogInformation($"Disconnected from MQTT-broker '{_projectSettings.MqttBroker}'");
        
        await Task.CompletedTask;
        
    }

    private async Task ConnectionFailed(ConnectingFailedEventArgs args)
    {
        _logger.LogCritical($"Failed to connect to broker\n '{args.Exception.Message}'\n '{args.ConnectResult.UserProperties}'\n'{args.ConnectResult.ResultCode}'");
        await Task.CompletedTask;
    }



    private async Task HandleMessages(MqttApplicationMessageReceivedEventArgs e)
    {
        var clientId = e.ClientId;
        var topic = e.ApplicationMessage.Topic;
        var message = e.ApplicationMessage.ConvertPayloadToString();

        var logMessage = $"{FormattedDateTime} : Received mqtt-message\n        ClientID: '{clientId}'\n        Topic: '{topic}'\n        Message: '{message}'\n";

        var validationResult = ValidateMessage(logMessage, topic, message);

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
                    _logger.LogInformation($"{FormattedDateTime} : Successfully added reading for '{sensor.Group} - {sensor.Name}' : '{value} {sensor.Measurement}'");
            }
        }
        await Task.CompletedTask;

    }



    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }

    private (SensorResponse? sensor, double value)? ValidateMessage(string logMessage, string topic, string message)
    {

        var sensor = Sensors.FirstOrDefault(s => s.MqttTopic == topic);

        if (sensor == null)
        {
            _logger.LogError($"{logMessage}       Problem : No sensor with MQTT-topic {topic}");
            return null;
        }
        if (!double.TryParse(message, out double value))
        {
            _logger.LogError($"{logMessage}       Problem : Message is not parsable to a double");
            return null;
        }
        if (value > sensor.MaxReading || value < sensor.MinReading)
        {
            _logger.LogError($"{logMessage}        Problem : Message is parsable to a double, but it's not within the sensors min/max value range");
            return null;
        }
        return (sensor, value);
    }

    private static string FormattedDateTime => $"{DateTime.Now.ToShortDateString()} - {DateTime.Now.ToLongTimeString()}";
}
