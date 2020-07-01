using System.ComponentModel.DataAnnotations;
using IogServices.Models.DTO.Generic;

namespace IogServices.Models.DTO
{
    public class MeterModelDto : BaseDto
    {
        public string Name { get; set; }
        public ManufacturerDto Manufacturer { get; set; }
    }
}