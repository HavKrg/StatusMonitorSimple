namespace Domain.Models;

public class Location : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ModbusMqttTopic { get; set; }

    public List<Alarm> Alarms { get; set; } = [];
    public List<Sensor> Sensors { get; set; } = [];
    public ModbusStatus? ModbusStatus { get; set; }
    public Location()
    {
        
    }

    public Location(int id, string name, string modbusMqttTopic)
    {
        Id = id;
        Name = name;
        ModbusMqttTopic = modbusMqttTopic;
    }

    public void Update(Location location)
    {
        Name = location.Name;
        Updated = DateTime.Now;
    }
}