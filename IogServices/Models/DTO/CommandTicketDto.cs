using System;
using IogServices.Models.DTO.Generic;
using NetworkObjects.Enum;

namespace IogServices.Models.DTO
{
    public class CommandTicketDto : BaseDto
    {
        public Guid CommandId { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int Attempt { get; set; }
        public string Pdu { get; set; }
        public Status Status { get; set; }
        public StatusCommand StatusCommand { get; set; }
        public string Answer { get; set; } 
        public Command Command { get; set; }
        public CommunicationStatus CommunicationStatus { get; set; }
    }
}