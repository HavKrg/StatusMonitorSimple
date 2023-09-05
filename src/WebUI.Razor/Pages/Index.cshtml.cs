using Application.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Razor.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ISensorService _sensorService;
    public IEnumerable<Sensor> Sensors { get; set; }

    public IndexModel(ILogger<IndexModel> logger, ISensorService sensorService)
    {
        _logger = logger;
        _sensorService = sensorService;
    }


    
    public async void OnGet()
    {
        Sensors = await _sensorService.GetAllSensorsAsync();
    }
}
