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
    [Route("meter-models")]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class MeterModelController : ControllerBase
    {
        private readonly IMeterModelService _meterModelService;
        private readonly IServicesUtils _servicesUtils;

        public MeterModelController(IMeterModelService meterModelService, IServicesUtils servicesUtils)
        {
            _meterModelService = meterModelService;
            _servicesUtils = servicesUtils;
        }

        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return new OkObjectResult(_meterModelService.GetAll());
        }

        [HttpGet("manufacturer/{manufacturerName}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetByManufacturer([FromRoute] string manufacturerName)
        {
            return new OkObjectResult(_meterModelService.GetByManufacturer(manufacturerName));
        }

        [HttpGet("{name}", Name = "GetByNameMeterModelRoute")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById([FromRoute] string name)
        {
            MeterModelDto savedMeterModelDto = _meterModelService.GetByName(name);
            if (savedMeterModelDto == null)
            {
                return new NotFoundResult();
            }
            else
            {
                return new OkObjectResult(savedMeterModelDto);
            }
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Save(MeterModelDto meterModelDto)
        {
            MeterModelDto savedMeterModelDto = _meterModelService.Save(meterModelDto);
            return new CreatedResult(
                _servicesUtils
                    .CreateUri(
                        Request,
                        Url.RouteUrl(
                            "GetByNameMeterModelRoute",
                            new {name = savedMeterModelDto.Name})),
                savedMeterModelDto);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(MeterModelDto meterModelDto)
        {
            MeterModelDto updatedMeterModelDto = _meterModelService.Update(meterModelDto);
            return new OkObjectResult(updatedMeterModelDto);
        }

        [HttpDelete("{name}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete([FromRoute] string name)
        {
            _meterModelService.Deactivate(name);
            return new NoContentResult();
        }
    }
}