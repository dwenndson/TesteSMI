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
    [Route("keys")]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    
    public class KeysController : ControllerBase
    {
        private readonly IMeterKeyService _meterKeyService;
        private readonly IServicesUtils _servicesUtils;
        
        public KeysController(IMeterKeyService meterKeyService, IServicesUtils servicesUtils)
        {
            _meterKeyService = meterKeyService;
            _servicesUtils = servicesUtils;
        }
        
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return new OkObjectResult(_meterKeyService.GetAll());
        }
        
        [HttpGet("{serial}", Name = "GetBySerialRoute")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBySerial([FromRoute] string serial)
        {
            var savedMeterDto = _meterKeyService.GetBySerial(serial);
            if (savedMeterDto == null)
                return new NotFoundResult();
            return new OkObjectResult(savedMeterDto);
            
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Save(KeysDto keysDto)
        {
            var keys = _meterKeyService.Create(keysDto);
            return new CreatedResult(
                _servicesUtils
                    .CreateUri(
                        Request,
                        Url.RouteUrl(
                            "GetBySerialRoute",
                            new {serial = keys.Serial})),
                keysDto);
        }
        
        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(KeysDto keysDto)
        { 
            return new OkObjectResult(_meterKeyService.Update(keysDto));
        }
        
        [HttpPost]
        [Route("import")]
        public IActionResult Import(IFormFile file)
        {
            _meterKeyService.ProcessFile(file);
            return Ok();
        }

    }
}