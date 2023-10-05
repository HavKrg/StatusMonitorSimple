using Application.Dtos;
using Domain.Models;

namespace Application.Dtos;

public class CreateLocation
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ModbusMqttTopic { get; set; } = string.Empty;

    public List<CreateAlarm> Alarms { get; set; } = [];
    public List<CreateSensor> Sensors { get; set; } = [];
    public int MyProperty { get; set; }
    

    public static explicit operator Location(CreateLocation createLocation)
    {
        return new Location(
            createLocation.Id,
            createLocation.Name,
            createLocation.ModbusMqttTopic);
            
    }
}