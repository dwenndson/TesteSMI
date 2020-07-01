using System.Collections.Generic;
using IogServices.Models.DAO;
using IogServices.Models.DTO;

namespace IogServices.Repositories
{
    public interface IMeterKeyRepository
    {
        MeterKeys GetBySerial(string serial);
        MeterKeys Create(MeterKeys meterKeys);
        MeterKeys Update(MeterKeys meterKeys);
        List<MeterKeys> GetAll();
    }
}