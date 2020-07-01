using System.Collections.Generic;
using IogServices.Models.DAO.Generic;
using IogServices.Models.DTO;

namespace IogServices.Models.DAO
{
    public class SmcUnregistered : Base
    {
        public string Serial { get; set; }
        public string DeviceEui { get; set; }
        public List<MeterUnregistered> Meters { get; set; }

        public void UpdateFields(SmcUnregistered smcUnregistered)
        {
            Meters = smcUnregistered.Meters;
            DeviceEui = smcUnregistered.DeviceEui;
        }
    }
}