using MqttClientLibrary.Models;
using Newtonsoft.Json;

namespace IogServices.Constants.ForwarderMqtt
{
    public class Mqtt
    {
        [JsonProperty("BrokerSettings")] public BrokerSettings BrokerSettings { get; set; }
        [JsonProperty("HostedServiceClientSettings")] public ClientSettings HostedServiceClientSettings { get; set; }
        [JsonProperty("CommandsBaseClientSettings")] public ClientSettings CommandsBaseClientSettings { get; set; }
        [JsonProperty("HostedServiceSubscribeTopic")] public Topic HostedServiceSubscribeTopic { get; set; }
        [JsonProperty("BaseCommandsSubscribeTopic")] public Topic BaseCommandsSubscribeTopic { get; set; }
    }
}