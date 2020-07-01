using System;
using System.ComponentModel.DataAnnotations;
using IogServices.Models.DTO.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IogServices.Models.DTO
{
    public class SmcModelDto : BaseDto
    {
        [Required]
        public string Name { get; set; }
        
        public int PositionsCount { get; set; }
        
        public ManufacturerDto Manufacturer { get; set; }
    }
}