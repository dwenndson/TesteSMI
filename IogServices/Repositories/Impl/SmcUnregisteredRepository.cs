using System;
using System.Collections.Generic;
using System.Linq;
using IogServices.Models.DAO;
using Microsoft.EntityFrameworkCore;

namespace IogServices.Repositories.Impl
{
    public class SmcUnregisteredRepository : ISmcUnregisteredRespository
    {
        private readonly IogContext _iogContext;
        public SmcUnregisteredRepository(IogContext iogContext)
        {
            _iogContext = iogContext;
        }
        public List<SmcUnregistered> GetAll()
        {
            return _iogContext.SmcUnregistereds.Include(s => s.Meters).Where(s=> s.Active).ToList();
        }

        public SmcUnregistered GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public SmcUnregistered Save(SmcUnregistered t)
        {
            _iogContext.SmcUnregistereds.Add(t);
            _iogContext.SaveChanges();
            return t;
        }

        public SmcUnregistered Update(SmcUnregistered t)
        {
            _iogContext.SmcUnregistereds.Update(t);
            _iogContext.SaveChanges();
            return t;
        }

        public SmcUnregistered GetBySerial(string serial)
        {
            return _iogContext.SmcUnregistereds.Include(s => s.Meters).FirstOrDefault(s => s.Serial == serial && s.Active);
        }
    }
}