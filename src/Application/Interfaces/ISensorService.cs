﻿using Application.Dtos;
using Domain.Models;

namespace Application.Interfaces.Services;

public interface ISensorService
{
    Task<SensorResponse> AddSensorAsync(CreateSensor createSensor);
    Task<SensorResponse?> GetSensorByIdAsync(int sensorId);
    Task<SensorResponse?> GetSensorByMqttTopicAsync(string mqttTopic);
    Task<List<SensorResponse>> GetAllSensorsForLocationAsync(int locationId);
    Task<List<SensorResponse>> GetAllSensorsAsync();
    Task UpdateSensorAsync(CreateSensor updateSensor);
    Task DeleteSensorAsync(int sensorId);
}
