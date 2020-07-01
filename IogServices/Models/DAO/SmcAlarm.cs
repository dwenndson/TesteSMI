using System;
using EletraSmcModels;
using IogServices.Models.DAO.Generic;

namespace IogServices.Models.DAO
{
    public class SmcAlarm : Base
    {
        public Smc Smc { get; set; }
        public DateTime ReadDateTime { get; set; }
        public string Description { get; set; }
    }
}