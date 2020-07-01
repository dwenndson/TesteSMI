using System;
using System.Linq;
using IogServices.Models.DAO;
using Microsoft.EntityFrameworkCore;

namespace IogServices.Repositories.Impl
{
    public class CommandTicketRepository : ICommandTicketRepository
    {
        private readonly IogContext _iogContext;

        public CommandTicketRepository(IogContext iogContext)
        {
            _iogContext = iogContext;
        }
        
        public CommandTicket GetById(Guid id)
        {
            return _iogContext.CommandTickets.Include(c => c.Ticket).FirstOrDefault(c => c.CommandId == id);
        }
    }
}