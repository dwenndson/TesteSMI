using System.Collections.Generic;
using IogServices.Models.DAO.Generic;

namespace IogServices.Models.DAO
{
    public class Smc : Base
    {
        public string Description { get; set; }
        public string Serial { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public SmcModel SmcModel { get; set; }
        public Modem Modem { get; set; }
        public List<Meter> Meters { get; set; }
        
        public List<SmcAlarm> SmcAlarms { get; set; }
        public bool Comissioned { get; set; }


        
        public void UpdateFields(Smc smc)
        {
            Description = smc.Description;
            Serial = smc.Serial;
            Latitude = smc.Latitude;
            Longitude = smc.Longitude;
            SmcModel = smc.SmcModel;
        }
    }
}