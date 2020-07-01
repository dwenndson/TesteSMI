using System.Collections.Generic;
using NetworkObjects.Enum;

namespace IogServices.HubConfig.PayloadHub
{
    public class PayloadUpdateAlarm : PayloadCommandHub
    {
        public List<string> Description { get; set; }
        public NetworkMessageType Type { get; set; }

        public PayloadUpdateAlarm (List<string> description, string serial, string updateAt, NetworkMessageType type)
        {
            this.Serial = serial;
            this.Description = description;
            this.UpdateAt = updateAt;
            this.Type = type;
        }
    }
}
