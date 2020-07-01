using System;
using System.Collections.Generic;
using System.Linq;
using IogServices.Models.DAO;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account;
using Microsoft.EntityFrameworkCore;

namespace IogServices.Repositories.Impl
{
    public class MeterUnregisteredRepository : IMeterUnregisteredRespository
    {
        private readonly IogContext _iogContext;
        public MeterUnregisteredRepository(IogContext iogContext)
        {
            _iogContext = iogContext;
        }

        public List<MeterUnregistered> GetAll()
        {
            return _iogContext.MeterUnregistereds.Where(m => m.Active && m.Serial != null).ToList();
        }

        public MeterUnregistered GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public MeterUnregistered Save(MeterUnregistered t)
        {
            _iogContext.MeterUnregistereds.Add(t);
            _iogContext.SaveChanges();
            return t;
        }

        public MeterUnregistered Update(MeterUnregistered t)
        {
            _iogContext.MeterUnregistereds.Update(t);
            _iogContext.SaveChanges();
            return t;
        }

        public MeterUnregistered GetBySerial(string serial)
        {
            return _iogContext.MeterUnregistereds.FirstOrDefault(s => s.Serial == serial && s.Active);
        }
    }
}