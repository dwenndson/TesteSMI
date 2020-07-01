using System;
using IogServices.Models.DAO.Generic;

namespace IogServices.Models.DAO
{
    public class MeterAlarm : Base
    {
        public Meter Meter { get; set; }
        public DateTime ReadDateTime { get; set; }
        public string Description { get; set; }
    }
}