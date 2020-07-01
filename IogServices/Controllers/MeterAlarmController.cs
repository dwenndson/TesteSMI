using System.Net.Mime;
using IogServices.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IogServices.Controllers
{
    [Route("meter-alarms")]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [ApiController]

    public class MeterAlarmController : ControllerBase
    {
        private readonly IMeterAlarmService _meterAlarmService;
        public MeterAlarmController(IMeterAlarmService meterAlarmService)
        {
            _meterAlarmService = meterAlarmService;
        }
        
        [HttpGet("{serial}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetBySerial([FromRoute] string serial)
        {
            return new OkObjectResult(_meterAlarmService.GetBySerial(serial));
        }

    }
}