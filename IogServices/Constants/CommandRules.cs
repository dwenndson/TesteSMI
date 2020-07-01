using Newtonsoft.Json;

namespace IogServices.Constants
{
    [JsonObject("CommandRules")]
    public class CommandRules
    {
        [JsonProperty("CommandAnswerTimeoutInSeconds")] public int CommandAnswerTimeoutInSeconds { get; set; }
        [JsonProperty("IntervalBetweenCommandTriesInSeconds")] public int IntervalBetweenCommandTriesInSeconds { get; set; }
        [JsonProperty("NumberOfCommandTries")] public int NumberOfCommandTries { get; set; }
    }
}