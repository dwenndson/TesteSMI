using IogServices.Models.DTO;
using IogServices.Util;

namespace IogServices.Services
{
    public interface IMeterMiddlewareService : IMiddlewareService
    {
        ForwarderMessage MakeMeterCreationPackageForForwarder(MeterDto meterDto);
        ForwarderMessage MakeMeterEditionPackageForForwarder(MeterDto meterDto);
        ForwarderMessage MakeMeterDeletionPackageForForwarder(MeterDto meterDto);
        ForwarderMessage MakeMeterRelayOnCommandForForwarder(MeterDto meterDto, CommandTicketDto commandTicketDto);
        ForwarderMessage MakeMeterRelayOffCommandForForwarder(MeterDto meterDto, CommandTicketDto commandTicketDto);
        ForwarderMessage MakeMeterRelayStatusCommandForForwarder(MeterDto meterDto, CommandTicketDto commandTicketDto);
    }
}