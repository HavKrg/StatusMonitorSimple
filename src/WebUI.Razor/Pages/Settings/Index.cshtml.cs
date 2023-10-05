using Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Razor.Pages.Settings
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ISensorReadingService _sensorReadingService;
        public long DbSize { get; set; }


        public IndexModel(ILogger<IndexModel> logger, ISensorReadingService sensorReadingService)
        {
            _logger = logger;
            _sensorReadingService = sensorReadingService;
        }

        public void OnGetAsync()
        {
            DbSize = GetDatabaseSize();
        }

        private long GetDatabaseSize()
        {
            if(System.IO.File.Exists("SensorMonitor.db"))
            {
                FileInfo fi = new FileInfo("SensorMonitor.db");
                return fi.Length/1000;
            }
            else
                return 0;
        }

        public async Task OnPostDeleteAllSensorReadingsAsync ()
        {
            await _sensorReadingService.DeleteAllSensorReadingsAsync();
            RedirectToPage("/Index");
        }
    }
}
