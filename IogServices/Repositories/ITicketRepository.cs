using System;
using System.Collections.Generic;
using IogServices.Models.DAO;
using IogServices.Models.DTO;

namespace IogServices.Repositories
{
    public interface ITicketRepository
    {
        Ticket Save(Ticket ticket);
        Ticket Update(Ticket ticket);
        List<Ticket> GetAll();
        List<Ticket> GetBySerial(string serial);
        Ticket GetById(Guid Id);
    }
}