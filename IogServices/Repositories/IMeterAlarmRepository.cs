using System.Collections.Generic;
using IogServices.Models;
using IogServices.Models.DAO;

namespace IogServices.Repositories
{
    public interface IMeterAlarmRepository : IGenericRepository<MeterAlarm>
    {
        List<MeterAlarm> GetBySerial(string serial);
        
    }
}