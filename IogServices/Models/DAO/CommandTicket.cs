using System;
using IogServices.Models.DAO.Generic;
using NetworkObjects.Enum;

namespace IogServices.Models.DAO
{
    public class CommandTicket : Base
    {
        public Guid CommandId { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int Attempt { get; set; }
        public string Pdu { get; set; }
        public Status Status { get; set; }
        public StatusCommand StatusCommand { get; set; }
        public string Answer { get; set; } 
        public Ticket Ticket { get; set; }
        public CommunicationStatus CommunicationStatus { get; set; }
        public Command Command { get; set; }
        public void UpdateValues(CommandTicket commandTicket)
        {
            InitialDate = commandTicket.InitialDate;
            FinishDate = commandTicket.FinishDate;
            Attempt = commandTicket.Attempt;
            Pdu = commandTicket.Pdu;
            Status = commandTicket.Status;
            StatusCommand = commandTicket.StatusCommand;
            Answer = commandTicket.Answer;
            CommunicationStatus = commandTicket.CommunicationStatus;
            Command = commandTicket.Command;
        }
    }
}