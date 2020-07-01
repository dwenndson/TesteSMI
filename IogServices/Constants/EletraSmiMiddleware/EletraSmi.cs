using Newtonsoft.Json;

namespace IogServices.Constants.EletraSmiMiddleware
{
    public class EletraSmi
    {
        [JsonProperty("BaseUrl")] public string BaseUrl { get; set; }
        [JsonProperty("Meters")] public Meters Meters { get; set; }
    }
}