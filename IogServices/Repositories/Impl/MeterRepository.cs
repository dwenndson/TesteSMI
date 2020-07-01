using System;
using System.Collections.Generic;
using System.Linq;
using IogServices.Models.DAO;
using Microsoft.EntityFrameworkCore;

namespace IogServices.Repositories.Impl
{
    public class MeterRepository : IMeterRepository
    {
        private readonly IogContext _iogContext;

        public MeterRepository(IogContext iogContext)
        {
            this._iogContext = iogContext;
        }
        
        public List<Meter> GetAll()
        {
            return _iogContext.Meters
                .Include(meter => meter.MeterModel)
                .Include(meter => meter.MeterModel.Manufacturer)
                .Include(meter => meter.RateType)
                .Include(meter => meter.Smc)
                .Include(meter => meter.Modem)
                .Where(meter => meter.Active)
                .OrderByDescending(meter => meter.UpdatedAt)
                .ToList();
        }

        public Meter GetById(Guid id)
        {
            return _iogContext.Meters
                .Include(meter => meter.MeterModel)
                .Include(meter => meter.MeterModel.Manufacturer)
                .Include(meter => meter.RateType)
                .Include(meter => meter.Smc)
                .Include(meter => meter.Modem)
                .FirstOrDefault(meter => meter.Id == id && meter.Active);
        }
        
        public Meter GetActiveBySerial(string serial)
        {
            return _iogContext.Meters
                .Include(meter => meter.MeterModel)
                .Include(meter => meter.MeterModel.Manufacturer)
                .Include(meter => meter.RateType)
                .Include(meter => meter.Smc)
                .Include(meter => meter.Modem)
                .Include(meter => meter.MeterKeys)
                .FirstOrDefault(meter => meter.Serial == serial && meter.Active);
        }

        public Meter GetBySerial(string serial)
        {
            return _iogContext.Meters
                .Include(meter => meter.MeterModel)
                .Include(meter => meter.MeterModel.Manufacturer)
                .Include(meter => meter.RateType)
                .Include(meter => meter.Smc)
                .ThenInclude(smc => smc.Meters)
                .Include(meter => meter.Modem)
                .FirstOrDefault(meter => meter.Serial == serial);
        }

        public List<Meter> GetByModem(Modem modem)
        {
            return _iogContext.Meters.Where(meter => meter.Modem.DeviceEui == modem.DeviceEui && meter.Active).ToList();
        }

        public List<Meter> GetAllBySmc(string serial)
        {
            return _iogContext.Meters
                .Include(meter => meter.MeterModel)
                .Include(meter => meter.MeterModel.Manufacturer)
                .Include(meter => meter.RateType)
                .Include(meter => meter.Smc)
                .Include(meter => meter.Modem)
                .Where(meter => meter.Smc.Serial == serial).ToList();
        }

        public List<Meter> GetAllComissioned()
        {
            return _iogContext.Meters
                .Include(meter => meter.MeterModel)
                .Include(meter => meter.MeterModel.Manufacturer)
                .Include(meter => meter.RateType)
                .Include(meter => meter.Smc)
                .Include(meter => meter.Modem)
                .Where(meter => meter.Active && meter.Comissioned)
                .OrderByDescending(meter => meter.UpdatedAt)
                .ToList();
        }
        
    
    public List<Meter> GetAllNotComissioned()
        {
            return _iogContext.Meters
                .Include(meter => meter.MeterModel)
                .Include(meter => meter.MeterModel.Manufacturer)
                .Include(meter => meter.RateType)
                .Include(meter => meter.Smc)
                .Include(meter => meter.Modem)
                .Where(meter => meter.Active && !meter.Comissioned)
                .OrderByDescending(meter => meter.UpdatedAt)
                .ToList();        }

    public List<Meter> GetAllNotBelongingToASmc()
    {
        return _iogContext.Meters
            .Include(meter => meter.MeterModel)
            .Include(meter => meter.MeterModel.Manufacturer)
            .Include(meter => meter.RateType)
            .Include(meter => meter.Smc)
            .Include(meter => meter.Modem)
            .Where(meter => meter.Active && meter.Smc == null)
            .ToList();
    }

    public Meter Save(Meter meter)
        {
            _iogContext.Meters.Add(meter);
            _iogContext.SaveChanges();
            return meter;
        }

        public Meter Update(Meter meter)
        {
            _iogContext.Update(meter);
            _iogContext.SaveChanges();
            return meter;
        }
    }
}