namespace Application.Models;

public class ProjectSettings
{
    public required string ProjectName { get; set; }
    public required string DatabasePath { get; set; }
    public required string MqttBroker { get; set; }
    public required int MqttPort { get; set; }
    public required string MqttUser { get; set; }
    public required string MqttPassword { get; set; }
    public required string MqttClientId { get; set; }
    public string Auth0ClientId { get; set; } = string.Empty;
    public string Auth0Domain { get; set; } = string.Empty;
}