using System.Collections.Generic;
using IogServices.Models.DAO;

namespace IogServices.Repositories
{
    public interface IMeterRepository : IGenericRepository<Meter>
    {
        Meter GetActiveBySerial(string serial);
        Meter GetBySerial(string serial);
        List<Meter> GetByModem(Modem modem);
        List<Meter> GetAllBySmc(string serial);
        List<Meter> GetAllComissioned();
        List<Meter> GetAllNotComissioned();
        List<Meter> GetAllNotBelongingToASmc();

    }
}