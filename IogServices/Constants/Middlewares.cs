using Newtonsoft.Json;

namespace IogServices.Constants
{
    [JsonObject("Middlewares")]
    public class Middlewares
    {
        [JsonProperty("EletraSmc")] public EletraSmcMiddleware.EletraSmc EletraSmc { get; set; }
        [JsonProperty("EletraSmi")] public EletraSmiMiddleware.EletraSmi EletraSmi { get; set; }
        [JsonProperty("NansenSmi")] public NansenSmiMiddleware.NansenSmi NansenSmi { get; set; }

    }
}