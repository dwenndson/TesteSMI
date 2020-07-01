using System;
using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using IogServices.Enums;
using IogServices.ExceptionHandlers.Exceptions;
using IogServices.HubConfig.PayloadHub;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using IogServices.Repositories;
using IogServices.Util;
using NetworkObjects.Enum;
using Command = NetworkObjects.Enum.Command;
using CommunicationStatus = NetworkObjects.Enum.CommunicationStatus;

namespace IogServices.Services.Impl
{
    public class CommandService : ICommandService
    {
        private readonly IMeterService _meterService;
        private readonly IServicesUtils _servicesUtils;
        private readonly IMiddlewareProviderService _middlewareProviderService;
        private readonly IThreadService _threadService;
        private readonly ITicketService _ticketService;
        private readonly IMapper _mapper;
        private readonly IEventHubService _eventHubService;

        public CommandService(IMeterService meterService, IServicesUtils servicesUtils,
            IMiddlewareProviderService middlewareProviderService, IMapper mapper, IThreadService threadService,
            ITicketService ticketService, IEventHubService eventHubService)
        {
            _meterService = meterService;
            _servicesUtils = servicesUtils;
            _middlewareProviderService = middlewareProviderService;
            _ticketService = ticketService;
            _mapper = mapper;
            _threadService = threadService;
            _eventHubService = eventHubService;
        }

        public void RelayOn(string serial, string token)
        {
            var meter = _meterService.GetExistingMeter(serial);
            var meterDto = _mapper.Map<Meter, MeterDto>(meter);
            var client = token != null ? _servicesUtils.GetCurrentClientType(token) : ClientType.ALL;
            if (meter.Modem == null) throw new BadRequestException("O medidor informado não possui modem");
            var relayOnCommandTicket = new CommandTicketDto
            {
                CommandId = Guid.NewGuid(),
                CommunicationStatus = CommunicationStatus.NO_INFORMATION,
                Attempt = 0,
                Status = Status.Waiting
            };
            var relayStatusCommandTicket = new CommandTicketDto
            {
                CommandId = Guid.NewGuid(),
                CommunicationStatus = CommunicationStatus.NO_INFORMATION,
                Attempt = 0,
                Status = Status.Waiting
            };
            var ticket = new TicketDto
            {
                User = client.ToString(),
                Serial = meter.Smc == null ? meter.Serial : meter.Smc.Serial,
                TicketId = Guid.NewGuid(),
                CommandType = CommandType.RelayOn,
                Interval = 18,
                TicketStatus = TicketStatus.Waiting
            };
            ForwarderMessage relayOnForwarderMessage;
            ForwarderMessage relayStatusForwarderMessage;
            CommandDeviceType commandDeviceType;
            if (meter.Smc != null)
            {
                var smcMiddlewareService =
                    _middlewareProviderService.GetSmcMiddlewareServiceByManufacturerName(meter.MeterModel.Manufacturer
                        .Name);
                relayOnForwarderMessage =
                    smcMiddlewareService.MakeMeterWithSmcRelayOnCommandForForwarder(meterDto, relayOnCommandTicket);
                relayStatusForwarderMessage =
                    smcMiddlewareService.MakeMeterWithSmcRelayStatusCommandForForwarder(meterDto,
                        relayStatusCommandTicket);
                commandDeviceType = CommandDeviceType.Smc;
            }
            else
            {
                var meterMiddlewareService =
                    _middlewareProviderService.GetMeterMiddlewareServiceByManufacturerName(meter.MeterModel.Manufacturer
                        .Name);
                relayOnForwarderMessage =
                    meterMiddlewareService.MakeMeterRelayOnCommandForForwarder(meterDto, relayOnCommandTicket);
                relayStatusForwarderMessage =
                    meterMiddlewareService.MakeMeterRelayStatusCommandForForwarder(meterDto, relayStatusCommandTicket);
                commandDeviceType = CommandDeviceType.Meter;
            }
            var forwarderMessageArray = new[] {relayOnForwarderMessage, relayStatusForwarderMessage};
            ticket.CommandTickets.Add(relayOnCommandTicket);
            ticket.CommandTickets.Add(relayStatusCommandTicket);
            var ticketSaved = _ticketService.Create(ticket);
            _threadService.AddTicketToThread(ticketSaved, forwarderMessageArray, PriorityValue.MediumPriority,
                commandDeviceType);
            // Init websocket
            if (ticketSaved != null)
            {
                _eventHubService.ACommandQueueWasUpdatedEvent(this, new PayloadUpdateCommandQueue(ticket.Serial, 2, DateTime.Now.ToString()));
                _eventHubService.ACommandStatusWasUpdatedEvent(this, new PayloadUpdateCommandStatus(ticket.Serial, ticket.CommandTickets[0].Command, DateTime.Now.ToString().ToString(), ticket.CommandTickets[0].StatusCommand.ToString()));
                _eventHubService.AComunicationStatusWasUpdatedEvent(this, new PayloadUpdateComunicationStatus(ticket.Serial, ticket.CommandTickets[0].CommunicationStatus, DateTime.Now.ToString()));
                _eventHubService.ARelayStateWasChangedEvent(this, new PayloadUpdateRelayState(meterDto.Serial, meterDto.UpdatedAt, meterDto.AccountantStatus));
            }
            // fim websocket
            // _threadService.AddCommand(command);
        }

