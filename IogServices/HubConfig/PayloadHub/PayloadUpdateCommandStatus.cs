using IogServices.Enums;
using NetworkObjects.Enum;

namespace IogServices.HubConfig.PayloadHub
{
    public class PayloadUpdateCommandStatus : PayloadCommandHub
    {
        public Command Command { get ;set;  }
        public string Description { get; set; }

        public PayloadUpdateCommandStatus(string Serial, Command command, string UpdateAt, string description)
        {
            this.Serial = Serial;
            this.Command = command;
            this.UpdateAt = UpdateAt;
            this.Description = description;
        }
    }
}
