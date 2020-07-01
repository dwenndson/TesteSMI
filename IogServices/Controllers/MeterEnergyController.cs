using System;
using System.Net.Mime;
using System.Security.Policy;
using IogServices.Models.DTO;
using IogServices.Services;
using IogServices.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IogServices.Controllers
{
    [Route("meter-energies")]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class MeterEnergyController : ControllerBase
    {
        private readonly IMeterEnergyService _meterEnergyService;

        public MeterEnergyController(IMeterEnergyService meterEnergyService)
        {
            _meterEnergyService = meterEnergyService;
        }

        [HttpGet("{serial}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllBySerial([FromRoute] string serial)
        {
            return new OkObjectResult(_meterEnergyService.GetAllBySerial(serial));
        }
        
        
    }
}