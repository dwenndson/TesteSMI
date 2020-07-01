using System;
using IogServices.Models.DTO.Generic;
using Newtonsoft.Json;

namespace IogServices.Models.DTO
{
    public class MeterAlarmDto : BaseDto
    {
        public DateTime ReadDateTime { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public string Serial { get; set; }
    }
}