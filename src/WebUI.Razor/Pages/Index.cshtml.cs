using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Razor.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ISensorService _sensorService;

    public IndexModel(ILogger<IndexModel> logger, ISensorService sensorService)
    {
        _logger = logger;
        _sensorService = sensorService;
    }

    public async void OnGet()
    {
        var sensors = await _sensorService.GetAllSensorsAsync();

        foreach (var sensor in sensors)
        {
            _logger.LogInformation($"{sensor.Id} - {sensor.Name}");
        }


    }
}
