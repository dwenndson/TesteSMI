using IogServices.Models.DAO;
using IogServices.Models.DTO;

namespace IogServices.Services
{
    public interface ISmcUnregisteredService : IGenericService<SmcUnregisteredDto>
    {
        SmcUnregistered Save(SmcUnregistered t);
        SmcUnregisteredDto FindBySerial(string serial);
        SmcUnregisteredDto Deactive(string serial);
    }
}