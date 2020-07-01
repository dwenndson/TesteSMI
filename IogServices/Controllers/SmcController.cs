using System.Net.Mime;
using IogServices.Models.DTO;
using IogServices.Services;
using IogServices.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IogServices.Controllers
{
    [Route("smcs")]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class SmcController : ControllerBase
    {
        private readonly ISmcService _smcService;
        private readonly IServicesUtils _servicesUtils;
        private readonly IEventService _eventService;

        public SmcController(ISmcService smcService, IServicesUtils servicesUtils, IEventService eventService)
        {
            _smcService = smcService;
            _servicesUtils = servicesUtils;
            _eventService = eventService;
        }
        
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return new OkObjectResult(_smcService.GetAll());
        }
        
        [HttpGet("comissioned")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllComissioned()
        {
            return new OkObjectResult(_smcService.GetAllComissioned());
        }
        
        [HttpGet("not-comissioned")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllNotComissioned()
        {
            return new OkObjectResult(_smcService.GetAllNotComissioned());
        }

        [HttpGet("{serial}", Name = "GetBySerialSmcRoute")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBySerial([FromRoute] string serial)
        {
            SmcDto savedSmcDto = _smcService.GetBySerial(serial);
            if (savedSmcDto == null)
            {
                return new NotFoundResult();
            }
            else
            {
                return new OkObjectResult(savedSmcDto);
            }
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Save(SmcDto smcDto)
        {
            SmcDto savedSmcDto = _smcService.Save(smcDto);
            _eventService.ASmcWasSavedEvent(this, new IoGServicedEventArgs<SmcDto>(savedSmcDto));
            return new CreatedResult(
                _servicesUtils
                    .CreateUri(
                        Request,
                        Url.RouteUrl(
                            "GetBySerialSmcRoute",
                            new {serial = savedSmcDto.Serial})),
                savedSmcDto);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(SmcDto smcDto)
        {
            SmcDto updatedSmcDto = _smcService.Update(smcDto);
            return new OkObjectResult(updatedSmcDto);
        }

        [HttpDelete("{serial}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete([FromRoute] string serial)
        {
            _smcService.Deactivate(serial);
            return new NoContentResult();
        }
        
        [HttpGet("{serial}/meters")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBySmc([FromRoute] string serial)
        {
            var savedMeterDto = _smcService.GetMeterBySmc(serial);
            if (savedMeterDto == null)
                return new NotFoundResult();
            return new OkObjectResult(savedMeterDto);
        }
    }
}