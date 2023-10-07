namespace Application.Interfaces;

public interface IMqttClientService
{
    Task PublishAsync(string topic, string payload);
    Task InitializeAsync();
}