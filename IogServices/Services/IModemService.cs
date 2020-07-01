using IogServices.Models.DAO;
using IogServices.Models.DTO;

namespace IogServices.Services
{
    public interface IModemService : IGenericService<ModemDto>
    {
        Modem GetExistingModem(string eui);
    }
}