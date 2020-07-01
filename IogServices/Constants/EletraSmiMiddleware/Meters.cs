using Newtonsoft.Json;

namespace IogServices.Constants.EletraSmiMiddleware
{
    public class Meters
    {
        [JsonProperty("SubRoute")] public string SubRoute { get; set; }
        [JsonProperty("RelayOn")] public string RelayOn { get; set; }
        [JsonProperty("RelayOff")] public string RelayOff { get; set; }
        [JsonProperty("RelayStatus")] public string RelayStatus { get; set; }
    }
}