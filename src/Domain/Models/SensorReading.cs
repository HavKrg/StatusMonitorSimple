namespace Domain.Models;

public class SensorReading : BaseEntity
{
    public SensorReading(int sensorId, double value)
    {
        SensorId = sensorId;
        Value = value;
    }

    public SensorReading()
    {
        
    }
    public int SensorId { get; set; }
    public double Value { get; set; }
}