using System;
using System.ComponentModel.DataAnnotations;
using IogServices.Models.DTO.Generic;

namespace IogServices.Models.DTO
{
    public class ManufacturerDto : BaseDto
    {
        [Required]
        public string Name { get; set; }
    }
}