using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IogServices.Constants;
using IogServices.Constants.ForwarderMqtt;
using IogServices.Enums;
using IogServices.Models.DTO;
using IogServices.Services;
using IogServices.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MqttClientLibrary;
using MqttClientLibrary.Models;
using Newtonsoft.Json;
using CommandTicketDto = NetworkObjects.CommandTicketDto;

namespace IogServices.BackgroundTasks
{
    public class IogServicesMqttCommandsReceiverHostedService : IHostedService
    {
        private readonly Mqtt _mqttConfiguration;
        private IIoGMqttClient _commandsMqttClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMqttClientConfiguration _mqttClientConfiguration;
        private readonly IMqttClientMethods _mqttClientMethods;
        private readonly List<CommandDeviceTopic> _commandDeviceTopics = new List<CommandDeviceTopic>();
        private readonly IEventService _eventService;

        public IogServicesMqttCommandsReceiverHostedService(IOptions<Forwarder> mqttConfiguration,
            IServiceProvider serviceProvider, IMqttClientMethods mqttClientMethods,
            IMqttClientConfiguration mqttClientConfiguration, IEventService eventService)
        {
            _mqttConfiguration = mqttConfiguration.Value.Mqtt;
            _serviceProvider = serviceProvider;
            _mqttClientConfiguration = mqttClientConfiguration;
            _mqttClientMethods = mqttClientMethods;
            _eventService = eventService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _commandsMqttClient = new IoGMqttClient(_mqttConfiguration.CommandsBaseClientSettings,
                _mqttConfiguration.BrokerSettings, _mqttClientConfiguration, _mqttClientMethods);

            IEnumerable<string> meterList;
            IEnumerable<string> smcList;
            
            using (var scope = _serviceProvider.CreateScope())
            {
                var meterService = scope.ServiceProvider.GetRequiredService<IMeterService>();
                var smcService = scope.ServiceProvider.GetRequiredService<ISmcService>();
                meterList = meterService.GetAllNotBelongingToASmc().Select(meter => meter.Serial);
                smcList = smcService.GetAll().Select(smc => smc.Serial);
            }

            foreach (var meter in meterList)
            {
                var topicAddress = AddDeviceToTopicList(meter, CommandDeviceType.Meter);
                var topic = MakeTopicObject(topicAddress);
                _commandsMqttClient.Subscribe(topic);
            }

            foreach (var smc in smcList)
            {
                var topicAddress = AddDeviceToTopicList(smc, CommandDeviceType.Smc);
                var topic = MakeTopicObject(topicAddress);
                _commandsMqttClient.Subscribe(topic);
            }
            
            AssignEvents();
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private string AddDeviceToTopicList(string serial, CommandDeviceType commandDeviceType)
        {
            var topic = _mqttConfiguration.BaseCommandsSubscribeTopic.Address
                .Replace("{smc-or-meter}", commandDeviceType.ToString().ToLower())
                .Replace("{serial}", serial);
            
            var deviceTopic = new CommandDeviceTopic()
            {
                DeviceSerial = serial,
                DeviceTopic = topic
            };
            
            _commandDeviceTopics.Add(deviceTopic);

            return topic;
        }

        private Topic MakeTopicObject(string address)
        {
            return new Topic(){Address = address, QoS = _mqttConfiguration.BaseCommandsSubscribeTopic.QoS};
        }

        public void AddMeter(MeterDto meterDto)
        {
            var topicAddress = AddDeviceToTopicList(meterDto.Serial, CommandDeviceType.Meter);
            var topic = MakeTopicObject(topicAddress);
            _commandsMqttClient.Subscribe(topic);
        }

        private void AddSmc(SmcDto smc)
        {
            var topicAddress = AddDeviceToTopicList(smc.Serial, CommandDeviceType.Smc);
            var topic = MakeTopicObject(topicAddress);
            _commandsMqttClient.Subscribe(topic);
        }

        public void RemoveMeter(MeterDto meterDto)
        {
            var deviceTopic = _commandDeviceTopics.Find(x => x.DeviceSerial == meterDto.Serial);
            if (deviceTopic == null) return;
            _commandDeviceTopics.Remove(deviceTopic);
            _commandsMqttClient.Unsubscribe(deviceTopic.DeviceTopic);
        }

        private void MessageReceived(object sender, MessageReceivedArgs messageReceivedArgs)
        {
            Task.Run(() => ProcessMessageReceived(messageReceivedArgs.MessageReceived));
        }

        private Task ProcessMessageReceived(Message message)
        {
            var topic = message.Topic;
            var deviceIdentifier = GetDeviceIdentifierFromTopic(topic);
            var deviceType = GetDeviceTypeFromTopic(topic);
            var commandDeviceTopic = _commandDeviceTopics.Find(x => x.DeviceSerial == deviceIdentifier);
            if(commandDeviceTopic == null) return Task.CompletedTask;

            commandDeviceTopic.Semaphore.Wait();
            var commandTicketReceived = JsonConvert.DeserializeObject<CommandTicketDto>(message.Payload);
            var newCommand = new Models.DTO.CommandTicketDto()
            {
                CommandId = commandTicketReceived.CommandId,
                InitialDate = commandTicketReceived.InitialDate,
                FinishDate = commandTicketReceived.FinishDate,
                Attempt = commandTicketReceived.Attempt,
                Pdu = commandTicketReceived.Pdu,
                Status = commandTicketReceived.Status,
                StatusCommand = commandTicketReceived.StatusCommand,
                Answer = commandTicketReceived.Answer,
                Command = commandTicketReceived.Command,
                CommunicationStatus = commandTicketReceived.CommunicationStatus,
                
            };
            
            using (var scope = _serviceProvider.CreateScope())
            {
                var ticketService = scope.ServiceProvider.GetRequiredService<ITicketService>();
                var ticket = ticketService.GetByTicketId(commandTicketReceived.CommandId);
                if (ticket != null)
                {
                    var ticketCommand =
                        ticket.CommandTickets.Find(command => command.CommandId == commandTicketReceived.CommandId);
                    if (ticketCommand != null)
                    {
                        ticketService.Update(newCommand);
                    }
                    else
                    {
                        ticketService.AddCommand(newCommand);
                    }
                }
                else
                {
                    var command = ticketService.GeCommandTicketDtoById(commandTicketReceived.CommandId);
                    if (command != null)
                    {
                        ticketService.Update(newCommand);
                    }
                    else
                    {
                        ticketService.Create(newCommand, deviceIdentifier);
                    }
                }
            }
            commandDeviceTopic.Semaphore.Release();
            
            return Task.CompletedTask;
        }

        private static string GetDeviceIdentifierFromTopic(string topic)
        {
            var subTopics = topic.Split("/");

            return subTopics[4];
        }

        private static CommandDeviceType GetDeviceTypeFromTopic(string topic)
        {
            var deviceTypeFromTopic = topic.Split("/")[3];

            switch (deviceTypeFromTopic)
            {
                case "smc":
                    return CommandDeviceType.Smc;
                case "meter":
                    return CommandDeviceType.Meter;
                default:
                    throw new NotImplementedException();
            }
        }

        private void AssignEvents()
        {
            _commandsMqttClient.MessageReceivedHandler += MessageReceived;
            _eventService.ASmcWasSaved += ASmcWasSaved;
            _eventService.AMeterWasSaved += AMeterWasSaved;
        }

        private void ASmcWasSaved(object sender, IoGServicedEventArgs<SmcDto> eventArgs)
        {
            if(eventArgs.ObjectFromEvent != null)
                AddSmc(eventArgs.ObjectFromEvent);
        }

        private void ResubscribeToDeviceTopic(string serial)
        {
            var commandDeviceTopic = _commandDeviceTopics.Find(deviceTopic => deviceTopic.DeviceSerial == serial);
            var topic = MakeTopicObject(commandDeviceTopic.DeviceTopic);
            _commandsMqttClient.Subscribe(topic);
        }

        private void UnsubscribeFromDeviceTopic(string serial)
        {
            var commandDeviceTopic = _commandDeviceTopics.Find(deviceTopic => deviceTopic.DeviceSerial == serial);
            _commandsMqttClient.Unsubscribe(commandDeviceTopic.DeviceTopic);
        }

        private void AMeterWasSaved(object sender, IoGServicedEventArgs<MeterDto> eventArgs)
        {
            if(eventArgs.ObjectFromEvent != null)
                AddMeter(eventArgs.ObjectFromEvent);
        }
    }
}