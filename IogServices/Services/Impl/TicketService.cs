using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using IogServices.Enums;
using IogServices.ExceptionHandlers.Exceptions;
using IogServices.HubConfig.PayloadHub;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using IogServices.Repositories;
using NetworkObjects.Enum;
using CommunicationStatus = NetworkObjects.Enum.CommunicationStatus;

namespace IogServices.Services.Impl
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ICommandTicketRepository _commandTicketRepository;
        private readonly IMapper _mapper;
        private readonly IEventHubService _eventHubService;

        public TicketService(ITicketRepository ticketRepository, IMapper mapper, ICommandTicketRepository commandTicketRepository,
            IEventHubService eventHubService)
        {
            _ticketRepository = ticketRepository;
            _commandTicketRepository = commandTicketRepository;
            _mapper = mapper;
            _eventHubService = eventHubService;
        }
        public TicketDto Create(CommandTicketDto commandTicket, string serial)
        {
            var ticket = new TicketDto
            {
                User = "system", Serial = serial, TicketId = commandTicket.CommandId,
                InitialDate = commandTicket.InitialDate, TicketStatus = TicketStatus.Executing
            };
            ticket.CommandTickets.Add(commandTicket);
            return _mapper.Map<Ticket, TicketDto>(_ticketRepository.Save(_mapper.Map<TicketDto, Ticket>(ticket)));;
        }

        public TicketDto Create(string serial, ClientType clientType)
        {
            var newTicket = new TicketDto
            {
                User = clientType.ToString(),
                Serial = serial,
                TicketId = Guid.NewGuid()
            };

            return _mapper.Map<Ticket, TicketDto>(_ticketRepository.Save(_mapper.Map<TicketDto, Ticket>(newTicket)));
        }

        public TicketDto Create(TicketDto ticketDto)
        {
            return _mapper.Map<Ticket, TicketDto>(_ticketRepository.Save(_mapper.Map<TicketDto, Ticket>(ticketDto)));;
        }

        public TicketDto AddCommand(CommandTicketDto commandTicket)
        {
            var ticket = GetByTicketId(commandTicket.CommandId);
            if (ticket == null) throw new BadRequestException("Ticket não cadastrado");
            ticket.CommandTickets.Add(commandTicket);
            //Init websocket
            _eventHubService.ACommandQueueWasUpdatedEvent(this, new PayloadUpdateCommandQueue(ticket.Serial, 1, commandTicket.UpdatedAt));
            _eventHubService.ACommandStatusWasUpdatedEvent(this, new PayloadUpdateCommandStatus(ticket.Serial, commandTicket.Command, commandTicket.UpdatedAt.ToString(), commandTicket.StatusCommand.ToString()));
            _eventHubService.AComunicationStatusWasUpdatedEvent(this, new PayloadUpdateComunicationStatus(ticket.Serial, commandTicket.CommunicationStatus, commandTicket.UpdatedAt.ToString()));
            //end Websocket
            return _mapper.Map<Ticket, TicketDto>(_ticketRepository.Update(_mapper.Map<TicketDto, Ticket>(ticket)));;
        }

        public TicketDto Update(CommandTicketDto commandTicket)
        {
            var command = _commandTicketRepository.GetById(commandTicket.CommandId);
            if (command == null) throw new BadRequestException("Comando não cadastrado");
            var ticket = _ticketRepository.GetById(command.Ticket.TicketId);
            var commandT = ticket.CommandTickets.Find(c => c.CommandId == commandTicket.CommandId);
            commandT.UpdateValues(_mapper.Map<CommandTicketDto, CommandTicket>(commandTicket));
            ticket.UpdateValues(command);
            //Init websocket
            if (commandT.Status == Status.Finished)
            {
                _eventHubService.ACommandQueueWasUpdatedEvent(this,
                    new PayloadUpdateCommandQueue(ticket.Serial, -1, command.UpdatedAt.ToString()));
            }

            _eventHubService.ACommandStatusWasUpdatedEvent(this,
                new PayloadUpdateCommandStatus(ticket.Serial, command.Command, command.UpdatedAt.ToString(),
                    command.StatusCommand.ToString()));

            _eventHubService.AComunicationStatusWasUpdatedEvent(this,
                 new PayloadUpdateComunicationStatus(ticket.Serial, commandTicket.CommunicationStatus, command.UpdatedAt.ToString()));
            //end websocket
            return _mapper.Map<Ticket, TicketDto>(_ticketRepository.Update(ticket));
        }

        public List<TicketDto> GetBySerial(string serial)
        {
            return _mapper.Map<List<TicketDto>>(_ticketRepository.GetBySerial(serial));
        }

        public TicketDto GetByTicketId(Guid id)
        {
            return _mapper.Map<Ticket, TicketDto>(_ticketRepository.GetById(id));
        }

        public CommandTicketDto GeCommandTicketDtoById(Guid id)
        {
            return _mapper.Map<CommandTicket, CommandTicketDto>(_commandTicketRepository.GetById(id));
        }

        public void CheckCommandSituation(MeterDto meter)
        {
            var tickets = GetBySerial(meter.Smc == null ? meter.Serial : meter.Smc.Serial);
            if (tickets == null) return;
            meter.CommandQueue = tickets.Sum(ticket =>
                ticket.CommandTickets.Count(command =>
                    command.Status == Status.Waiting || command.Status == Status.Executing));


            var ticketBeingExecuted = tickets.Find(ticket => ticket.Status == Status.Executing);
            var commandBeingExecuted =
                ticketBeingExecuted?.CommandTickets.Find(command => command.Status == Status.Executing);
            if (commandBeingExecuted != null)
            {
                meter.Command = commandBeingExecuted.Command;
            }

            var allCommands = tickets.SelectMany(ticket => ticket.CommandTickets).ToList();
            if (allCommands.Count != 0)
            {
                meter.CommunicationStatus =
                    allCommands.OrderBy(command => command.InitialDate).Last().CommunicationStatus;
            }
        }

        public void RestartCommandFields()
        {
            // var tickets = _ticketRepository.GetAll().Where(ticket => ticket.Status == Status.Executing);
            // var enumerable = tickets as Ticket[] ?? tickets.ToArray();
            // if (!enumerable.Any()) return;
            // {
            //     foreach (var ticket in enumerable)
            //     {
            //         var commandTicket = ticket.CommandTickets.First(command => command.Status == Status.Executing);
            //         if(commandTicket == null) continue;
            //         var commandInterrupted = _mapper.Map<CommandTicket, CommandTicketDto>(commandTicket);
            //         commandInterrupted.StatusCommand = StatusCommand.INCOMPLETE;
            //         commandInterrupted.Command = Command.WAITING_COMMAND;
            //         commandInterrupted.CommunicationStatus = CommunicationStatus.NO_INFORMATION;
            //         commandInterrupted.Status = Status.Failed;
            //         Update(commandInterrupted);
            //     }
            // }

            // var tickets = _ticketRepository.GetAll().Where(ticket => ticket.Status == Status.Executing);
            // if (!tickets.Any())
            //     return;
            // foreach (var ticket in tickets)
            // {
            //     var commandInterrupted = _mapper.Map<CommandTicket, CommandTicketDto>(ticket.CommandTickets.FirstOrDefault(command => command.Status == Status.Executing));
            //     if (commandInterrupted == null)
            //         continue;
            //     commandInterrupted.StatusCommand = StatusCommand.INCOMPLETE;
            //     commandInterrupted.Command = Command.WAITING_COMMAND;
            //     commandInterrupted.CommunicationStatus = CommunicationStatus.NO_INFORMATION;
            //     commandInterrupted.Status = Status.Failed;
            //     Update(commandInterrupted);
            // }
        }
    }
}