using System;
using IogServices.Models.DAO;

namespace IogServices.Repositories
{
    public interface ICommandTicketRepository
    {
        CommandTicket GetById(Guid id);
    }
}