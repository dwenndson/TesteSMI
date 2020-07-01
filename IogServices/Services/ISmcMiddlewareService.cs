using IogServices.Models.DTO;
using IogServices.Util;

namespace IogServices.Services
{
    public interface ISmcMiddlewareService : IMiddlewareService
    {
        ForwarderMessage MakeSmcCreationPackageForForwarder(SmcDto smcDto);
        ForwarderMessage MakeSmcEditionPackageForForwarder(SmcDto smcDto);
        ForwarderMessage MakeSmcDeletionPackageForForwarder(SmcDto smcDto);
        
        ForwarderMessage MakeMeterWithSmcRelayOnCommandForForwarder(MeterDto meterDto,
            CommandTicketDto commandTicketDto);

        ForwarderMessage MakeMeterWithSmcRelayOffCommandForForwarder(MeterDto meterDto,
            CommandTicketDto commandTicketDto);

        ForwarderMessage MakeMeterWithSmcRelayStatusCommandForForwarder(MeterDto meterDto,
            CommandTicketDto commandTicketDto);
    }
}