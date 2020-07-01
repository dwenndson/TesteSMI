using System;
using System.Collections.Generic;
using IogServices.Models.DAO.Generic;
using IogServices.Models.DTO;

namespace IogServices.Models.DAO
{
    public class SmcModel : Base
    {
        public string Name { get; set; }
        
        public int PositionsCount { get; set; }
        public Manufacturer Manufacturer { get; set; }
        
        public List<Smc> Smcs { get; set; }
        
        public void UpdateFields(SmcModel smcModel)
        {
            Name = smcModel.Name;
            PositionsCount = smcModel.PositionsCount;
            Manufacturer = smcModel.Manufacturer;
        }
    }
}