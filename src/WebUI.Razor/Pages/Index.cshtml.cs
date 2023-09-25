using System.Collections.ObjectModel;
using Application;
using Application.Dtos;
using Application.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Razor.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ISensorService _sensorService;
    public IEnumerable<SensorResponse> Sensors { get; set; }
    public List<List<SensorResponse>> SensorGroups { get; set; } = new List<List<SensorResponse>>();
    public ProjectSettings _projectSettings { get; set; }
    public long DatabaseSize { get; set; } = 0;
    

    public IndexModel(ILogger<IndexModel> logger, ISensorService sensorService, ProjectSettings projectSettings)
    {
        _logger = logger;
        _sensorService = sensorService;
        _projectSettings = projectSettings;
    }



    public async void OnGet()
    {
        Sensors = await _sensorService.GetAllSensorsAsync();
        var groups = new List<string>();
        foreach (var sensor in Sensors)
        {
            if(!groups.Contains(sensor.Group))
                groups.Add(sensor.Group);
        }

        foreach (var group in groups)
        {
            List<SensorResponse> sensorsInGroup = Sensors.Where(sensor => sensor.Group == group).ToList();

            SensorGroups.Add(sensorsInGroup);
        }
        // var databasePath = _projectSettings.DatabasePath;

        // if(databasePath != null)
        // {
        //     var DbFileInfo = new FileInfo(databasePath);
        //     DatabaseSize = DbFileInfo.Length/1000;
        // }

        System.Console.WriteLine();
    }
}

