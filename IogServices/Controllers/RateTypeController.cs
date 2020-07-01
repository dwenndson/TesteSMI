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
    [Route("rate-types")]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class RateTypeController : ControllerBase
    {
        private readonly IRateTypeService _rateTypeService;
        private readonly IServicesUtils _servicesUtils;

        public RateTypeController(IRateTypeService rateTypeService, IServicesUtils servicesUtils)
        {
            _rateTypeService = rateTypeService;
            _servicesUtils = servicesUtils;
        }

        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return new OkObjectResult(_rateTypeService.GetAll());
        }

        [HttpGet("{name}", Name = "GetByNameRateTypeRoute")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById([FromRoute] string name)
        {
            RateTypeDto savedRateTypeDto = _rateTypeService.GetByName(name);
            if (savedRateTypeDto == null)
            {
                return new NotFoundResult();
            }
            else
            {
                return new OkObjectResult(savedRateTypeDto);
            }
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Save(RateTypeDto rateTypeDto)
        {
            RateTypeDto savedRateTypeDto = _rateTypeService.Save(rateTypeDto);
            return new CreatedResult(
                _servicesUtils
                    .CreateUri(
                        Request,
                        Url.RouteUrl(
                            "GetByNameRateTypeRoute",
                            new {name = savedRateTypeDto.Name})),
                savedRateTypeDto);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(RateTypeDto rateTypeDto)
        {
            RateTypeDto updatedRateTypeDto = _rateTypeService.Update(rateTypeDto);
            return new OkObjectResult(updatedRateTypeDto);
        }

        [HttpDelete("{name}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete([FromRoute] string name)
        {
            _rateTypeService.Deactivate(name);
            return new NoContentResult();
        }
    }
}