using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using IogServices.Constants;
using IogServices.Constants.ForwarderMqtt;
using IogServices.Enums;
using IogServices.Models.DTO;
using IogServices.Threads;
using IogServices.Util;
using Microsoft.Extensions.Options;
using MqttClientLibrary;
using MqttClientLibrary.Models;
using CommandTicketDto = NetworkObjects.CommandTicketDto;

namespace IogServices.Services.Impl
{
    public class ThreadService : IThreadService
    {
        private static readonly List<ITicketThread> TicketThreads = new List<ITicketThread>();
        private readonly IMqttClientConfiguration _mqttClientConfiguration;
        private readonly IMqttClientMethods _mqttClientMethods;
        private readonly BrokerSettings _brokerSettings;
        private readonly ClientSettings _baseClientSettings;
        private readonly Topic _subscribeTopic;
        private readonly ISmcService _smcService;
        private readonly IMeterService _meterService;
        private readonly CommandRules _commandRules;
        private readonly IEventService _eventService;
        private readonly IForwarderSenderService _forwarderSenderService;

        public ThreadService(IMqttClientMethods mqttClientMethods, IMqttClientConfiguration mqttClientConfiguration,
            IOptions<Forwarder> forwarderConfig, IServiceProvider serviceProvider, ISmcService smcService,
            IOptions<CommandRules> commandRules, IEventService eventService,
            IForwarderSenderService forwarderSenderService, IMeterService meterService)
        {
            _mqttClientConfiguration = mqttClientConfiguration;
            _mqttClientMethods = mqttClientMethods;
            _brokerSettings = forwarderConfig.Value.Mqtt.BrokerSettings;
            _baseClientSettings = forwarderConfig.Value.Mqtt.CommandsBaseClientSettings;
            _subscribeTopic = forwarderConfig.Value.Mqtt.BaseCommandsSubscribeTopic;
            _smcService = smcService;
            _commandRules = commandRules.Value;
            _eventService = eventService;
            _eventService.AThreadIsShuttingDown += AThreadIsShuttingDown;
            _forwarderSenderService = forwarderSenderService;
            _meterService = meterService;
        }

        public void AddTicketToThread(TicketDto ticketDto, ForwarderMessage forwarderMessage,
            PriorityValue priorityValue, CommandDeviceType commandDeviceType)
        {
            AddTicketToThread(ticketDto, new []{forwarderMessage}, priorityValue, commandDeviceType);
        }

        public void AddTicketToThread(TicketDto ticketDto, ForwarderMessage[] forwarderMessage, PriorityValue priorityValue,
            CommandDeviceType commandDeviceType)
        {
            var thread = GetOrCreateTicketThread(ticketDto.Serial, commandDeviceType, out var wasCreated);
            thread.AddTicket(ticketDto, priorityValue, forwarderMessage);
            if(wasCreated) new Thread(() => thread.Run()).Start();
        }

        public ITicketThread GetTicketThread(string serial)
        {
            return TicketThreads.Find(thread => thread.GetDeviceSerial() == serial && !thread.IsDying());
        }

        private ITicketThread GetOrCreateTicketThread(string serial, CommandDeviceType commandDeviceType, out bool wasCreated)
        {
            var result = GetTicketThread(serial);
            if (result != null)
            {
                wasCreated = false;
                return result;
            }
            Console.WriteLine("Criando nova thread");
            switch (commandDeviceType)
            {
                case CommandDeviceType.Smc:
                    var smc = _smcService.GetBySerial(serial);
                    result = new SmcTicketThread(smc, _baseClientSettings, _brokerSettings,
                        _mqttClientConfiguration, _mqttClientMethods, _subscribeTopic,
                        _commandRules, _eventService, _forwarderSenderService);
                    TicketThreads.Add(result);
                    break;
                case CommandDeviceType.Meter:
                    var meter = _meterService.GetBySerial(serial);
                    result = new MeterTicketThread(meter, _baseClientSettings, _brokerSettings,
                        _mqttClientConfiguration, _mqttClientMethods, _subscribeTopic,
                        _commandRules, _eventService, _forwarderSenderService);
                    TicketThreads.Add(result);
                    break;
                default:
                    throw new NotImplementedException();
            }

            wasCreated = true;
            return result;
        }

        private static void AThreadIsShuttingDown(object sender, IoGServicedEventArgs<ITicketThread> ticketThreadEventArgs)
        {
            ticketThreadEventArgs.ObjectFromEvent.Dispose();
            TicketThreads.Remove(ticketThreadEventArgs.ObjectFromEvent);
        }
    }
}