using System.Text.RegularExpressions;
using Domain.Models;

namespace Application;

public class SensorWithoutReadingsResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string MqttTopic { get; set; }
    public required double MinReading { get; set; }
    public required double MaxReading { get; set; }
    public required string Measurement { get; set; }
    public required int PageSize { get; set; }
    public required string Style { get; set; }
    public required string Group { get; set; }

    public static implicit operator SensorWithoutReadingsResponse?(Sensor? sensor)
    {
        if(sensor == null)
            return null;
        return new SensorWithoutReadingsResponse
        {
            Id = sensor.Id,
            Name = sensor.Name,
            Description = sensor.Description,
            MqttTopic = sensor.MqttTopic,
            MinReading = sensor.MinReading,
            MaxReading = sensor.MaxReading,
            Measurement = sensor.Measurement,
            PageSize = sensor.PageSize,
            Style = sensor.Style,
            Group = sensor.Group
        };
    }

}
