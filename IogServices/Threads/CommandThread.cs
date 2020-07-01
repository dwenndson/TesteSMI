using System;
using System.Collections.Concurrent;
using System.Threading;
using IogServices.Constants;
using IogServices.Enums;
using IogServices.Models.DTO;
using IogServices.Services;
using IogServices.Util;
using Microsoft.Extensions.DependencyInjection;
using MqttClientLibrary;
using MqttClientLibrary.Models;

namespace IogServices.Threads
{
    public class CommandThread
    {
        public MeterDto MeterDto { get; set; }
        public CommandStatus CommandStatus { get; set; }
        public CommunicationStatus CommunicationStatus { get; set; }
        private bool Answered { get; set; } = false;
        private readonly Semaphore _mutex;
        private readonly Semaphore _answeredSemaphore;
        private readonly ConcurrentQueue<Command> _commandQueue;
        private readonly IoGMqttClient _mqttClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly CommandRules _commandRules;

        public CommandThread(MeterDto meterDto, ClientSettings clientSettings, BrokerSettings brokerSettings,
            IMqttClientConfiguration mqttClientConfiguration, IMqttClientMethods mqttClientMethods, Topic topic,
            IServiceProvider serviceProvider, CommandRules commandRules)
        {
            MeterDto = meterDto;
            _mutex = new Semaphore(0, 10000);
            _answeredSemaphore = new Semaphore(1, 1);
            _commandQueue = new ConcurrentQueue<Command>();
            _serviceProvider = serviceProvider;
            _commandRules = commandRules;


            clientSettings.ClientId = clientSettings.ClientId.Replace("serial", meterDto.Serial);
            clientSettings.ClientName = clientSettings.ClientName.Replace("serial", meterDto.Serial);
            topic.Address = topic.Address.Replace("{smc-or-meter}", "meter").Replace("{serial}", meterDto.Serial);
            _mqttClient = new IoGMqttClient(clientSettings, brokerSettings, mqttClientConfiguration, mqttClientMethods);
            _mqttClient.Subscribe(topic);
            _mqttClient.MessageReceivedHandler += ReceivedCommandAnswer;
        }

        public void Run()
        {
            while (true)
            {
                _mutex.WaitOne();
                _commandQueue.TryDequeue(out var commandPeek);
                CommandStatus = commandPeek.CommandStatus;
                CommunicationStatus = CommunicationStatus.Busy;
                SendAndWaitAnswerOrTimeOut(commandPeek.ForwarderMessage);
                _answeredSemaphore.WaitOne();
                if (!Answered)
                {
                    var commandAnswer = "timeout";
                    CommunicationStatus = CommunicationStatus.CommunicationFailed;
                    SendCommandAnswer(commandAnswer);
                }

                Answered = false;
                _answeredSemaphore.Release();

            }
        }

        private void SendCommand(ForwarderMessage message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedForwarderSender = scope.ServiceProvider.GetRequiredService<IForwarderSenderService>();
                scopedForwarderSender.SendPost(message);
            }
        }

        private void SendCommandAnswer(string answer)
        {
            
        }

        public void AddCommand(Command command)
        {
            _commandQueue.Enqueue(command);
            _mutex.Release();
        }

        private void ReceivedCommandAnswer(object sender, MessageReceivedArgs messageReceivedArgs)
        {
            _answeredSemaphore.WaitOne();
            if (CommunicationStatus != CommunicationStatus.Busy || CommandStatus != CommandStatus.NoCommissioned/*TODO*/) return;
            Answered = true;
            SendCommandAnswer(messageReceivedArgs.MessageReceived.Payload);
            CommunicationStatus = CommunicationStatus.Success;
            _answeredSemaphore.Release();
        }

        private void SendAndWaitAnswerOrTimeOut(ForwarderMessage message)
        {
            SendCommand(message);
            var start = DateTime.Now;
            DateTime end;
            do
            {
                end = DateTime.Now;
            } while ((end - start).TotalSeconds <= _commandRules.CommandAnswerTimeoutInSeconds && !Answered);
        }

        public int GetNumberOfCommands()
        {
            return _commandQueue.Count;
        }
    }
}