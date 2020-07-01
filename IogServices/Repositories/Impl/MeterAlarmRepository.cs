using System;
using System.Collections.Generic;
using System.Linq;
using IogServices.Models;
using IogServices.Models.DAO;
using Microsoft.EntityFrameworkCore;

namespace IogServices.Repositories.Impl
{
    public class MeterAlarmReposirory : IMeterAlarmRepository
    {
        private readonly IogContext _iogContext;

        public MeterAlarmReposirory(IogContext iogContext)
        {
            _iogContext = iogContext;
        }

        public List<MeterAlarm> GetAll()
        {
            return _iogContext.MeterAlarms.ToList();

        }

        public MeterAlarm GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public MeterAlarm Save(MeterAlarm t)
        {
            _iogContext.MeterAlarms.Add(t);
            _iogContext.SaveChanges();
            return t;
        }

        public MeterAlarm Update(MeterAlarm t)
        {
            throw new NotImplementedException();
        }

        public List<MeterAlarm> GetBySerial(string serial)
        {
            return _iogContext.MeterAlarms.Include(a => a.Meter).Where(a => a.Meter.Serial == serial).OrderByDescending(a=> a.ReadDateTime).ToList();
        }
    }
}