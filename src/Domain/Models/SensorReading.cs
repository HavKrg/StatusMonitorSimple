namespace Domain.Models;

public class SensorReading : BaseEntity
{

    public int SensorId { get; set; }
    public double Value { get; set; }
    public bool IsValid { get; set; }
    public SensorReading(int sensorId, double value, bool isValid)
    {
        SensorId = sensorId;
        Value = value;
        IsValid = isValid;
    }

    public SensorReading()
    {
        
    }
    
}