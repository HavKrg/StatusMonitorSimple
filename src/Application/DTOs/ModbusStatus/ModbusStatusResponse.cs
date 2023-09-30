using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Application.Dtos;

public class ModbusStatusResponse
{
    public int LocationId { get; set; }
    public bool Success { get; set; }
    public int AttemptedModbusReadings { get; set; }= 0;
    public int SuccessfullModbusReadings { get; set; }= 0;
    public int FailedModbusReadings { get; set; } = 0;
    public DateTime TimeOfLastFailedReading { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;

    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }

    public static implicit operator ModbusStatusResponse?(ModbusStatus? modbusStatus)
    {
        if(modbusStatus == null)
            return null;
        return new ModbusStatusResponse
        {
            LocationId = modbusStatus.LocationId,
            Success = modbusStatus.Success,
            AttemptedModbusReadings = modbusStatus.AttemptedModbusReadings,
            SuccessfullModbusReadings = modbusStatus.SuccessfullModbusReadings,
            FailedModbusReadings = modbusStatus.FailedModbusReadings,
            TimeOfLastFailedReading = modbusStatus.TimeOfLastFailedReading,
            Created = modbusStatus.Created,
            Updated = modbusStatus.Updated,
            ErrorMessage = modbusStatus.ErrorMessage
        };
    }
}
