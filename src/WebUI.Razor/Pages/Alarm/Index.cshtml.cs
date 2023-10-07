using Application;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Razor.Pages.Alarm
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMqttClientService _mqttClientService;


        public IndexModel(ILogger<IndexModel> logger, IMqttClientService mqttClientService)
        {
            _logger = logger;
            _mqttClientService = mqttClientService;
        }

        public void OnGetAsync()
        {
            _logger.LogInformation("test");
        }

        

        public async Task<IActionResult> OnPostResetAlarmAsync()
        {
            await _mqttClientService.PublishAsync("jv/resetalarms", "1");
            return RedirectToPage("/Index");
        }
    }
}
