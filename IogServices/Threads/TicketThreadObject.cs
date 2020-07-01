using System;
using System.Collections.Generic;
using IogServices.Enums;
using IogServices.Models.DTO;
using IogServices.Util;

namespace IogServices.Threads
{
    public class TicketThreadObject : IComparable
    {
        public TicketDto Ticket { get; set; }
        private PriorityValue PriorityValue { get; set; }
        public List<ForwarderMessage> ForwarderMessages { get; set; } = new List<ForwarderMessage>();

        public TicketThreadObject(TicketDto ticket, PriorityValue priorityValue)
        {
            Ticket = ticket;
            PriorityValue = priorityValue;
        }

        public TicketThreadObject(TicketDto ticket, PriorityValue priorityValue, ForwarderMessage forwarderMessage)
        {
            Ticket = ticket;
            PriorityValue = priorityValue;
            ForwarderMessages.Add(forwarderMessage);
        }

        public TicketThreadObject(TicketDto ticket, PriorityValue priorityValue,
            IEnumerable<ForwarderMessage> forwarderMessages)
        {
            Ticket = ticket;
            PriorityValue = priorityValue;
            ForwarderMessages.AddRange(forwarderMessages);
        }
        
        
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (!(obj is TicketThreadObject otherTicket))
                throw new ArgumentException("Objeto não é um ticket");
            
            return PriorityValue.CompareTo(otherTicket.PriorityValue);
        }
    }
}