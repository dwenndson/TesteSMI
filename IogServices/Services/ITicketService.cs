using System;
using System.Collections.Generic;
using IogServices.Enums;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using NetworkObjects.Enum;

namespace IogServices.Services
{
    public interface ITicketService
    {
        TicketDto Create(CommandTicketDto commandTicket, string serial);
        TicketDto Create(string serial, ClientType clientType);
        TicketDto Create(TicketDto ticketDto);
        TicketDto AddCommand(CommandTicketDto commandTicket);
        TicketDto Update(CommandTicketDto commandTicket);
        List<TicketDto> GetBySerial(string serial);
        TicketDto GetByTicketId(Guid id);
        CommandTicketDto GeCommandTicketDtoById(Guid id);
        void CheckCommandSituation(MeterDto meter);
        void RestartCommandFields();

    }
}