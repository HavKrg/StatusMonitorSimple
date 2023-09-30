using Domain.Models;

namespace Application.Dtos;

public class CreateModbusStatus
{
    public int LocationId { get; set; }
    public bool Success { get; set; }
    public int AttemptedModbusReadings { get; set; }= 0;
    public int SuccessfullModbusReadings { get; set; }= 0;
    public int FailedModbusReadings { get; set; } = 0;
    public DateTime TimeOfLastFailedReading { get; set; }
    public string LastErrorMessage { get; set; } = string.Empty;
    public static explicit operator ModbusStatus(CreateModbusStatus createModbusStatus)
    {
        return new ModbusStatus(
           createModbusStatus.LocationId, 
           createModbusStatus.Success,
           createModbusStatus.AttemptedModbusReadings, 
           createModbusStatus.SuccessfullModbusReadings, 
           createModbusStatus.FailedModbusReadings,
           createModbusStatus.LastErrorMessage,
           createModbusStatus.TimeOfLastFailedReading
        );
    }
}
