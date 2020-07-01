using System;
using System.Collections.Generic;
using IogServices.Enums;
using IogServices.Models.DAO.Generic;
using NetworkObjects.Enum;

namespace IogServices.Models.DAO
{
    public class Ticket : Base
    {
        public Guid TicketId { get; set; }
        public string Serial { get; set; }
        public CommandType CommandType { get; set; }
        public string User { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinishDate { get; set; }
        public Status Status { get; set; }
        public StatusCommand StatusCommand { get; set; }
        public int Interval { get; set; }
        public List<CommandTicket> CommandTickets { get; set; }
        public TicketStatus TicketStatus { get; set; }

        public void UpdateValues(CommandTicket commandTicket)
        {
            Status = commandTicket.Status;
            StatusCommand = commandTicket.StatusCommand;
            FinishDate = commandTicket.FinishDate;
            if (CommandTickets.TrueForAll(command => command.Status == Status.Finished))
            {
                TicketStatus = TicketStatus.Finished;
            }
            else if (CommandTickets.TrueForAll(command =>
                command.Status == Status.Finished || command.Status == Status.Failed))
            {
                TicketStatus = TicketStatus.PartiallyFinished;
            }
            else if(TicketStatus == TicketStatus.Waiting && commandTicket.Status == Status.Executing)
            {
                TicketStatus = TicketStatus.Executing;
            }
        }
    }
}