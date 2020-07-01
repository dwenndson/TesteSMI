using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IogServices.Models.DAO.Generic;
using IogServices.Models.DTO;
using IogServices.Models.DTO.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IogServices.Models.DAO
{
    public class MeterModel : Base
    {
        public string Name { get; set; }
            
        public Manufacturer Manufacturer { get; set; }

        public List<Meter> Meters { get; set; }
        
        public void UpdateFields(MeterModel meterModel)
        {
            Name = meterModel.Name;
            Manufacturer = meterModel.Manufacturer;
        }
    }
}