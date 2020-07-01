using System;
using System.Collections.Generic;
using System.Linq;
using IogServices.Models.DAO;
using Microsoft.EntityFrameworkCore;

namespace IogServices.Repositories.Impl
{
    public class MeterModelRepository : IMeterModelRepository
    {
        
        private readonly IogContext _iogContext;

        public MeterModelRepository(IogContext iogContext)
        {
            _iogContext = iogContext;
        }
        
        public List<MeterModel> GetAll()
        {
            return _iogContext.MeterModels
                .Include(meterModel => meterModel.Manufacturer)
                .Where(meterModel => meterModel.Active)
                .OrderByDescending(meterModel => meterModel.UpdatedAt).ToList();
        }

        public MeterModel GetById(Guid id)
        {
          return _iogContext.MeterModels
                .Include(meterModelIterator => meterModelIterator.Manufacturer)
                .FirstOrDefault(meterModelIterator =>
                    meterModelIterator.Id == id && meterModelIterator.Active);
        }

        public MeterModel GetActiveByName(string name)
        {
           return _iogContext.MeterModels
                .Include(meterModelIterator => meterModelIterator.Manufacturer)
                .Include(meterModeIterator => meterModeIterator.Meters)
                .FirstOrDefault(meterModelIterator =>
                    meterModelIterator.Name == name && meterModelIterator.Active);
        }

        public MeterModel GetByName(string name)
        {
            return _iogContext.MeterModels
                .Include(meterModelIterator => meterModelIterator.Manufacturer)
                .Include(meterModeIterator => meterModeIterator.Meters)
                .FirstOrDefault(meterModelIterator =>
                    meterModelIterator.Name == name );
        }

        public MeterModel Save(MeterModel meterModel)
        {
            _iogContext.MeterModels.Add(meterModel);
            _iogContext.SaveChanges();
            return meterModel;
        }

        public MeterModel Update(MeterModel meterModel)
        {
            _iogContext.Update(meterModel);
            _iogContext.SaveChanges();
            return meterModel;
        }
    }
}