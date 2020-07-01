using IogServices.Models.DAO;

namespace IogServices.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User GetByEmail(string email);
    }
}