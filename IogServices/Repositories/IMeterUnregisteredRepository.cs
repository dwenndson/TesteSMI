using IogServices.Models.DAO;

namespace IogServices.Repositories
{
    public interface IMeterUnregisteredRespository : IGenericRepository<MeterUnregistered>
    {
        MeterUnregistered GetBySerial(string serial);
    }
}