using System;
using System.Collections.Generic;
using IogServices.Models.DAO.Generic;
using IogServices.Models.DTO;

namespace IogServices.Models.DAO
{
    public class Manufacturer : Base
    {
        public string Name { get; set; }
        public List<SmcModel> SmcModels { get; set; }

        public List<MeterModel> MeterModels { get; set; }
        
        public void UpdateFields(Manufacturer manufacturer)
        {
            Name = manufacturer.Name;
        }
    }
}