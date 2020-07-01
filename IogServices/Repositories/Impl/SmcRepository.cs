using System;
using System.Collections.Generic;
using System.Linq;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace IogServices.Repositories.Impl
{
    public class SmcRepository : ISmcRepository
    {

        private readonly IogContext _iogContext;

        public SmcRepository(IogContext iogContext)
        {
            _iogContext = iogContext;
        }
        
        public List<Smc> GetAll()
        {
            return _iogContext.Smcs.Where(smc => smc.Active)
                .Include(smc => smc.SmcModel)
                .Include(smc => smc.Modem)
                .ToList();
        }

        public Smc GetById(Guid id)
        {
            return _iogContext.Smcs
                .Include(smc => smc.SmcModel)
                .Include(smc => smc.Modem)
                .FirstOrDefault(smc => smc.Id == id);
        }

        public Smc Save(Smc smc)
        {
            _iogContext.Smcs.Add(smc);
            _iogContext.SaveChanges();
            return smc;
        }

        public Smc Update(Smc smc)
        {
            _iogContext.Update(smc);
            _iogContext.SaveChanges();
            return smc;
        }

        public List<Smc> GetByModem(Modem modem)
        {
            return _iogContext.Smcs.Where(smc => smc.Modem.DeviceEui == modem.DeviceEui && smc.Active).ToList();
        }

        public Smc GetBySerial(string serial)
        {
            return _iogContext.Smcs
                .Include(smc => smc.SmcModel)
                .Include(smc => smc.Modem)
                .Include(smc => smc.Meters)
                .FirstOrDefault(smc => smc.Active && smc.Serial == serial);
        }

        public List<Smc> GetAllComissioned()
        {
            return _iogContext.Smcs.Where(smc => smc.Active && smc.Comissioned)
                .Include(smc => smc.SmcModel)
                .Include(smc => smc.Modem)
                .ToList();        }

        public List<Smc> GetAllNotComissioned()
        {
            return _iogContext.Smcs.Where(smc => smc.Active && !smc.Comissioned)
                .Include(smc => smc.SmcModel)
                .Include(smc => smc.Modem)
                .ToList();        }
    }
}