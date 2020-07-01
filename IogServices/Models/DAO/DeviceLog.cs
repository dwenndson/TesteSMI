using System;
using IogServices.Models.DAO.Generic;
using Microsoft.Extensions.Logging;

namespace IogServices.Models.DAO
{
    public class DeviceLog : Base
    { 
        public DateTime Date { get; set; }
        public string DeviceSerial { get; set; }
        public string Message { get; set; }
        public LogLevel LogLevel { get; set; }
    }
}