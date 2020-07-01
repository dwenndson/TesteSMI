using System.Collections.Generic;
using IogServices.Models.DTO.Generic;

namespace IogServices.Models.DTO
{
    public class SmcUnregisteredDto : BaseDto
    {
        public string Serial { get; set; }
        public string DeviceEui { get; set; }
        public List<MeterUnregisteredDto> Meters { get; set; }
        
    }
}