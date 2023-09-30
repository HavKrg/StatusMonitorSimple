namespace Domain.Models;

public class ModbusStatus : BaseEntity
{
    public int LocationId { get; set; }
    public bool Success { get; set; }
    public int AttemptedModbusReadings { get; set; }= 0;
    public int SuccessfullModbusReadings { get; set; }= 0;
    public int FailedModbusReadings { get; set; } = 0;
    public DateTime TimeOfLastFailedReading { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;

    public ModbusStatus(int locationId, bool success,  int attemptedModbusReadings, int successfullModbusReadings, 
                        int failedModbusReadings, string errorMessage, DateTime timeOfLastFailedReading)
    {
        LocationId = locationId;
        Success = success;
        AttemptedModbusReadings = attemptedModbusReadings;
        SuccessfullModbusReadings = successfullModbusReadings;
        FailedModbusReadings = failedModbusReadings;
        ErrorMessage = errorMessage;
        TimeOfLastFailedReading = timeOfLastFailedReading;
    }
    public void Update(ModbusStatus updatedModbusStatus)
    {
        Success =  updatedModbusStatus.Success;
        AttemptedModbusReadings = updatedModbusStatus.AttemptedModbusReadings;
        SuccessfullModbusReadings = updatedModbusStatus.SuccessfullModbusReadings;
        FailedModbusReadings =  updatedModbusStatus.FailedModbusReadings;
        TimeOfLastFailedReading = updatedModbusStatus.TimeOfLastFailedReading;
        ErrorMessage = updatedModbusStatus.ErrorMessage;
    }
}
