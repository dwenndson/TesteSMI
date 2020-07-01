using IogServices.Models.DAO.Generic;

namespace IogServices.Models.DAO
{
    public class MeterUnregistered : Base
    {
        public string Serial { get; set; }
        public string MeterPhase { get; set; }
        public string DeviceEui { get; set; }

        public void UpdateFields(MeterUnregistered meterUnregistered)
        {
            MeterPhase = meterUnregistered.MeterPhase;
            DeviceEui = meterUnregistered.DeviceEui;
        }
    }
}