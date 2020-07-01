using Newtonsoft.Json;

namespace IogServices.Constants.NansenSmiMiddleware
{
    public class NansenSmi
    {
        [JsonProperty("BaseUrl")] public string BaseUrl { get; set; }
        [JsonProperty("Meters")] public Meters Meters { get; set; }
    }
}