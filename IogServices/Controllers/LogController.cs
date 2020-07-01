using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using IogServices.Models.DTO;
using IogServices.Services;
using IogServices.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IogServices.Controllers
{
    [Route("logs")]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    
    public class LogController : ControllerBase
    {
        private readonly IDeviceLogService _deviceLogService;
        
        
        public LogController(IDeviceLogService deviceLogService)
        {
            _deviceLogService = deviceLogService;
        }
        
        [HttpGet("{serial}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetBySerial([FromRoute] string serial)
        {
            return new OkObjectResult(_deviceLogService.GetBySerial(serial));
        }
       
        [HttpGet("{serial}/{level}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetBySerialFilterByLogLevel([FromRoute] string serial, [FromRoute] LogLevel level)
        {
            return new OkObjectResult(_deviceLogService.GetBySerialFilterByLogLevel(serial,level));
        }
    }
}