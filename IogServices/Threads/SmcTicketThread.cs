using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DataStructures;
using IogServices.Constants;
using IogServices.Enums;
using IogServices.Models.DTO;
using IogServices.Services;
using IogServices.Util;
using MqttClientLibrary;
using MqttClientLibrary.Models;
using NetworkObjects.Enum;
using Newtonsoft.Json;
using Command = NetworkObjects.Enum.Command;
using CommunicationStatus = NetworkObjects.Enum.CommunicationStatus;

namespace IogServices.Threads
{
    public class SmcTicketThread : ITicketThread
    {
        public SmcDto Smc { get; set; }
        private ConcurrentPriorityQueue<TicketThreadObject> Tickets { get;}
        private TicketThreadObject CurrentTicket { get; set; }
        private CommandTicketDto CurrentCommand { get; set; }
        public Command Command { get; set; }
        public CommunicationStatus CommunicationStatus { get; set; }
        public Status Status { get; set; }
        private readonly SemaphoreSlim _executingCommandSemaphore;
        private readonly SemaphoreSlim _waitingCommandMessageSemaphore;
        private readonly SemaphoreSlim _answerSemaphore;
        private bool _isExecutingCommand = false;
        private bool _isWaitingCommandMessage = false;
        private readonly IoGMqttClient _mqttClient;
        private readonly CommandRules _commandRules;
        private readonly List<TicketThreadObject> _ticketsExecuted = new List<TicketThreadObject>();
        private readonly IEventService _eventService;
        private readonly IForwarderSenderService _forwarderSenderService;

        public SmcTicketThread(SmcDto smcDto, ClientSettings clientSettings,
            BrokerSettings brokerSettings, IMqttClientConfiguration mqttClientConfiguration,
            IMqttClientMethods mqttClientMethods, Topic topic,
            CommandRules commandRules, IEventService eventService, IForwarderSenderService forwarderSenderService)
        {
            Smc = smcDto;
            _eventService = eventService;
            _forwarderSenderService = forwarderSenderService;
            Tickets = new ConcurrentPriorityQueue<TicketThreadObject>();
            _executingCommandSemaphore = new SemaphoreSlim(1, 1);
            _waitingCommandMessageSemaphore = new SemaphoreSlim(1, 1);
            _answerSemaphore = new SemaphoreSlim(1, 1);

            var localClientSettings = new ClientSettings
            {
                ClientId = clientSettings.ClientId, ClientName = clientSettings.ClientName
            };
            localClientSettings.ClientId = localClientSettings.ClientId.Replace("serial", smcDto.Serial);
            localClientSettings.ClientName = localClientSettings.ClientName.Replace("serial", smcDto.Serial);
            localClientSettings.DebugMode = clientSettings.DebugMode;
            localClientSettings.AutoReconnectDelayInSeconds = clientSettings.AutoReconnectDelayInSeconds;
            var localTopic = new Topic {Address = topic.Address, QoS = topic.QoS};
            localTopic.Address = localTopic.Address
                .Replace("{smc-or-meter}", CommandDeviceType.Smc.ToString().ToLower())
                .Replace("{serial}", smcDto.Serial);
            _mqttClient = new IoGMqttClient(localClientSettings, brokerSettings, mqttClientConfiguration,
                mqttClientMethods);
            _mqttClient.Subscribe(localTopic);
            _mqttClient.MessageReceivedHandler += ReceivedUpdate;
            _commandRules = commandRules;
        }

