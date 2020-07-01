using System;
using System.Collections.Generic;
using System.Linq;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace IogServices.Repositories.Impl
{
    public class MeterEnergyRepository : IMeterEnergyRepository
    {
        private readonly IogContext _iogContext;

        public MeterEnergyRepository(IogContext iogContext)
        {
            this._iogContext = iogContext;
        }

        public List<MeterEnergy> GetAllBySerial(string serial)
        {
            List<MeterEnergy> meterEnergies = this._iogContext.MeterEnergies
                .Include(meterEnergy => meterEnergy.Meter)
                .Where(meterEnergy =>
                    meterEnergy.Active && meterEnergy.Meter.Serial == serial && meterEnergy.Meter.Active)
                .OrderByDescending(meterEnergy => meterEnergy.ReadingTime)
                .ToList();
            return meterEnergies;
        }

        public MeterEnergy Save(MeterEnergy meterEnergy)
        {
            this._iogContext.MeterEnergies.Add(meterEnergy);
            this._iogContext.SaveChanges();
            return meterEnergy;
        }

        public MeterEnergy GetById(Guid id)
        {
            return this._iogContext.MeterEnergies.FirstOrDefault(meterEnergy =>
                meterEnergy.Id == id && meterEnergy.Active);
        }

        public MeterEnergy Update(MeterEnergy t)
        {
            throw new NotImplementedException();
        }

        public List<MeterEnergy> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}