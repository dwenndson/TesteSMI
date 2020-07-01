using System.ComponentModel.DataAnnotations;

namespace IogServices.Models.DTO
{
    public class KeysDto
    {
        // [Required]
        // [MaxLength(32, ErrorMessage = "A chave AK deve possuir 32 caracteres")]
        // [MinLength(32, ErrorMessage = "A chave AK deve possuir 32 caracteres")]
        public string Ak { get; set; }
        // [Required]
        // [MaxLength(32, ErrorMessage = "A chave EK deve possuir 32 caracteres")]
        // [MinLength(32, ErrorMessage = "A chave EK deve possuir 32 caracteres")]

        public string Ek { get; set; }
        // [Required]
        // [MaxLength(32, ErrorMessage = "A chave MK deve possuir 32 caracteres")]
        // [MinLength(32, ErrorMessage = "A chave MK deve possuir 32 caracteres")]
        public string Mk { get; set; }
        // [Required]
        public string Serial { get; set; }
    }
}