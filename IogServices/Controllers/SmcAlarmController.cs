using System.Net.Mime;
using IogServices.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IogServices.Controllers
{
    [Route("smc-alarms")]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [ApiController]

    public class SmcAlarmController : ControllerBase
    {
        private readonly ISmcAlarmService _smcAlarmService;
        public SmcAlarmController(ISmcAlarmService smcAlarmService)
        {
            _smcAlarmService = smcAlarmService;
        }
        
        [HttpGet("{serial}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetBySerial([FromRoute] string serial)
        {
            return new OkObjectResult(_smcAlarmService.GetBySerial(serial));
        }

    }
}