using Application.Dtos;
using Infrastructure;

namespace Application;

public interface ISensorReadingService
{
    Task<SensorReadingResponse> AddReadingAsync(CreateSensorReading createSensorReading);
    Task<SensorReadingResponse?> GetLatestReadingForSensorAsync(int sensorId);
    Task<PaginatedDataResponse<List<SensorReadingResponse>>?> GetPaginatedSensorReadingsAsync(int sensorId, int pageNumber);
    Task<List<SensorReadingResponse?>> GetAllReadingsForSensorAsync(int sensorId);
    Task DeleteAllSensorReadingsAsync();
}

