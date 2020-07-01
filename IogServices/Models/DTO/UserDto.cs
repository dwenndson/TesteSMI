using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using IogServices.Enums;
using IogServices.Models.DAO.Generic;
using IogServices.Models.DTO.Generic;
using Microsoft.IdentityModel.Tokens;

namespace IogServices.Models.DTO
{
    public class UserDto : BaseDto
    {
        public string Description { get; set; }
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public ClientType ClientType { get; set; }
        public UserType UserType { get; set; }
    }
    
    public class TokenConfigurations
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int Seconds { get; set; }
    }

    public class SigningConfigurations
    {
        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }

        public SigningConfigurations()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
            {
                Key = new RsaSecurityKey(provider.ExportParameters(true));
            }

            SigningCredentials = new SigningCredentials(
                Key, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}