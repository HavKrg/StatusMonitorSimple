using System.Text.Json;
using Application.Dtos;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace Application.Services;

public class MqttClientService : IMqttClientService
{
    private readonly ILogger<MqttClientService> _logger;
    private readonly IManagedMqttClient _mqttClient;
    private readonly ProjectSettings _projectSettings;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private IEnumerable<SensorResponse> _sensors;
    private IEnumerable<AlarmResponse> _alarms;
    private IEnumerable<LocationResponse> _locations;
    private Dictionary<string, Func<MqttApplicationMessageReceivedEventArgs, Task>> _handlers;


    public MqttClientService(ILogger<MqttClientService> logger, ProjectSettings projectSettings, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _mqttClient = new MqttFactory().CreateManagedMqttClient();
        _projectSettings = projectSettings;
        _serviceScopeFactory = serviceScopeFactory;

        _handlers = new Dictionary<string, Func<MqttApplicationMessageReceivedEventArgs, Task>>
        {
            { "/sensor/", HandleSensorReading },
            { "/alarm/", HandleAlarmReading },
            { "/modbus", HandleModbusStatus }
        };
        InitializeAsync().GetAwaiter().GetResult();
    }

    public async Task InitializeAsync()
    {
        _logger.LogInformation("Starting MqttClientService");

        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var sensorService = scope.ServiceProvider.GetRequiredService<ISensorService>();
            var alarmService = scope.ServiceProvider.GetRequiredService<IAlarmService>();
            var locationService = scope.ServiceProvider.GetRequiredService<ILocationService>();
            _sensors = await sensorService.GetAllSensorsAsync();
            _alarms = await alarmService.GetAllAlarmsAsync();
            _locations = await locationService.GetAllLocationsAsync();
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
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(60))
                .Build();

        var managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
                .WithClientOptions(mqttClientOptions)
                .Build();

        try
        {
            await SubscribeToSensorTopics();
            await SubscribeToAlarmtopics();
            await SubscribeToModbusTopics();
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

    public async Task PublishAsync(string topic, string payload)
    {
        var message = new MqttApplicationMessageBuilder()
                        .WithTopic(topic)
                        .WithPayload(payload)
                        .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)
                        .Build();

        await _mqttClient.EnqueueAsync(message);
        SpinWait.SpinUntil(() => _mqttClient.PendingApplicationMessagesCount == 0, 10000);
        if (_mqttClient.PendingApplicationMessagesCount == 0)
            _logger.LogInformation("Successfully published all messages");
        else
            _logger.LogWarning("Was not able to publish all messages");
    }

        private async Task HandleMessages(MqttApplicationMessageReceivedEventArgs e)
    {
        var topic = e.ApplicationMessage.Topic;
        var message = e.ApplicationMessage.ConvertPayloadToString();

        foreach (var handler in _handlers)
        {
            if (topic.Contains(handler.Key))
            {
                await handler.Value(e);
                return;
            }
        }
    }
    
