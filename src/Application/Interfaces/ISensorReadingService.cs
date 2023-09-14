using Application.Dtos;
using Infrastructure;

namespace Application;

public interface ISensorReadingService
{
    Task<bool> AddReadingAsync(CreateSensorReading createSensorReading);
    Task<SensorReadingResponse?> GetLatestReadingForSensorAsync(Guid sensorId);
    Task<PaginatedDataResponse<List<SensorReadingResponse>>?> GetPaginatedSensorReadingsAsync(Guid sensorId, int pageNumber);
}

