using System;
using System.Security.Cryptography;
using IogServices.Enums;
using IogServices.Models.DAO.Generic;

namespace IogServices.Models.DAO
{
    public class User : Base
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public ClientType ClientType { get; set; }
        public UserType UserType { get; set; }

        public void UpdateFields(User user)
        {
            Description = user.Description;
            Name = user.Name;
            Email = user.Email;
            ClientType = user.ClientType;
            UserType = user.UserType;
            
            if (!String.IsNullOrEmpty(user.Password))
            {
                var salt = GenerateSalt();
                Password = GenerateHash(user.Password, salt);
                Salt = Convert.ToBase64String(salt);
            }
        }
        
        public static byte[] GenerateSalt()
        {
            using(RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16];
                rng.GetBytes(salt);
                return salt;
            }
        }
        
        public static string GenerateHash(string password, byte[] salt)
        {
            var pass = new Rfc2898DeriveBytes(password, salt, 1000);
            byte[] hash = pass.GetBytes(32);
            return Convert.ToBase64String(hash);
        }
    }
}