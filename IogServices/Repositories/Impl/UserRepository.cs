using System;
using System.Collections.Generic;
using System.Linq;
using IogServices.Models.DAO;

namespace IogServices.Repositories.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly IogContext _iogContext;

        public UserRepository(IogContext iogContext)
        {
            _iogContext = iogContext;
        }
        
        public List<User> GetAll()
        {
            return _iogContext.Users.Where(user => user.Active).OrderByDescending(user => user.UpdatedAt).ToList();
        }

        public User GetById(Guid id)
        {
            return _iogContext.Users.FirstOrDefault(user => user.Active && user.Id == id);
        }

        public User GetByEmail(string email)
        {
            return _iogContext.Users.FirstOrDefault(user => user.Active && user.Email == email);
        }

        public User Save(User user)
        {
            _iogContext.Users.Add(user);
            _iogContext.SaveChanges();
            return user;
        }

        public User Update(User user)
        {
            _iogContext.Users.Update(user);
            _iogContext.SaveChanges();
            return user;
        }
    }
}