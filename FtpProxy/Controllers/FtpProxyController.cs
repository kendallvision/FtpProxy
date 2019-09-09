namespace FtpProxy.Controllers
{
    using FtpProxy.Extensions;
    using FtpProxy.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;

    [Route("api/proxy")]
    [ApiController]
    public class FtpProxyController : ControllerBase
    {
        private readonly IFileSender Sender;
        private readonly ILogger<FtpProxyController> Logger;

        public FtpProxyController(ILogger<FtpProxyController> logger, IFileSender sender)
        {
            this.Logger = logger;
            this.Sender = sender;
        }

        [HttpGet]
        public ActionResult Ping()
        {
            Logger.LogInformation($"Ping called at {DateTime.UtcNow}");
            return Ok($"Alive at {DateTime.UtcNow} (UTC)");
        }

        [HttpPost]
        public ActionResult SendFile(string source)
        {
            try
            {
                Logger.LogInformation($"FtpProxy.SendFile Called for: {source}");

                source.CheckNullOrEmpty(nameof(source));

                Sender.SendFile(source);

                return Ok("Success");
            }
            catch (Exception except)
            {
                Logger.LogError(except, $"Error sending file {source}");
                return BadRequest(except.ToString());
            }
        }
    }
}