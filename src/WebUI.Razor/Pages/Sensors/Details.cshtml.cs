using Application;
using Application.Dtos;
using Application.Interfaces.Services;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Razor.Pages.Sensors
{
    public class DetailsModel : PageModel
    {

        private readonly ILogger<DetailsModel> _logger;
        private readonly ISensorService _sensorService;
        private readonly ISensorReadingService _sensorReadingService;

        public DetailsModel(ILogger<DetailsModel> logger, ISensorService sensorService, ISensorReadingService sensorReadingService)
        {
            _logger = logger;
            _sensorService = sensorService;
            _sensorReadingService = sensorReadingService;
        }

        public SensorResponse Sensor { get; set; }
        public PaginatedDataResponse<List<SensorReadingResponse>>? Readings { get; set; }
        public async Task OnGetAsync(int sensorId, int pageNumber = 0)
        {
            Sensor = await _sensorService.GetSensorByIdAsync(sensorId);
            Readings = await _sensorReadingService.GetPaginatedSensorReadingsAsync(sensorId, pageNumber);
        }
    }
}
