using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;
public class ModbusStatusRepository : IModbusStatusRepository
{
    private readonly ILogger<ModbusStatusRepository> _logger;
    private readonly StatusMonitorDbContext _context;

    public ModbusStatusRepository(ILogger<ModbusStatusRepository> logger, StatusMonitorDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<ModbusStatus?> GetModbusStatusByLocationIdAsync(int locationId)
    {
        return await _context.ModbusStatuses.FirstOrDefaultAsync(m => m.LocationId == locationId);
    }

    public async Task UpdateModbusStatusAsync(ModbusStatus updatedModbusStatus)
    {
        var modbusStatus = await _context.ModbusStatuses.FirstOrDefaultAsync(m => m.LocationId == updatedModbusStatus.LocationId);
        if(modbusStatus == null)
            throw new  NullReferenceException($"no modbusstatus with locationId {updatedModbusStatus.LocationId}");

        modbusStatus.Update(updatedModbusStatus);
        _context.Entry(modbusStatus).State  = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}