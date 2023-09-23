namespace WebUI.Razor;

public class ProjectSettings
{
    public required string ProjectName { get; set; }
    public required string MqttBroker { get; set; }
    public required int MqttPort { get; set; }
    public required string MqttUser { get; set; }
    public required string MqttPassword { get; set; }
    public required string MqttClientId { get; set; }
}
