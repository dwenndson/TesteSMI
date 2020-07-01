using IogServices.Constants.ForwarderMqtt;
using Newtonsoft.Json;

namespace IogServices.Constants
{
    [JsonObject("Forwarder")]
    public class Forwarder
    {
        [JsonProperty("Mqtt")] public Mqtt Mqtt { get; set; }
    }
}