        public void RelayOff(string serial, string token)
        {
            var meter = _meterService.GetExistingMeter(serial);
            var meterDto = _mapper.Map<Meter, MeterDto>(meter);
            var client = token != null ? _servicesUtils.GetCurrentClientType(token) : ClientType.ALL;
            if (meter.Modem == null) throw new BadRequestException("O medidor informado não possui modem");
            var relayOffCommandTicket = new CommandTicketDto
            {
                CommandId = Guid.NewGuid(),
                CommunicationStatus = CommunicationStatus.NO_INFORMATION,
                Attempt = 0,
                Status = Status.Waiting,
            };
            var relayStatusCommandTicket = new CommandTicketDto
            {
                CommandId = Guid.NewGuid(),
                CommunicationStatus = CommunicationStatus.NO_INFORMATION,
                Attempt = 0,
                Status = Status.Waiting
            };
            var ticket = new TicketDto
            {
                User = client.ToString(),
                Serial = meter.Smc == null ? meter.Serial : meter.Smc.Serial,
                TicketId = Guid.NewGuid(),
                CommandType = CommandType.RelayOff,
                Interval = 18,
                TicketStatus = TicketStatus.Waiting
            };
            ForwarderMessage relayOffForwarderMessage;
            ForwarderMessage relayStatusForwarderMessage;
            CommandDeviceType commandDeviceType;
            if (meter.Smc != null)
            {
                var smcMiddlewareService =
                    _middlewareProviderService.GetSmcMiddlewareServiceByManufacturerName(meter.MeterModel.Manufacturer
                        .Name);
                relayOffForwarderMessage =
                    smcMiddlewareService.MakeMeterWithSmcRelayOffCommandForForwarder(meterDto, relayOffCommandTicket);
                relayStatusForwarderMessage =
                    smcMiddlewareService.MakeMeterWithSmcRelayStatusCommandForForwarder(meterDto, relayStatusCommandTicket);
                commandDeviceType = CommandDeviceType.Smc;
            }
            else
            {
                var meterMiddlewareService =
                    _middlewareProviderService.GetMeterMiddlewareServiceByManufacturerName(meter.MeterModel.Manufacturer
                        .Name); 
                relayOffForwarderMessage =
                    meterMiddlewareService.MakeMeterRelayOffCommandForForwarder(meterDto, relayOffCommandTicket);
                relayStatusForwarderMessage =
                    meterMiddlewareService.MakeMeterRelayStatusCommandForForwarder(meterDto, relayStatusCommandTicket);
                commandDeviceType = CommandDeviceType.Meter;
            }
            var forwarderMessageArray = new[] {relayOffForwarderMessage, relayStatusForwarderMessage};
            ticket.CommandTickets.Add(relayOffCommandTicket);
            ticket.CommandTickets.Add(relayStatusCommandTicket);
            var ticketSaved = _ticketService.Create(ticket);
            _threadService.AddTicketToThread(ticketSaved, forwarderMessageArray, PriorityValue.MediumPriority,
                commandDeviceType);
            // _threadService.AddCommand(command);
            // Init websocket
            _eventHubService.ACommandQueueWasUpdatedEvent(this, new PayloadUpdateCommandQueue(ticket.Serial, 2, DateTime.Now.ToString()));
            _eventHubService.ACommandStatusWasUpdatedEvent(this, new PayloadUpdateCommandStatus(ticket.Serial, ticket.CommandTickets[0].Command, DateTime.Now.ToString().ToString(), ticket.CommandTickets[0].StatusCommand.ToString()));
            _eventHubService.AComunicationStatusWasUpdatedEvent(this, new PayloadUpdateComunicationStatus(ticket.Serial, ticket.CommandTickets[0].CommunicationStatus, DateTime.Now.ToString()));
            _eventHubService.ARelayStateWasChangedEvent(this, new PayloadUpdateRelayState(meterDto.Serial, meterDto.UpdatedAt, meterDto.AccountantStatus));
            // fim websocket
        }

