using IogServices.Models.DTO.Generic;

namespace IogServices.Models.DTO
{
    public class ModemDto : BaseDto
    {
        public string DeviceEui { get; set; }
        public int FirmwareVersion { get; set; }
        public int Serial { get; set; }
    }
}