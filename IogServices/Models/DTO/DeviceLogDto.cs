using System;
using IogServices.Models.DTO.Generic;
using Microsoft.Extensions.Logging;

namespace IogServices.Models.DTO
{
    public class DeviceLogDto : BaseDto
    {
        public DateTime Date { get; set; }
        public string DeviceSerial { get; set; }
        public string Message { get; set; }
        public LogLevel LogLevel { get; set; }
    }
}