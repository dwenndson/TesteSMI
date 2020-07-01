using System;
using System.Collections.Generic;
using System.Linq;
using IogServices.Models.DAO;
using Microsoft.EntityFrameworkCore;

namespace IogServices.Repositories.Impl
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IogContext _iogContext;

        public TicketRepository(IogContext iogContext)
        {
            _iogContext = iogContext;
        }

        public Ticket Save(Ticket ticket)
        {
            _iogContext.Tickets.Add(ticket);
            _iogContext.SaveChanges();
            return ticket;
        }

        public Ticket Update(Ticket ticket)
        {
            _iogContext.Update(ticket);
            _iogContext.SaveChanges();
            return ticket;
        }

        public List<Ticket> GetAll()
        {
            return _iogContext.Tickets.Include(t => t.CommandTickets).ToList();
        }

        public List<Ticket> GetBySerial(string serial)
        {
            return _iogContext.Tickets.Include(t => t.CommandTickets).Where(t => t.Serial == serial).OrderByDescending(t => t.InitialDate).ToList();
        }

        public Ticket GetById(Guid Id)
        {
            return _iogContext.Tickets.Include(t => t.CommandTickets).FirstOrDefault(t => t.TicketId == Id);
        }
    }
}