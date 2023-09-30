using Domain.Models;

namespace Infrastructure.Interfaces;
public interface IModbusStatusRepository
{
    Task<ModbusStatus?> GetModbusStatusByLocationIdAsync(int locationId);
    Task UpdateModbusStatusAsync(ModbusStatus updatedModbusStatus);
}