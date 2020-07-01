using IogServices.Models.DAO;

namespace IogServices.Repositories
{
    public interface ISmcUnregisteredRespository : IGenericRepository<SmcUnregistered>
    {
        SmcUnregistered GetBySerial(string serial);
    }
}