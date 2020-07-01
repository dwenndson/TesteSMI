using IogServices.Enums;
using IogServices.Models.DTO;
using NetworkObjects.Enum;

namespace IogServices.Util
{
    public class Command
    {
        public MeterDto MeterDto { get; set; }
        public ClientType ClientType { get; set; }
        public CommandType CommandType { get; set; }
        public ForwarderMessage ForwarderMessage { get; set; }
        public CommandStatus CommandStatus { get; set; }
    }
}