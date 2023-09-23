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

        var mqttFactory = new MqttFactory();

        _mqttClient = mqttFactory.CreateManagedMqttClient();

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
                _logger.LogInformation($"Subscribed to {sensor.MqttTopic}");
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
            _logger.LogInformation($"Connection status changed : connected");
        else
            _logger.LogInformation($"Connection status changed : disconnected");
        
        _logger.LogInformation($"{args.ToString()}");
        await Task.CompletedTask;
        
    }

    private async Task ConnectionFailed(ConnectingFailedEventArgs args)
    {
        _logger.LogCritical($"Failed to connect to broker\n {args.Exception.Message}\n {args.ConnectResult.UserProperties}\n{args.ConnectResult.ResultCode}");

    }



    private async Task HandleMessages(MqttApplicationMessageReceivedEventArgs e)
    {
        var clientId = e.ClientId;
        var topic = e.ApplicationMessage.Topic;
        var message = e.ApplicationMessage.ConvertPayloadToString();


        var validationResult = ValidateMessage(clientId, topic, message);

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

    private (SensorResponse? sensor, double value)? ValidateMessage(string clientId, string topic, string message)
    {
        var logMessage = $"{FormattedDateTime} : Received mqtt-message\n      ClientID: {clientId}\n      Topic: {topic}\n      Message: {message}\n";
        

        var sensor = Sensors.FirstOrDefault(s => s.MqttTopic == topic);

        if (sensor == null)
        {
            _logger.LogWarning($"{logMessage}{FormattedDateTime} : No sensor with MQTT-topic {topic}");
            return null;
        }
        if (!double.TryParse(message, out double value))
        {
            _logger.LogWarning($"{logMessage}{FormattedDateTime} : Message is not parsable to a double");
            return null;
        }
        if (value > sensor.MaxReading || value < sensor.MinReading)
        {
            _logger.LogWarning($"{logMessage}{FormattedDateTime} : Message is parsable to a double, but it's not within the sensors min/max value range");
            return null;
        }
        return (sensor, value);
    }

    private static string FormattedDateTime => $"{DateTime.Now.ToShortDateString()} - {DateTime.Now.ToShortTimeString()}";
}
