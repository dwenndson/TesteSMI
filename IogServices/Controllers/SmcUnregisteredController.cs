using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using IogServices.Models.DTO;
using IogServices.Services;
using IogServices.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IogServices.Controllers
{
    [Route("smc-not-registered")]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    
    public class SmcUnregisteredController : ControllerBase
    {
        private readonly ISmcUnregisteredService _smcUnregisteredService;
        
        public SmcUnregisteredController(ISmcUnregisteredService smcUnregisteredService)
        {
            _smcUnregisteredService = smcUnregisteredService;
        }
        
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return new OkObjectResult(_smcUnregisteredService.GetAll());
        }
        

    }
}