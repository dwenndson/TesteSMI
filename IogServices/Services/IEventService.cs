using System;
using IogServices.Models.DTO;
using IogServices.Threads;
using IogServices.Util;

namespace IogServices.Services
{
    public interface IEventService
    {
        void ASmcWasSavedEvent(object sender, IoGServicedEventArgs<SmcDto> smc);
        void AMeterWasSavedEvent(object sender, IoGServicedEventArgs<MeterDto> meter);
        void AThreadIsShuttingDownEvent(object sender, IoGServicedEventArgs<ITicketThread> ticketThread);
        
        event EventHandler<IoGServicedEventArgs<SmcDto>> ASmcWasSaved;
        event EventHandler<IoGServicedEventArgs<MeterDto>> AMeterWasSaved;
        event EventHandler<IoGServicedEventArgs<ITicketThread>> AThreadIsShuttingDown;
    }
}