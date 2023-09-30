using Application.Dtos;

namespace Application.Interfaces;
public interface IModbusStatusService
{
    Task<ModbusStatusResponse?> GetModbusStatusByLocationIdAsync(int locationId);
    Task UpdateModbusStatusAsync(CreateModbusStatus updatedModbusStatus);
}