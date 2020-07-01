using System;
using System.Net.Mime;
using System.Security.Policy;
using IogServices.Models.DTO;
using IogServices.Services;
using IogServices.Services.Impl;
using IogServices.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IogServices.Controllers
{
    [Route("manufacturers")]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class ManufacturerController : ControllerBase
    {
        private readonly IManufacturerService _manufacturerService;
        private readonly IServicesUtils _servicesUtils;

        public ManufacturerController(IManufacturerService manufacturerService, IServicesUtils servicesUtils)
        {
            _manufacturerService = manufacturerService;
            _servicesUtils = servicesUtils;
        }

        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return new OkObjectResult(_manufacturerService.GetAll());
        }

        [HttpGet("{name}", Name = "GetByNameManufacturerRoute")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByName([FromRoute] string name)
        {
            ManufacturerDto savedManufacturerDto = _manufacturerService.GetByName(name);
            if (savedManufacturerDto == null)
            {
                return new NotFoundResult();
            }
            else
            {
                return new OkObjectResult(savedManufacturerDto);
            }
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Save(ManufacturerDto manufacturerDto)
        {
            ManufacturerDto savedManufacturerDto = _manufacturerService.Save(manufacturerDto);
            return new CreatedResult(
                _servicesUtils
                    .CreateUri(
                        Request,
                        Url.RouteUrl(
                            "GetByNameManufacturerRoute",
                            new {name = savedManufacturerDto.Name })),
                savedManufacturerDto);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(ManufacturerDto manufacturerDto)
        {
            ManufacturerDto updatedManufacturerDto = _manufacturerService.Update(manufacturerDto);
            return new OkObjectResult(updatedManufacturerDto);
        }

        [HttpDelete("{name}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete([FromRoute] string name)
        {
            _manufacturerService.Deactivate(name);
            return new NoContentResult();
        }
    }
}