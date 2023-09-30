using Application.Dtos;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services;
public class ModbusStatusService : IModbusStatusService
{
    private readonly ILogger<ModbusStatusService>  _logger;
    private readonly IModbusStatusRepository _modbusStatusRepository;

    public ModbusStatusService(ILogger<ModbusStatusService> logger, IModbusStatusRepository modbusStatusRepository)
    {
        _logger = logger;
        _modbusStatusRepository = modbusStatusRepository;
    }

    public async Task<ModbusStatusResponse?> GetModbusStatusByLocationIdAsync(int locationId)
    {
        return await _modbusStatusRepository.GetModbusStatusByLocationIdAsync(locationId);
    }

    public async Task UpdateModbusStatusAsync(CreateModbusStatus updatedModbusStatus)
    {
        await _modbusStatusRepository.UpdateModbusStatusAsync((ModbusStatus)updatedModbusStatus);
    }
}