
using Application.Dtos;
using Domain.Models;
using Infrastructure;
using Infrastructure.Interfaces;

namespace Application;

public class SensorReadingService : ISensorReadingService
{
    private readonly ISensorReadingRepository _sensorReadingRepository;

    public SensorReadingService(ISensorReadingRepository sensorReadingRepository)
    {
        _sensorReadingRepository = sensorReadingRepository;
    }

    public async Task<bool> AddReadingAsync(CreateSensorReading createSensorReading)
    {
        return await _sensorReadingRepository.AddReadingAsync((SensorReading)createSensorReading);
    }

    public async Task<List<SensorReadingResponse?>> GetAllReadingsForSensorAsync(int sensorId)
    {
        var readings = await _sensorReadingRepository.GetAllReadingsForSensorAsync(sensorId);
        var response = readings.Select(r => (SensorReadingResponse)r).ToList();
        return response;
    }

    public async Task<SensorReadingResponse?> GetLatestReadingForSensorAsync(int sensorId)
    {
        var sensorReadingResponse = await _sensorReadingRepository.GetLatestReadingForSensorAsync(sensorId);
        if(sensorReadingResponse == null)
            return null;
        return sensorReadingResponse;
        
    }

    public async Task<PaginatedDataResponse<List<SensorReadingResponse>>?> GetPaginatedSensorReadingsAsync(int sensorId, int pageNumber)
    {
        var readings = await _sensorReadingRepository.GetPaginatedSensorReadings(sensorId, pageNumber);

        if(readings == null || !readings.Data.Any())
            return null;
        
        return  new PaginatedDataResponse<List<SensorReadingResponse>>()
        {
            CurrentPage = readings.CurrentPage,
            PageSize = readings.PageSize,
            TotalPages = readings.TotalPages,
            Data = readings.Data.Select(r => (SensorReadingResponse)r).ToList()
        };

    }
}
