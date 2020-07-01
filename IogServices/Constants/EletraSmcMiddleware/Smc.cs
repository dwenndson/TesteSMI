using Newtonsoft.Json;

namespace IogServices.Constants.EletraSmcMiddleware
{
    public class Smc
    {
        [JsonProperty("SubRoute")] public string SubRoute { get; set; }
    }
}