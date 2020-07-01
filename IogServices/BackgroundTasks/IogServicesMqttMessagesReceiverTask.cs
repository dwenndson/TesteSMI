using System;
using System.Threading;
using System.Threading.Tasks;
using IogServices.Constants;
using IogServices.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MqttClientLibrary;
using MqttClientLibrary.Models;

namespace IogServices.BackgroundTasks
{
    public class IogServicesMqttMessagesReceiverTask : IIoGMqttClientHostedService
    {
        private readonly IoGMqttClient _ioGMqttClient;
        private readonly Topic _topicToSubscribe;
        private IServiceProvider Services { get; }

        public IogServicesMqttMessagesReceiverTask(IOptionsMonitor<Forwarder> forwarderConfig,
            IMqttClientMethods mqttClientMethods,
            IMqttClientConfiguration mqttClientConfiguration,
            IServiceProvider services)
        {
            var client = forwarderConfig.CurrentValue.Mqtt.HostedServiceClientSettings;
            var broker = forwarderConfig.CurrentValue.Mqtt.BrokerSettings;
            _ioGMqttClient = new IoGMqttClient(client, broker, mqttClientConfiguration, mqttClientMethods);
            _topicToSubscribe = forwarderConfig.CurrentValue.Mqtt.HostedServiceSubscribeTopic;
            Services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _ioGMqttClient.MessageReceivedHandler += MessagedReceived;
            _ioGMqttClient.Subscribe(_topicToSubscribe);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Publish(Message message)
        {
            return _ioGMqttClient.Publish(message);
        }

        public void Subscribe(Topic topic)
        {
            _ioGMqttClient.Subscribe(topic);
        }

        public void MessagedReceived(object sender, MessageReceivedArgs messageReceivedArgs)
        {
            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService = 
                    scope.ServiceProvider
                        .GetRequiredService<IMiddlewaresMessageHandlerService>();

                scopedProcessingService.MessageReceivedFromMiddlewares(this, messageReceivedArgs);
            }
        }

        public event EventHandler<MessageReceivedArgs> MessageReceivedHandler;
    }
}