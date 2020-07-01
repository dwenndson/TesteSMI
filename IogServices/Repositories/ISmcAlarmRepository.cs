using System.Collections.Generic;
using IogServices.Models.DAO;

namespace IogServices.Repositories
{
    public interface ISmcAlarmRepository : IGenericRepository<SmcAlarm>
    {
        List<SmcAlarm> GetBySerial(string serial);
        
    }
}