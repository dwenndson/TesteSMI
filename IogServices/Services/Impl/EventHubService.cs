using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IogServices.Enums;
using IogServices.HubConfig;
using IogServices.HubConfig.PayloadHub;
using IogServices.Models.DTO;
using IogServices.Util;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IogServices.Services.Impl
{
    public class EventHubService : IEventHubService
    {
        private IHubService _hubService;
        public EventHubService(IHubService hubService)
        {
            _hubService = hubService;
        }
        
        public void ACommandQueueWasUpdatedEvent(object sender, PayloadCommandHub payloadCommandHub)
        {
            _hubService.GeneralSendUpdate("UpdateForAll",new CommandHub(CommandTypeHub.ACommandQueueWasUpdatedEvent,payloadCommandHub));
        }

        public void ACommandStatusWasUpdatedEvent(object sender, PayloadCommandHub payloadCommandHub)
        {
            _hubService.GeneralSendUpdate("UpdateForAll", new CommandHub(CommandTypeHub.ACommandStatusWasUpdatedEvent, payloadCommandHub));
        }

        public void AComunicationStatusWasUpdatedEvent(object sender, PayloadCommandHub payloadCommandHub)
        {
            _hubService.GeneralSendUpdate("UpdateForAll", new CommandHub(CommandTypeHub.AComunicationStatusWasUpdatedEvent, payloadCommandHub));
        }

        public void ARelayStateWasChangedEvent(object sender, PayloadCommandHub payloadCommandHub)
        {
            _hubService.GeneralSendUpdate("UpdateForAll", new CommandHub(CommandTypeHub.ARelayStateWasChangedEvent, payloadCommandHub));
        }

        public void AAlarmWasGeneratedEvent(object sender, PayloadCommandHub payloadCommandHub)
        {
            _hubService.GeneralSendUpdate("UpdateForAll", new CommandHub(CommandTypeHub.AAlarmWasGeneratedEvent, payloadCommandHub));
        }

        public void ALogWasGeneratedEvent(object sender, PayloadCommandHub payloadCommandHub)
        {
            _hubService.GeneralSendUpdate("UpdateForAll", new CommandHub(CommandTypeHub.ALogWasGeneratedEvent, payloadCommandHub));
        }

    }
}
