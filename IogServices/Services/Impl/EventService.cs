using System;
using IogServices.Models.DTO;
using IogServices.Threads;
using IogServices.Util;

namespace IogServices.Services.Impl
{
    public class EventService : IEventService
    {
        public void ASmcWasSavedEvent(object sender, IoGServicedEventArgs<SmcDto> smc)
        {
            ASmcWasSaved?.Invoke(sender, smc);
        }

        public void AMeterWasSavedEvent(object sender, IoGServicedEventArgs<MeterDto> meter)
        {
            AMeterWasSaved?.Invoke(sender, meter);
        }

        public void AThreadIsShuttingDownEvent(object sender, IoGServicedEventArgs<ITicketThread> ticketThread)
        {
            AThreadIsShuttingDown?.Invoke(sender, ticketThread);
        }

        public event EventHandler<IoGServicedEventArgs<SmcDto>> ASmcWasSaved;
        public event EventHandler<IoGServicedEventArgs<MeterDto>> AMeterWasSaved;
        public event EventHandler<IoGServicedEventArgs<ITicketThread>> AThreadIsShuttingDown;
    }
}