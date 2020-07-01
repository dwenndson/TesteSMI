using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace IogServices.HubConfig.PayloadHub
{
    public class PayloadUpdateLog : PayloadCommandHub
    {
        public string Message { get; set; }
        public LogLevel LogLevel { get; set; }

        public PayloadUpdateLog(string message, string serial, string updateAt, LogLevel logLevel)
        {
            this.Message = message;
            this.Serial = serial;
            this.UpdateAt = updateAt;
            this.LogLevel = logLevel;
        }
    }
}
