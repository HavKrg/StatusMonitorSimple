using Domain.Models;

namespace Application.Dtos;

public class SensorResponse
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
    public required double Divider { get; set; }
    public SensorReadingResponse? LatestReading { get; set; }
    public List<SensorReadingResponse> Readings { get; set; } = new List<SensorReadingResponse>();
    public int TotalReadings => Readings.Count();

    public static implicit operator SensorResponse?(Sensor? sensor)
    {
        if(sensor == null)
            return null;
        return new SensorResponse
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
            Group = sensor.Group,
            Divider = sensor.Divider
        };
    }

    public int? GetLatestReadingPercent()
    {
        if(LatestReading == null)
            return null;
        return (int)((double)LatestReading.Value / MaxReading * 100);
    }
    
    public bool LatestReadingIsOld()
    {
        if(LatestReading == null)
            return true;
        
        TimeSpan timeSinceLastReading = DateTime.Now.Subtract(LatestReading.Created);
        if(timeSinceLastReading.TotalHours >= 3)
            return true;
        return false;
    }


    public int GetTotalPages(int pageSize)
    {
        return (int)Math.Ceiling((double)TotalReadings / pageSize);
    }
}
