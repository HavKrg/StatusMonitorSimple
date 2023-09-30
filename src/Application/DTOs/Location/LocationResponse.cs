using Application.Dtos;
using Domain.Models;

namespace WebUI.Razor.Models;

public class LocationResponse
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ModbusMqttTopic { get; set; } = string.Empty;
    public List<AlarmResponse> Alarms { get; set; } = [];
    public List<SensorResponse> Sensors { get; set; } = [];
    public ModbusStatusResponse? ModbusStatus { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }

    public static implicit operator LocationResponse?(Location? location)
    {
        if(location == null)
            return null;
        return new LocationResponse
        {
            Id = location.Id,
            LocationId = location.Id,
            Name = location.Name,
            ModbusMqttTopic = location.ModbusMqttTopic,
            Created = location.Created,
            Updated = location.Updated
        };
    }
}