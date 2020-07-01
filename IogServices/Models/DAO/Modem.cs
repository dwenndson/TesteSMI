using IogServices.Models.DAO.Generic;

namespace IogServices.Models.DAO
{
    public class Modem : Base
    {
        public string DeviceEui { get; set; }
        public int FirmwareVersion { get; set; }
        public int Serial { get; set; }
        public void UpdateFields(Modem modem)
        {
            DeviceEui = modem.DeviceEui;
            FirmwareVersion = modem.FirmwareVersion;
            Serial = modem.Serial;
        }
    }
    
   
}