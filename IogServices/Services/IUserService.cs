using IogServices.Models.DAO;
using IogServices.Models.DTO;

namespace IogServices.Services
{
    public interface IUserService : IGenericService<UserDto>
    {
        UserDto GetByEmail(string email);
        void Deactivate(string email);
        object Login(UserDto userDto, SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations);
        User GetExistingUser(string email);
    }
}