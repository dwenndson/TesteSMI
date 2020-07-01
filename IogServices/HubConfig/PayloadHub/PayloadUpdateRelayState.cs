using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IogServices.Enums;
using NetworkObjects.Enum;

namespace IogServices.HubConfig.PayloadHub
{
    public class PayloadUpdateRelayState : PayloadCommandHub
    {
        public string StatusRelay { get => RelayStatus.ToString(); }
        public RelayStatus RelayStatus { get; set; }
        public AccountantStatus AccountantStatus { get; set; }
        public PayloadUpdateRelayState(string serial, string updateAt,  AccountantStatus accountantStatus)
        {
            this.Serial = serial;
            this.UpdateAt = updateAt;
            this.AccountantStatus = accountantStatus;
        }
    }
}
