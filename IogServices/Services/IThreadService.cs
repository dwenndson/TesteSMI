using IogServices.Enums;
using IogServices.Models.DTO;
using IogServices.Threads;
using IogServices.Util;
using CommandTicketDto = NetworkObjects.CommandTicketDto;

namespace IogServices.Services
{
    public interface IThreadService
    {
        void AddTicketToThread(TicketDto ticketDto, ForwarderMessage forwarderMessage, PriorityValue priorityValue,
            CommandDeviceType commandDeviceType);

        void AddTicketToThread(TicketDto ticketDto, ForwarderMessage[] forwarderMessage, PriorityValue priorityValue,
            CommandDeviceType commandDeviceType);
        ITicketThread GetTicketThread(string serial);
        
    }
}