        public void Run()
        {
            try
            {
                do
                {
                    SetExecutingCommand(true);
                    CurrentTicket = Tickets.Take();
                    Console.WriteLine("TICKET ATUAL SERIAL " + CurrentTicket.Ticket.Serial + " COMANDO " + CurrentTicket.Ticket.CommandType);
                    foreach (var command in CurrentTicket.Ticket.CommandTickets)
                    {
                        CurrentCommand = command;
                        Console.WriteLine("COMANDO ATUAL " + command.CommandId);
                        CommunicationStatus = command.CommunicationStatus;
                        Status = command.Status;
                        var forwarderMessage =
                            CurrentTicket.ForwarderMessages.Find(message => message.CommandId == command.CommandId);
                        var tries = 0;
                        var stillWaitingForResponse = false;
                        do
                        {
                            tries++;
                            Console.WriteLine("TENTATIVA " + tries);
                            var sent = SendCommand(forwarderMessage);
                            if (sent.IsSuccessStatusCode)
                            {
                                SetWaitingCommandMessage(true);
                                Console.WriteLine("AGUARDANDO RESPOSTA");
                                stillWaitingForResponse = WaitConfirmation();
                                if (!stillWaitingForResponse)
                                {
                                    break;
                                }

                                SetWaitingCommandMessage(false);
                            }
                            else
                            {
                                CurrentTicket.Ticket.TicketStatus =
                                    CurrentTicket.Ticket.TicketStatus == TicketStatus.Waiting
                                        ? TicketStatus.FailedToStart
                                        : TicketStatus.FailedToContinue;
                                break;
                            }

                            Task.Delay(TimeSpan.FromSeconds(_commandRules.IntervalBetweenCommandTriesInSeconds));
                        } while (tries < _commandRules.NumberOfCommandTries);

                        if (CurrentTicket.Ticket.TicketStatus == TicketStatus.FailedToStart ||
                            CurrentTicket.Ticket.TicketStatus == TicketStatus.FailedToContinue)
                        {
                            break;
                        }

                        if (stillWaitingForResponse) continue;
                        Console.WriteLine("AGUARDANDO COMEÇAR");
                        if (CurrentCommand.Status == Status.Waiting) WaitCommandStart();

                        if (CurrentCommand.Status == Status.Waiting) break;
                        Console.WriteLine("AGUARDANDO FINALIZAR");
                        if (CurrentCommand.Status == Status.Executing) WaitCommandFinish();

                        if (CurrentCommand.Status == Status.Executing) break;
                    }

                    SetExecutingCommand(false);
                    _ticketsExecuted.Add(CurrentTicket);
                } while (AnyCommandExecutingOrWaiting());
                
                _eventService.AThreadIsShuttingDownEvent(this, new IoGServicedEventArgs<ITicketThread>(this));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public void AddTicket(TicketDto ticket, PriorityValue priorityValue, IEnumerable<ForwarderMessage> forwarderMessage)
        {
            var ticketThreadObject = new TicketThreadObject(ticket, priorityValue, forwarderMessage);
            Tickets.Add(ticketThreadObject);
        }

        public int GetNumberOfCommands()
        {
            return Tickets.Sum(ticketThreadObject =>
                ticketThreadObject.Ticket.CommandTickets.Count(command =>
                    command.Status == Status.Waiting || command.Status == Status.Executing));
        }

        public void SetExecutingCommand(bool value)
        {
            _executingCommandSemaphore.Wait();
            _isExecutingCommand = value;
            _executingCommandSemaphore.Release();
        }

        public bool CheckIfIsExecutingCommand()
        {
            _executingCommandSemaphore.Wait();
            var check = _isExecutingCommand;
            _executingCommandSemaphore.Release();
            return check;
        }

        private void SetWaitingCommandMessage(bool value)
        {
            _waitingCommandMessageSemaphore.Wait();
            _isWaitingCommandMessage = value;
            _waitingCommandMessageSemaphore.Release();
        }

        private bool CheckIfIsWaitingCommandMessage(out bool check)
        {
            _waitingCommandMessageSemaphore.Wait();
            check = _isWaitingCommandMessage;
            _waitingCommandMessageSemaphore.Release();
            return check;
        }

        public bool IsDying()
        {
            return !AnyCommandExecutingOrWaiting() && CheckIfIsExecutingCommand();
        }

        public void ReceivedUpdate(object sender, MessageReceivedArgs messageReceivedArgs)
        {
            var ticketCommand = JsonConvert.DeserializeObject<NetworkObjects.CommandTicketDto>(messageReceivedArgs.MessageReceived.Payload);
            SetWaitingCommandMessage(false);
            CommunicationStatus = ticketCommand.CommunicationStatus;
            Status = ticketCommand.Status;
            if (ticketCommand.CommandId == CurrentCommand.CommandId)
            {
                CurrentCommand.InitialDate = ticketCommand.InitialDate;
                CurrentCommand.FinishDate = ticketCommand.FinishDate;
                CurrentCommand.Attempt = ticketCommand.Attempt;
                CurrentCommand.Pdu = ticketCommand.Pdu;
                CurrentCommand.Status = ticketCommand.Status;
                CurrentCommand.StatusCommand = ticketCommand.StatusCommand;
                CurrentCommand.Answer = ticketCommand.Answer;
                CurrentCommand.Command = ticketCommand.Command;
                    
               // _eventService.ATicketNeedsToBeUpdatedEvent(this, new IoGServicedEventArgs<CommandTicketDto>(CurrentCommand));
            }
        }

        public string GetDeviceSerial()
        {
            return Smc.Serial;
        }

        public Command GetCurrentCommand()
        {
            return Command;
        }

        public CommunicationStatus GetCommunicationStatus()
        {
            return CommunicationStatus;
        }

        private HttpResponseMessage SendCommand(ForwarderMessage forwarderMessage)
        {
            try
            {
                return _forwarderSenderService.SendPost(forwarderMessage).Result;
            }
            catch(Exception e)
            {
                throw;
            }
        }

        private bool AnyCommandExecutingOrWaiting()
        {
            return Tickets.Any(ticketThreadObject => ticketThreadObject.Ticket.CommandTickets.Any(command =>
                command.Status == Status.Executing || command.Status == Status.Waiting));
        }

        private bool WaitConfirmation()
        {
            var timeout = TimeSpan.FromSeconds(_commandRules.CommandAnswerTimeoutInSeconds);
            var watch = Stopwatch.StartNew();
            CheckIfIsWaitingCommandMessage(out var isWaiting);
            while (watch.Elapsed < timeout && CheckIfIsWaitingCommandMessage(out isWaiting))
            {
                Thread.Sleep(1000);
            }

            return isWaiting;
        }

        private void WaitCommandStart()
        {
            WaitCommandChangeStatus(Status.Waiting);
        }

        private void WaitCommandFinish()
        {
            WaitCommandChangeStatus(Status.Executing);
        }

        private void WaitCommandChangeStatus(Status status)
        {
            var timeout = TimeSpan.FromSeconds(_commandRules.CommandAnswerTimeoutInSeconds);
            var watch = Stopwatch.StartNew();
            SetWaitingCommandMessage(true);
            while (watch.Elapsed < timeout && CurrentCommand.Status == status)
            {
                Thread.Sleep(1000);
                CheckIfIsWaitingCommandMessage(out var isWaiting);
                if (isWaiting || CurrentCommand.Status != status) continue;
                watch.Restart();
                SetWaitingCommandMessage(true);
            }
        }

        public void Dispose()
        {
            _executingCommandSemaphore?.Dispose();
            _waitingCommandMessageSemaphore?.Dispose();
            _answerSemaphore?.Dispose();
            Tickets?.Dispose();
            _mqttClient.ManagedMqttClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}