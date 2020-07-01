using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IogServices.HubConfig.PayloadHub;
using IogServices.Models.DTO;
using IogServices.Util;

namespace IogServices.Services
{
    public interface IEventHubService
    {
        void ACommandQueueWasUpdatedEvent(object sender, PayloadCommandHub payloadCommandHub);
        void ACommandStatusWasUpdatedEvent(object sender, PayloadCommandHub payloadCommandHub);
        void AComunicationStatusWasUpdatedEvent(object sender, PayloadCommandHub payloadCommandHub);
        void ARelayStateWasChangedEvent(object sender, PayloadCommandHub payloadCommandHub);
        void AAlarmWasGeneratedEvent(object sender, PayloadCommandHub payloadCommandHub);
        void ALogWasGeneratedEvent(object sender, PayloadCommandHub payloadCommandHub);

    }
}
