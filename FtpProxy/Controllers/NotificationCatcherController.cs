namespace FtpProxy.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;

    [Route("api/notify")]
    [ApiController]
    public class NotificationCatcherController : ControllerBase
    {
        private readonly ILogger<NotificationCatcherController> _logger;

        public NotificationCatcherController(ILogger<NotificationCatcherController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult Notify(string filename)
        {
            try
            {
                _logger.LogInformation($"Received Notification for: {filename}");
                return Ok("Success");
            }
            catch (Exception except)
            {
                _logger.LogError(except, $"Error processing notification");
                return BadRequest(except.Message);
            }
        }
    }
}
