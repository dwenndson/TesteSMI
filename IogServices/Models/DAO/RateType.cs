using System.Collections.Generic;
using IogServices.Models.DAO.Generic;
using IogServices.Models.DTO;

namespace IogServices.Models.DAO
{
    public class RateType : Base
    {
        public string Name { get; set; }

        public List<Meter> Meters { get; set; }
        
        public void UpdateFields(RateType rateType)
        {
            Name = rateType.Name;
        }
    }
}