    private async Task HandleModbusStatus(MqttApplicationMessageReceivedEventArgs args)
    {
        var topic = args.ApplicationMessage.Topic;
        var message = args.ApplicationMessage.ConvertPayloadToString();

        _logger.LogInformation($"received '{message}' on topic '{topic}'");

        using (var scope = _serviceScopeFactory.CreateAsyncScope())
        {
            var modbusStatusService = scope.ServiceProvider.GetRequiredService<IModbusStatusService>();
            foreach (var location in _locations)
            {
                if (topic.Contains(location.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    var modbusStatus = JsonSerializer.Deserialize<CreateModbusStatus>(message);
                    if (modbusStatus == null)
                        throw new NullReferenceException("invalid format of message");
                    modbusStatus.LocationId = location.Id;
                    await modbusStatusService.UpdateModbusStatusAsync(modbusStatus);
                }
            }
        }

    }
    private async Task HandleAlarmReading(MqttApplicationMessageReceivedEventArgs args)
    {
        var topic = args.ApplicationMessage.Topic;
        var message = args.ApplicationMessage.ConvertPayloadToString();

        _logger.LogInformation($"received '{message}' on topic  '{topic}'");
        using (var scope = _serviceScopeFactory.CreateAsyncScope())
        {
            var alarmService = scope.ServiceProvider.GetRequiredService<IAlarmService>();

            var alarm = await alarmService.GetAlarmByMqttTopicAsync(topic);

            if (alarm == null)
                throw new NullReferenceException($"no alarm found  for topic '{topic}'");

            if (!TryParseOneZeroToBool(message, out bool status))
            {
                _logger.LogError($"Problem : Message is not parsable to a bool");
            }

            await alarmService.SetAlarmStatusAsync(alarm.Id, status);
        }
    }

    private async Task HandleSensorReading(MqttApplicationMessageReceivedEventArgs args)
    {
        var topic = args.ApplicationMessage.Topic;
        var message = args.ApplicationMessage.ConvertPayloadToString();

        _logger.LogInformation($"received '{message}' on topic '{topic}'");
        using (var scope = _serviceScopeFactory.CreateAsyncScope())
        {
            var sensorReadingService = scope.ServiceProvider.GetRequiredService<ISensorReadingService>();
            var sensorService = scope.ServiceProvider.GetRequiredService<ISensorService>();

            var sensor = await sensorService.GetSensorByMqttTopicAsync(topic);

            if (sensor == null)
                throw new NullReferenceException($"no sensor found  for topic '{topic}'");

            if (sensor.Divider == 0)
            {
                _logger.LogError($"Problem : Sensor divider is zero");
                throw new DivideByZeroException("Divider can not be zero");
            }

            // try to parse the incomming message to a double
            if (!double.TryParse(message, out double value))
            {
                _logger.LogError($"Problem : Message is not parsable to a double");
            }

            value /= sensor.Divider;

            // check if the value is inside the expected range
            if (value > sensor.MaxReading || value < sensor.MinReading)
            {
                _logger.LogError($"Problem : Message is parsable to a double, but it's not within the sensors min/max value range");
            }

            var newSensorReading = await sensorReadingService.AddReadingAsync(new CreateSensorReading { SensorId = sensor!.Id, Value = value, IsValid = true });
            if (newSensorReading != null && newSensorReading.Id > 0 && newSensorReading.SensorId == sensor.Id && newSensorReading.Value == value)
            {
                _logger.LogInformation($"successfully added sensor-reading {newSensorReading.Value} to database for {sensor.Name}");
            }
            else
                _logger.LogError("unable to add sensor-reading to database");

        }
    }

    private async Task SubscribeToAlarmtopics()
    {
        foreach (var alarm in _alarms)
        {
            try
            {
                await _mqttClient.SubscribeAsync(alarm.MqttTopic);
                _logger.LogInformation($"Subscribed to {alarm.MqttTopic}");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
    private async Task SubscribeToSensorTopics()
    {
        foreach (var sensor in _sensors)
        {
            await _mqttClient.SubscribeAsync(sensor.MqttTopic);
            _logger.LogInformation($"Subscribed to {sensor.MqttTopic}");
        }
    }
    private async Task SubscribeToModbusTopics()
    {
        foreach (var location in _locations)
        {
            await _mqttClient.SubscribeAsync(location.ModbusMqttTopic);
            _logger.LogInformation($"Subscribed to {location.ModbusMqttTopic}");
        }
    }
    private async Task ConnectionChanged(EventArgs args)
    {
        if (_mqttClient.IsConnected)
            _logger.LogInformation($"Connected to MQTT-broker '{_projectSettings.MqttBroker}' with clientId '{_projectSettings.MqttClientId}'");
        else
            _logger.LogInformation($"Disconnected from MQTT-broker '{_projectSettings.MqttBroker}'");


    }
    private async Task ConnectionFailed(ConnectingFailedEventArgs args)
    {
        _logger.LogCritical($"Failed to connect to broker\n '{args.Exception.Message}'\n '{args.ConnectResult.UserProperties}'\n'{args.ConnectResult.ResultCode}'");
    }
    private static string FormattedDateTime => $"{DateTime.Now.ToShortDateString()} - {DateTime.Now.ToLongTimeString()}";
    public static bool TryParseOneZeroToBool(string str, out bool result)
    {
        if (str == "1")
        {
            result = true;
            return true;
        }
        else if (str == "0")
        {
            result = false;
            return true;
        }

        result = false;
        return false;
    }
}
