using IogServices.Models.DTO.Generic;

namespace IogServices.Models.DTO
{
    public class MeterUnregisteredDto : BaseDto
    {
        public string Serial { get; set; }
        public string MeterPhase { get; set; }
        public string DeviceEui { get; set; }

    }
}