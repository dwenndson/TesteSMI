using System.Net.Mime;
using IogServices.Models.DTO;
using IogServices.Services;
using IogServices.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IogServices.Controllers
{
    [Route("users")]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IServicesUtils _servicesUtils;

        public UserController(IUserService userService, IServicesUtils servicesUtils)
        {
            _userService = userService;
            _servicesUtils = servicesUtils;
        }
        
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return new OkObjectResult(_userService.GetAll());
        }
        
        [HttpGet("{email}", Name = "GetByEmailUserRoute")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByEmail([FromRoute] string email)
        {
            UserDto savedUserDto = _userService.GetByEmail(email);
            if (savedUserDto == null)
            {
                return new NotFoundResult();
            }
            else
            {
                return new OkObjectResult(savedUserDto);
            }
        }
        
        [HttpPost("login")]
        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] UserDto userDto, [FromServices]SigningConfigurations signingConfigurations,
            [FromServices]TokenConfigurations tokenConfigurations)
        {
            object loginObject = _userService.Login(userDto, signingConfigurations, tokenConfigurations);
            return new OkObjectResult(loginObject);
        }
        
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Save(UserDto userDto)
        {
            UserDto savedUserDto = _userService.Save(userDto);
            return new CreatedResult(
                _servicesUtils
                    .CreateUri(
                        Request,
                        Url.RouteUrl(
                            "GetByEmailUserRoute",
                            new {email = savedUserDto.Email})),
                savedUserDto);
        }
        
        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(UserDto userDto)
        {
            UserDto updatedUserDto = _userService.Update(userDto);
            return new OkObjectResult(updatedUserDto);
        }
        
        [HttpDelete("{email}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete([FromRoute] string email)
        {
            _userService.Deactivate(email);
            return new NoContentResult();
        }
    }
}