        public void GetRelay(string serial, string token)
        {
            var meter = _meterService.GetExistingMeter(serial);
            var meterDto = _mapper.Map<Meter, MeterDto>(meter);
            var client = token != null ? _servicesUtils.GetCurrentClientType(token) : ClientType.ALL;
            if (meter.Modem == null) throw new BadRequestException("O medidor informado não possui modem");
            var commandTicket = new CommandTicketDto
            {
                CommandId = Guid.NewGuid(),
                CommunicationStatus = CommunicationStatus.NO_INFORMATION,
                Attempt = 0,
                Status = Status.Waiting,
            };
            var ticket = new TicketDto
            {
                User = client.ToString(),
                Serial = meter.Smc == null ? meter.Serial : meter.Smc.Serial,
                TicketId = Guid.NewGuid(),
                CommandType = CommandType.RelayStatus,
                Interval = 18,
                TicketStatus = TicketStatus.Waiting
            };
            ForwarderMessage relayStatusForwarderMessage;
            CommandDeviceType commandDeviceType;
            if (meter.Smc != null)
            {
                var smcMiddlewareService =
                    _middlewareProviderService.GetSmcMiddlewareServiceByManufacturerName(meter.MeterModel.Manufacturer
                        .Name);
                relayStatusForwarderMessage =
                    smcMiddlewareService.MakeMeterWithSmcRelayStatusCommandForForwarder(meterDto, commandTicket);
                commandDeviceType = CommandDeviceType.Smc;
            }
            else
            {
                var meterMiddlewareService =
                    _middlewareProviderService.GetMeterMiddlewareServiceByManufacturerName(meter.MeterModel.Manufacturer
                        .Name);
                relayStatusForwarderMessage =
                    meterMiddlewareService.MakeMeterRelayStatusCommandForForwarder(meterDto, commandTicket);
                commandDeviceType = CommandDeviceType.Meter;
            }
            ticket.CommandTickets.Add(commandTicket);
            var ticketSaved = _ticketService.Create(ticket);
            _threadService.AddTicketToThread(ticketSaved, relayStatusForwarderMessage, PriorityValue.MediumPriority,
                commandDeviceType);
            _eventHubService.ACommandQueueWasUpdatedEvent(this, new PayloadUpdateCommandQueue(ticket.Serial, 1, DateTime.Now.ToString()));
            _eventHubService.ACommandStatusWasUpdatedEvent(this, new PayloadUpdateCommandStatus(ticket.Serial, ticket.CommandTickets[0].Command, DateTime.Now.ToString().ToString(), ticket.CommandTickets[0].StatusCommand.ToString()));
            _eventHubService.AComunicationStatusWasUpdatedEvent(this, new PayloadUpdateComunicationStatus(ticket.Serial, ticket.CommandTickets[0].CommunicationStatus, DateTime.Now.ToString()));
            _eventHubService.ARelayStateWasChangedEvent(this, new PayloadUpdateRelayState(meterDto.Serial, meterDto.UpdatedAt, meterDto.AccountantStatus));
        }
    }
}