using System.Collections.Generic;
using System.Linq;
using IogServices.Models.DAO;
using IogServices.Services;

namespace IogServices.Repositories.Impl
{
    public class MeterKeyRepository : IMeterKeyRepository
    {
        private readonly IogContext _iogContext;
        public MeterKeyRepository(IogContext iogContext)
        {
            _iogContext = iogContext;
        }
        public MeterKeys GetBySerial(string serial)
        {
            return _iogContext.MeterKeys.FirstOrDefault(keys => keys.Serial == serial);
        }

        public MeterKeys Create(MeterKeys meterKeys)
        {
            _iogContext.MeterKeys.Add(meterKeys);
            _iogContext.SaveChanges();
            return GetBySerial(meterKeys.Serial);
        }

        public MeterKeys Update(MeterKeys meterKeys)
        {
            _iogContext.Update(meterKeys);
            _iogContext.SaveChanges();
            return meterKeys;
        }

        public List<MeterKeys> GetAll()
        {
            return _iogContext.MeterKeys.Where(m => m.Active).ToList();
        }
    }
}