using System.Collections.Generic;
using IogServices.Models.DAO;

namespace IogServices.Repositories
{
    public interface ISmcRepository : IGenericRepository<Smc>
    {
        List<Smc> GetByModem(Modem modem);
        Smc GetBySerial(string serial);
        List<Smc> GetAllComissioned();
        List<Smc> GetAllNotComissioned();

    }
}