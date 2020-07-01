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
    [Route("smc-models")]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class SmcModelController : ControllerBase
    {
        private readonly ISmcModelService _smcModelService;
        private readonly IServicesUtils _servicesUtils;

        public SmcModelController(ISmcModelService smcModelService, IServicesUtils servicesUtils)
        {
            _smcModelService = smcModelService;
            _servicesUtils = servicesUtils;
        }

        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return new OkObjectResult(_smcModelService.GetAll());
        }

        [HttpGet("manufacturer/{name}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetByManufacturer([FromRoute] string manufacturerName)
        {
            return new OkObjectResult(_smcModelService.GetByManufacturer(manufacturerName));
        }

        [HttpGet("{name}", Name = "GetByNameSmcModelRoute")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByName([FromRoute] string name)
        {
            SmcModelDto savedSmcModelDto = _smcModelService.GetByName(name);
            if (savedSmcModelDto == null)
            {
                return new NotFoundResult();
            }
            else
            {
                return new OkObjectResult(savedSmcModelDto);
            }
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Save(SmcModelDto smcModelDto)
        {
            SmcModelDto savedSmcModelDto = _smcModelService.Save(smcModelDto);
            return new CreatedResult(
                _servicesUtils
                    .CreateUri(
                        Request,
                        Url.RouteUrl(
                            "GetByNameSmcModelRoute",
                            new {name = savedSmcModelDto.Name})),
                savedSmcModelDto);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(SmcModelDto smcModelDto)
        {
            SmcModelDto updatedSmcModelDto = _smcModelService.Update(smcModelDto);
            return new OkObjectResult(updatedSmcModelDto);
        }

        [HttpDelete("{name}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete([FromRoute] string name)
        {
            _smcModelService.Deactivate(name);
            return new NoContentResult();
        }
    }
}