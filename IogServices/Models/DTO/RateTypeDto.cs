using System.ComponentModel.DataAnnotations;
using IogServices.Models.DTO.Generic;
using Microsoft.CodeAnalysis.CSharp;

namespace IogServices.Models.DTO
{
    public class RateTypeDto : BaseDto
    {
        [Required]
        public string Name { get; set; }
    }
}