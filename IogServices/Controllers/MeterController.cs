using System;
using System.Net.Mime;
using IogServices.Models.DTO;
using IogServices.Services;
using IogServices.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IogServices.Controllers
{
    [Route("meters")]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class MeterController : ControllerBase
    {
        private readonly IMeterService _meterService;
        private readonly IServicesUtils _servicesUtils;
        private readonly IEventService _eventService;

        public MeterController(IMeterService meterService, IServicesUtils servicesUtils,
            IEventService eventService)
        {
            _meterService = meterService;
            _servicesUtils = servicesUtils;
            _eventService = eventService;
        }

        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return new OkObjectResult(_meterService.GetAll());
        }

        [HttpGet("{serial}", Name = "GetBySerialMeterRoute")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBySerial([FromRoute] string serial)
        {
            var savedMeterDto = _meterService.GetBySerial(serial);
            if (savedMeterDto == null)
                return new NotFoundResult();
            return new OkObjectResult(savedMeterDto);
            
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Save(MeterDto meterDto)
        {
            MeterDto savedMeterDto = _meterService.Save(meterDto);
            _eventService.AMeterWasSavedEvent(this, new IoGServicedEventArgs<MeterDto>(savedMeterDto));
            return new OkResult();

        }
        
        [HttpGet("comissioned")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllComissioned()
        {
            return new OkObjectResult(_meterService.GetMetersComissioned());
        }
        
        [HttpGet("not-comissioned")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllNotComissioned()
        {
            return new OkObjectResult(_meterService.GetMetersNotComissioned());
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(MeterDto meterDto)
        {
            MeterDto updatedMeterDto = _meterService.Update(meterDto);
            return new OkObjectResult(updatedMeterDto);
        }

        [HttpDelete("{serial}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete([FromRoute] string serial)
        {
            _meterService.Deactivate(serial);
            return new NoContentResult();
        }
        

    }
}