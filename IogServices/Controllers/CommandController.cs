using System.Linq;
using IogServices.Services;
using IogServices.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace IogServices.Controllers
{
    [Route("commands")]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly ICommandService _commandService;
        private readonly IHubService _hubService;
        private StringValues _token = new StringValues();

        public CommandController(ICommandService commandService, IHubService hubService)
        {
            _commandService = commandService;
            _hubService = hubService;
        }
        
        [HttpPut("relay-on/{serial}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RelayOn([FromRoute] string serial)
        {
            Request.Headers.TryGetValue("Authorization", out _token);
            
            _commandService.RelayOn(serial, Request.Headers["HeaderAuthorization"]);
            
            return new NoContentResult();
        }
        
        [HttpPut("relay-off/{serial}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RelayOff([FromRoute] string serial)
        {
            Request.Headers.TryGetValue("Authorization", out _token);
            _commandService.RelayOff(serial, Request.Headers["HeaderAuthorization"]);
            return new NoContentResult();
        }
        
        [HttpGet("get-relay/{serial}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetRelay([FromRoute] string serial)
        {
            Request.Headers.TryGetValue("Authorization", out _token);
            
            _commandService.GetRelay(serial, _token);
            return new NoContentResult();
        }
    }
}