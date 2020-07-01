using IogServices.Models.DAO;

namespace IogServices.Repositories
{
    public interface IModemRepository : IGenericRepository<Modem>
    {
        Modem GetByEui(string eui);
    }
}