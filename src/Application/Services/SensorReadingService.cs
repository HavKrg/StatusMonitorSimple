
using Application.Dtos;
using Domain.Models;
using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application;

public class SensorReadingService : ISensorReadingService
{
    private readonly ILogger<SensorReadingService> _logger;
    private readonly ISensorReadingRepository _sensorReadingRepository;

    public SensorReadingService(ILogger<SensorReadingService> logger, ISensorReadingRepository sensorReadingRepository)
    {
        _logger = logger;
        _sensorReadingRepository = sensorReadingRepository;
    }

    public async Task<SensorReadingResponse> AddReadingAsync(CreateSensorReading createSensorReading)
    {
        return await _sensorReadingRepository.AddSensorReadingAsync((SensorReading)createSensorReading);
    }

    public async Task<List<SensorReadingResponse?>> GetAllReadingsForSensorAsync(int sensorId)
    {
        var readings = await _sensorReadingRepository.GetAllSensorReadingsForSensorAsync(sensorId);
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
