using System.Collections.ObjectModel;
using Application;
using Application.Dtos;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Models;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Razor.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ISensorService _sensorService;
    private readonly IAlarmService _alarmService;
    private readonly ILocationService _locationService;
    private readonly IModbusStatusService _modbusStatusService;
    public IEnumerable<SensorResponse> Sensors { get; set; }
    public IEnumerable<AlarmResponse> Alarms { get; set; }
    public IEnumerable<LocationResponse> Locations { get; set; }
    public List<List<SensorResponse>> SensorGroups { get; set; } = new List<List<SensorResponse>>();
    public ProjectSettings _projectSettings { get; set; }
    public long DatabaseSize { get; set; } = 0;


    public IndexModel(ILogger<IndexModel> logger, ISensorService sensorService, IAlarmService alarmService, ILocationService locationService, ProjectSettings projectSettings, IModbusStatusService modbusStatusService)
    {
        _logger = logger;
        _sensorService = sensorService;
        _alarmService = alarmService;
        _locationService = locationService;
        _projectSettings = projectSettings;
        _modbusStatusService = modbusStatusService;
    }



    public async void OnGet()
    {
        Locations = await _locationService.GetAllLocationsAsync();
        foreach (var location in Locations)
        {
            location.ModbusStatus = await _modbusStatusService.GetModbusStatusByLocationIdAsync(location.Id);
            location.Sensors = (List<SensorResponse>)await _sensorService.GetAllSensorsForLocationAsync(location.Id);
            location.Alarms = (List<AlarmResponse>)await _alarmService.GetAllAlarmsForLocationAsync(location.Id);
        }

        var DbFileInfo = new FileInfo("SensorMonitor.db");
        DatabaseSize = DbFileInfo.Length/1000;
        

        System.Console.WriteLine();
    }
}

