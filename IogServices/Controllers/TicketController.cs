using System;
using IogServices.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IogServices.Controllers
{
    [Route("tickets")]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        
        [HttpGet("{serial}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBySerial([FromRoute] string serial)
        {
            return new OkObjectResult(_ticketService.GetBySerial(serial)); 
        }
        
        [HttpGet("ticket/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByTicketId([FromRoute] Guid id)
        {
            return new OkObjectResult(_ticketService.GetByTicketId(id)); 
        }

    }
}