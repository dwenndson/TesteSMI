using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IogServices.Models.DAO;
using IogServices.Models.DTO.Generic;

namespace IogServices.Models.DTO
{
    public class SmcDto : BaseDto
    {
        public string Description { get; set; }
        [Required]
        public string Serial { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        [Required]
        public SmcModelDto SmcModel { get; set; }
        public ModemDto Modem { get; set; }
        public KeysDto KeysDto { get; set; }
    }
}