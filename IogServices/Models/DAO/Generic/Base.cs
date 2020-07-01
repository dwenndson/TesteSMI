using System;

namespace IogServices.Models.DAO.Generic
{
    public class Base
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool Active { get; set; } = true; 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}