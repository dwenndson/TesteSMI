using Newtonsoft.Json;

namespace IogServices.Constants.EletraSmcMiddleware
{
    public class EletraSmc
    {
        [JsonProperty("BaseUrl")] public string BaseUrl { get; set; }
        [JsonProperty("Smc")] public Smc Smc { get; set; }
        [JsonProperty("Meters")] public Meters Meters { get; set; }
    }
}