using IogServices.Models.DAO;
using IogServices.Models.DTO;

namespace IogServices.Services
{
    public interface IMeterUnregisteredService : IGenericService<MeterUnregisteredDto>
    {
        MeterUnregistered Save(MeterUnregistered t);
        MeterUnregisteredDto FindBySerial(string serial);
        MeterUnregisteredDto Deactive(string serial);
    }
}