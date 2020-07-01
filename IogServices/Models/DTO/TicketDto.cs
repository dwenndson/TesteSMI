using System;
using System.Collections.Generic;
using IogServices.Enums;
using IogServices.Models.DAO;
using NetworkObjects.Enum;

namespace IogServices.Models.DTO
{
    public class TicketDto
    {
        public string Serial { get; set; }
        public Guid TicketId { get; set; }

        public CommandType CommandType { get; set; }
        public string User { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinishDate { get; set; }
        public Status Status { get; set; }
        public StatusCommand StatusCommand { get; set; }
        public int Interval { get; set; }
        public List<CommandTicketDto> CommandTickets { get; set; }
        public TicketStatus TicketStatus { get; set; }

        public TicketDto()
        {
            CommandTickets = new List<CommandTicketDto>();
        }
    }
}