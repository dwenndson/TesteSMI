using System;
using IogServices.Models.DAO.Generic;

namespace IogServices.Models.DAO
{
    public class MeterEnergy : Base
    {
        public string DirectEnergy { get; set; }
        public string ReverseEnergy { get; set; }
        public DateTime ReadingTime { get; set; }
        public Meter Meter { get; set; }
    }
}