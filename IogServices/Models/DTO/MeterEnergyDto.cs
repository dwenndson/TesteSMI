using System;
using System.ComponentModel.DataAnnotations;
using IogServices.Models.DTO.Generic;
using Newtonsoft.Json;

namespace IogServices.Models.DTO
{
    public class MeterEnergyDto : BaseDto
    {
        [Required]
        public string DirectEnergy { get; set; }
        [Required]
        public string ReverseEnergy { get; set; }
        [Required]
        public string ReadingTime { get; set; }
        [JsonIgnore]
        public MeterDto Meter { get; set; }
    } 
}