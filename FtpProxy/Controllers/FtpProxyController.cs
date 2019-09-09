namespace FtpProxy.Controllers
{
    using FtpProxy.Extensions;
    using FtpProxy.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using System;

    [Route("api/proxy")]
    [ApiController]
    public class FtpProxyController : ControllerBase
    {
        private readonly IFileSender Sender;

        public FtpProxyController(IFileSender sender)
        {
            this.Sender = sender;
        }

        [HttpGet]
        public ActionResult Ping()
        {
            return Ok($"Alive at {DateTime.UtcNow} (UTC)");
        }

        [HttpPost]
        public ActionResult SendFile(string source)
        {
            try
            {
                source.CheckNullOrEmpty(nameof(source));

                this.Sender.SendFile(source);

                return Ok(true);
            }
            catch (Exception except)
            {
                // TODO: Log the event and return error
                return BadRequest(except.ToString());
            }
        }
    }
}