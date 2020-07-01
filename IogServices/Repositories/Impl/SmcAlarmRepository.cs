using System;
using System.Collections.Generic;
using System.Linq;
using IogServices.Models.DAO;
using Microsoft.EntityFrameworkCore;

namespace IogServices.Repositories.Impl
{
    public class SmcAlarmRepository : ISmcAlarmRepository
    {
        private readonly IogContext _iogContext;

        public SmcAlarmRepository(IogContext iogContext)
        {
            _iogContext = iogContext;
        }
        public List<SmcAlarm> GetAll()
        {
            return _iogContext.SmcAlarms.ToList();
        }

        public SmcAlarm GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public SmcAlarm Save(SmcAlarm t)
        {
            _iogContext.SmcAlarms.Add(t);
            _iogContext.SaveChanges();
            return t;
        }

        public SmcAlarm Update(SmcAlarm t)
        {
            _iogContext.SmcAlarms.Update(t);
            _iogContext.SaveChanges();
            return t;
        }

        public List<SmcAlarm> GetBySerial(string serial)
        {
            return _iogContext.SmcAlarms.Include(a => a.Smc).Where(a => a.Smc.Serial == serial).OrderByDescending(a=> a.ReadDateTime).ToList();
        }
    }
}